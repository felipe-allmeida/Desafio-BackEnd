using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;
using BuildingBlocks.Common.Extensions;

namespace BikeRental.API.Infrastructure.Serialization
{
    public class SnakeCaseQueryValueProviderFactory : IValueProviderFactory
    {
        public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            var valueProvider = new SnakeCaseQueryValueProvider(
                BindingSource.Query,
                context.ActionContext.HttpContext.Request.Query,
                CultureInfo.CurrentCulture);

            context.ValueProviders.Add(valueProvider);

            return Task.CompletedTask;
        }
    }

    public class SnakeCaseQueryValueProvider : QueryStringValueProvider, IValueProvider
    {
        public SnakeCaseQueryValueProvider(BindingSource bindingSource, IQueryCollection values, CultureInfo? culture)
            : base(bindingSource, values, culture)
        {

        }

        public override bool ContainsPrefix(string prefix)
        {
            return base.ContainsPrefix(prefix.ToSnakeCase());
        }

        public override ValueProviderResult GetValue(string key)
        {
            return base.GetValue(key.ToSnakeCase());
        }
    }

}
