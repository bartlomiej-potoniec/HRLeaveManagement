using System.ComponentModel;
using System.Reflection;
using System.Linq.Expressions;
using FluentValidation;

namespace HRLeaveManagement.BlazorUI.Extensions;

public static class ValidationExtension
{
    public static IRuleBuilderOptions<TViewModel, TProperty> WithDisplayName<TViewModel, TProperty>(
        this IRuleBuilderOptions<TViewModel, TProperty> rule,
        Expression<Func<TViewModel, TProperty>> expression)
    {
        string propertyName = GetPropertyName(expression);
        string displayName = GetDisplayName<TViewModel>(propertyName);

        return rule.WithName(displayName);
    }

    private static string GetPropertyName<TViewModel, TProperty>(Expression<Func<TViewModel, TProperty>> expression)
    {
        if (expression.Body is MemberExpression memberExpression)
        {
            return memberExpression.Member.Name;
        }

        throw new ArgumentException("Incorrect lambda expression. Expected reference to property");
    }

    private static string GetDisplayName<TViewModel>(string propertyName)
    {
        var property = typeof(TViewModel).GetProperty(propertyName);

        if (property is null || !(Attribute.IsDefined(property, typeof(DisplayNameAttribute))))
        {
            return propertyName;
        }

        var displayNameAttribute = property.GetCustomAttribute<DisplayNameAttribute>();
        return displayNameAttribute?.DisplayName ?? propertyName;
    }
}
