using System;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.WebApi.App_Start
{
    /// <summary>
    /// ModelBinder para DateTime ser deserializado como DateKind.Utc.
    /// </summary>
    public class FixedValuesModelBinder : IModelBinder
    {
        /// <summary>
        /// Determina se pode bindar o tipo informado.
        /// </summary>
        /// <param name="modelType">O tipo.</param>
        /// <returns>true caso possa bindar, false caso contrário.</returns>
        public static bool CanBindType(Type modelType)
        {
            return typeof(IFixedValue).IsAssignableFrom(modelType);
        }

        /// <summary>
        /// Binds the model to a value by using the specified controller context and binding context.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <returns>true if model binding is successful; otherwise, false.</returns>
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            ValidateBindingContext(bindingContext);

            if (!bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName) ||
                !CanBindType(bindingContext.ModelType))
            {
                return false;
            }

            var val = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (typeof(FixedValuesBase<string>).IsAssignableFrom(bindingContext.ModelType))
            {
                bindingContext.Model = FixedValuesHelper.GetConversionOperator(bindingContext.ModelType).Invoke(null, new object[] { val.RawValue });
            }
            else if (typeof(FixedValuesBase<int>).IsAssignableFrom(bindingContext.ModelType))
            {
                bindingContext.Model = FixedValuesHelper.GetConversionOperator(bindingContext.ModelType).Invoke(null, new object[] { Convert.ToInt32(val.RawValue) });
            }
            else if (typeof(FixedValuesBase<short?>).IsAssignableFrom(bindingContext.ModelType))
            {
                bindingContext.Model = FixedValuesHelper.GetConversionOperator(bindingContext.ModelType).Invoke(null, new object[] { Convert.ToInt16(val.RawValue) });
            }

            return true;
        }

        private static void ValidateBindingContext(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException("bindingContext");
            }

            if (bindingContext.ModelMetadata == null)
            {
                throw new ArgumentException(Texts.ModelMetadataCannotBeNull, "bindingContext");
            }
        }
    }
}