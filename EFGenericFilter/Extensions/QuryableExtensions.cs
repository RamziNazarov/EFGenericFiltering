using System.Linq.Expressions;
using System.Reflection;
using EFGenericFilter.Attributes;
using EFGenericFilter.Enums;

namespace EFGenericFilter.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TSource> Filter<TSource, TParameters>(this IQueryable<TSource> source,
        TParameters parameters)
    {
        var parameterProperties = typeof(TParameters).GetProperties().Select(x=> new
            {
                Ignore = x.GetCustomAttribute<IgnoreFilteringAttribute>() != null,
                Operation = x.GetCustomAttribute<OperationTypeAttribute>()?.Operation ?? Operations.Equal,
                x.GetCustomAttribute<CompareWithAttribute>()?.CompareWith,
                Value = x.GetValue(parameters),
                x.Name
            })
            .Where(x => !x.Ignore && x.Value != null).ToList();
        if (!parameterProperties.Any())
            return source;
        Expression comparison = null;
        var type = typeof(TSource);
        var parameter = Expression.Parameter(type, "par");

        foreach (var filterParameter in parameterProperties)
        {
            // get property from string name
            var prop = type.GetProperty(filterParameter.CompareWith ?? filterParameter.Name, 
                                                BindingFlags.Public 
                                                | BindingFlags.Instance 
                                                | BindingFlags.IgnoreCase);

            // prepare the lambda representing this filter

            var propAccessExpr = Expression
                                                .MakeMemberAccess(parameter, prop!);

            var expressionValue = ConvertValueType(Expression.Constant(filterParameter.Value), prop!.PropertyType);

            if (comparison == null)
            {
                comparison = GetComparator(filterParameter.Operation, 
                    propAccessExpr, 
                    expressionValue);
            }
            else
            {
                comparison = Expression.And(GetComparator(filterParameter.Operation,
                    propAccessExpr, 
                    expressionValue),comparison);
            }

        }
        var filterExpr = Expression.Lambda<Func<TSource, bool>>(
            comparison ?? Expression.Constant(true), parameter);

        return source.Where(filterExpr);
        
    }
    
    private static Expression GetComparator(
        Operations operation, 
        Expression left, 
        Expression right)
    {
        switch (operation)
        {
            case Operations.GreaterThan:
                return Expression.GreaterThan(right,  left,false,null);
            case Operations.LessThan:
                return Expression.GreaterThan( left , right ,false, null);
            case Operations.GreaterThanOrEqual:
                return Expression.GreaterThanOrEqual(right, left,false,null);
            case Operations.LessThanOrEqual:
                return Expression.GreaterThanOrEqual( left , right,false,null);
            case Operations.Equal:
                return Expression.Equal(left, right);
            case Operations.NotEqual:
                return Expression.NotEqual(left, right);
            case Operations.Contains:
                return Expression.Call(left, Operations.Contains.ToString(), null, right, Expression.Constant(StringComparison.InvariantCultureIgnoreCase));
            default:
                return null;
        }
    }

    private static UnaryExpression ConvertValueType(Expression expression, Type type)
    {
        return Expression.Convert(expression, type);
    }
}