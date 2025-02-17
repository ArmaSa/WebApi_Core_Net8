using System.Linq.Expressions;

namespace InvoiceAppWebApi.FrameworkExtention
{
    public static class Helper
    {
        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }
    }

    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }

        public static Expression<Func<T, bool>> Or<T>(
            this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }

        private static Expression<Func<T, bool>> Compose<T>(
            this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second,
            Func<Expression, Expression, BinaryExpression> merge)
        {
            var param = Expression.Parameter(typeof(T), "x");

            var body = merge(
                Expression.Invoke(first, param),
                Expression.Invoke(second, param)
            );

            return Expression.Lambda<Func<T, bool>>(body, param);
        }
    }

}
