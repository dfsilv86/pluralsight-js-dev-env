using System;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace Walmart.Sgp.WebApi.App_Start
{
    /// <summary>
    /// ModelBinder para DateTime ser deserializado como DateKind.Utc.
    /// </summary>
    public class DateTimeModelBinder : IModelBinder
    {
        /// <summary>
        /// Determina se pode bindar o tipo informado.
        /// </summary>
        /// <param name="modelType">O tipo.</param>
        /// <returns>true caso possa bindar, false caso contrário.</returns>
        public static bool CanBindType(Type modelType)
        {
            return modelType == typeof(DateTime) || modelType == typeof(DateTime?);
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

            bindingContext.Model = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName)
                .ConvertTo(bindingContext.ModelType, RuntimeContext.Current.Culture);

            DateTime? theValue = (DateTime?)bindingContext.Model;

            if (theValue.HasValue && theValue.Value.Kind != DateTimeKind.Local)
            {
                theValue = DateTime.SpecifyKind(theValue.Value, DateTimeKind.Local);

                if (bindingContext.ModelType == typeof(DateTime))
                {
                    bindingContext.Model = theValue.Value;
                }
                else
                {
                    bindingContext.Model = theValue;
                }
            }

            bindingContext.ValidationNode.ValidateAllProperties = true;

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