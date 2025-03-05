using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using InvoiceAppWebApi.Data;
using InvoiceAppWebApi.Common;
using InvoiceAppWebApi.FrameworkExtention;
using Microsoft.Extensions.Options;

namespace InvoiceAppWebApi.Services.ServiceBase
{
    public abstract class CrudBase<TEntity, TModel> : ReadBase<TEntity, TModel>, ICrudBase<TEntity, TModel> where TEntity : class, IKey64 where TModel : class, IKey64
    {
        private Int64 _minimumValue = default(Int64);
        public CrudBase(AutoMapper.IMapper mapper, ILogger<ReadBase<TEntity, TModel>> logger, IUnitOfWork<TEntity> uow, IOptions<ApplicationSettings> appSettings) : base(mapper, logger, uow, appSettings)
        {
        }

        public virtual async Task<ServiceResultInfo> CreateAsync(TModel modelItem, bool savechange = true)
        {
            var serviceResult = new ServiceResultInfo();

            var entityItem = _mapper.Map<TEntity>(modelItem);
            await base._entities.AddAsync(entityItem).ConfigureAwait(false);

            if (savechange)
            {
                await _uow.SaveChangesAsync(true).ConfigureAwait(false);

                if (entityItem.Id.CompareTo(_minimumValue) > 0)
                {
                    modelItem.Id = entityItem.Id;
                    serviceResult.Succeeded = true;
                    serviceResult.Messages = "Successfuly!";
                }
                else
                {
                    serviceResult.Succeeded = false;
                    serviceResult.Messages = "Failed!";
                }
            }
            else
            {
                serviceResult.Succeeded = true;
            }

            return serviceResult;
        }

        public virtual async Task<ServiceResultInfo> CreateAsync(IEnumerable<TModel> modelItems, bool savechange = true)
        {
            var serviceResult = new ServiceResultInfo();

            var entityItems = _mapper.Map<List<TEntity>>(modelItems);
            await _entities.AddRangeAsync(entityItems).ConfigureAwait(false);

            if (savechange)
            {
                await _uow.SaveChangesAsync(true).ConfigureAwait(false);
                serviceResult.Messages = "Successfuly added!";
            }

            serviceResult.Succeeded = true;

            return serviceResult;
        }

        public virtual async Task<ServiceResultInfo> UpdateAsync(TModel modelItem, bool savechange = true)
        {
            var serviceResult = new ServiceResultInfo();

            var entityItem = this._mapper.Map<TEntity>(modelItem);
            _entities.Update(entityItem);

            if (savechange)
            {
                await this._uow.SaveChangesAsync(true).ConfigureAwait(false);
                serviceResult.Messages = "Successfuly updated!";
            }

            serviceResult.Succeeded = true;

            return serviceResult;
        }

        public virtual async Task<ServiceResultInfo> UpdateAsync(IEnumerable<TModel> modelItems, bool savechange = true)
        {
            var serviceResult = new ServiceResultInfo();

            var entityItems = this._mapper.Map<List<TEntity>>(modelItems);
            _entities.UpdateRange(entityItems);

            if (savechange)
            {
                await this._uow.SaveChangesAsync(true).ConfigureAwait(false);

                serviceResult.Messages = "Successfuly updated!";
            }

            serviceResult.Succeeded = true;

            return serviceResult;
        }

        public virtual async Task<ServiceResultInfo> DeleteByKeysAsync(Int64 id, bool savechange = true)
        {
            var serviceResult = new ServiceResultInfo();

            var entityItem = await this._entities.FindAsync(id).ConfigureAwait(false);

            if (entityItem != null)
            {
                _entities.Remove(entityItem);

                if (savechange)
                {
                    await this._uow.SaveChangesAsync(true).ConfigureAwait(false);
                    serviceResult.Messages = "Successfuly Removed!";
                }

                serviceResult.Succeeded = true;
            }
            else
            {
                serviceResult.Succeeded = false;
                serviceResult.Messages = "Failed to remove!";
            }

            return serviceResult;
        }

        public virtual async Task<ServiceResultInfo> DeleteByKeysAsync(List<Int64> ids, bool savechange = true)
        {
            var serviceResult = new ServiceResultInfo();

            var entityItems = await this._entities.Where(c => ids.OrEmptyIfNull().Contains(c.Id)).ToListAsync().ConfigureAwait(false);

            if (entityItems?.Any() == true)
            {
                _entities.RemoveRange(entityItems);

                if (savechange)
                {
                    await this._uow.SaveChangesAsync(true).ConfigureAwait(false);
                    serviceResult.Messages = "Successfuly Removed!";
                }

                serviceResult.Succeeded = true;
            }
            else
            {
                serviceResult.Succeeded = false;
                serviceResult.Messages = "Failed to remove!";
            }

            return serviceResult;
        }

        public virtual async Task<ServiceResultInfo> DeleteRangeAsync(List<TEntity> entityItems, bool savechange = true)
        {
            var serviceResult = new ServiceResultInfo();

            if (entityItems?.Any() == true)
            {
                _entities.RemoveRange(entityItems);

                if (savechange)
                {
                    await this._uow.SaveChangesAsync(true).ConfigureAwait(false);
                    serviceResult.Messages = "Successfuly Removed!";
                }

                serviceResult.Succeeded = true;
            }
            else
            {
                serviceResult.Succeeded = false;
                serviceResult.Messages = "Failed to remove!";
            }

            return serviceResult;
        }
    }
}