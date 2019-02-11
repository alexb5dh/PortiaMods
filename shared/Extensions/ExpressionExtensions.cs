using System;
using System.Linq.Expressions;
using System.Reflection;

public static class ExpressionExtensions
{
    public static PropertyInfo TryGetPropertyInfo<TSource, TProperty>(this Expression<Func<TSource, TProperty>> expression)
    {
        var member = expression.Body as MemberExpression;
        if (!(member?.Member is PropertyInfo propertyInfo))
            return null;
        if (propertyInfo.ReflectedType == null || !propertyInfo.ReflectedType.IsAssignableFrom(typeof(TSource)))
            return null;

        return propertyInfo;
    }
}