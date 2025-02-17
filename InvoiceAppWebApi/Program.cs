using InvoiceAppWebApi.Data;
using InvoiceAppWebApi.Domain;
using InvoiceAppWebApi.FrameworkExtention;
using InvoiceAppWebApi.Service.Mapping;
using InvoiceWebApi.Data.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using InvoiceAppWebApi.Common;
using Microsoft.Extensions.Configuration;
using static InvoiceAppWebApi.Common.ApplicationSettings;
using System.Runtime;
using System.Net;
using System.Security.Claims;
using Microsoft.VisualBasic;

namespace InvoiceWebApi
{
    public class Program
    {
        public static IServiceProvider ServiceProvider { get; }

        public static async Task Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            var ConfigurationBinder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var appSettings = new ApplicationSettings();
            ConfigurationBinder.Bind(appSettings);

            builder.Services.Configure<ApplicationSettings>(ConfigurationBinder); 
            builder.Services.AddSingleton(appSettings);

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Invoice API",
                    Version = "v1",
                    Description = "An API for managing invoices",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Ali Arena",
                        Email = "www.programmerTips.com",
                        Url = new Uri("https://programmerTips.com")
                    }
                });

                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token."
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
            },
                        new string[] {}
                    }
                });
            });

            builder.Services.AddDbContext();
            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            builder.Services.AddAutoMapper(typeof(MapperConfig));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var secretKey = Encoding.UTF8.GetBytes(appSettings.Jwt.SecretKey);
                var encryptionKey = Encoding.UTF8.GetBytes(appSettings.Jwt.EncryptKey);

                var validationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero, // default: 5 min
                    RequireSignedTokens = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),

                    RequireExpirationTime = true,
                    ValidateLifetime = true,

                    ValidateAudience = true, //default : false
                    ValidAudience = appSettings.Jwt.Audience,

                    ValidateIssuer = true, //default : false
                    ValidIssuer = appSettings.Jwt.Issuer,

                    TokenDecryptionKey = new SymmetricSecurityKey(encryptionKey)
                };

                options.TokenValidationParameters = validationParameters;
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception != null)
                            throw new Exception($"Authentication failed. {HttpStatusCode.Unauthorized}");

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = async context =>
                    {
                        var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
                        var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                        if (claimsIdentity.Claims?.Any() != true)
                            context.Fail("This token has no claims.");

                        var securityStamp = claimsIdentity.FindFirst(new ClaimsIdentityOptions().SecurityStampClaimType).Value;
                        if (string.IsNullOrEmpty(securityStamp))
                            context.Fail("This token has no security stamp");

                        var userId = claimsIdentity.GetUserId<int>();
                        var user = await userManager.FindByIdAsync(userId.ToString());

                        if (user == null)
                        {
                            context.Fail("User not found.");
                            return;
                        }

                        var expectedSecurityStamp = await userManager.GetSecurityStampAsync(user);
                        if (expectedSecurityStamp != securityStamp)
                        {
                            context.Fail("Token security stamp is not valid.");
                            return;
                        }

                        //if (!user.IsActive)
                        //{
                        //    context.Fail("User is not active.");
                        //    return;
                        //}

                        //user.LastLoginDate = DateTime.UtcNow;
                        await userManager.UpdateAsync(user);
                    },
                    OnChallenge = context =>
                    {
                        if (context.AuthenticateFailure != null)
                            throw new Exception($"Authentication failure. {HttpStatusCode.Unauthorized}");
                        throw new Exception($"You are unauthorized to access this resource. {HttpStatusCode.Unauthorized}");
                    }
                };
            });

            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(identityOptions =>
            {
                //Password Settings
                identityOptions.Password.RequireDigit = appSettings.IdentitySettings.PasswordRequireDigit;
                identityOptions.Password.RequiredLength = appSettings.IdentitySettings.PasswordRequiredLength;
                identityOptions.Password.RequireNonAlphanumeric = appSettings.IdentitySettings.PasswordRequireNonAlphanumeric; 
                identityOptions.Password.RequireUppercase = appSettings.IdentitySettings.PasswordRequireUppercase;
                identityOptions.Password.RequireLowercase = appSettings.IdentitySettings.PasswordRequireLowercase;

                //UserName Settings
                identityOptions.User.RequireUniqueEmail = appSettings.IdentitySettings.RequireUniqueEmail;
            })
                .AddEntityFrameworkStores<InvoiceDbContext>()
                .AddDefaultTokenProviders();


            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();

            builder.Logging.AddConsole();

            builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>((container) =>
            {
                container.AddServices();
            });

            var app = builder.Build();

            app.Logger.LogInformation("Starting Application");

            app.ConfigureCustomExceptionMiddleware();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

                await SeedData.SeedAdminUserAsync(userManager, roleManager);
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.DefaultModelsExpandDepth(-1);
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Invoice API v1");
                    options.RoutePrefix = string.Empty; 
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
        
            app.UseHttpsRedirection();

            app.Run();
        }
    }

    public static class CustomExceptionMiddlewareExtensions
    {
        public static void ConfigureCustomExceptionMiddleware(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}

