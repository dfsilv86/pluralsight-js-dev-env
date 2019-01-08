namespace Walmart.Sgp.WebApi.App_Start
{
    using System;
    using System.Web.Http;
    using System.Web.Http.ModelBinding;

    /// <summary>
    /// Provedor de ModelBinder específico para IFixedValues.
    /// </summary>
    public class FixedValuesModelBinderProvider : ModelBinderProvider
    {
        private readonly FixedValuesModelBinder binder = new FixedValuesModelBinder();

        /// <summary>
        /// Obtém o ModelBinder para o tipo informado.
        /// </summary>
        /// <param name="configuration">A configuração da requisição.</param>
        /// <param name="modelType">O tipo.</param>
        /// <returns>O ModelBinder.</returns>
        public override IModelBinder GetBinder(HttpConfiguration configuration, Type modelType)
        {
            if (FixedValuesModelBinder.CanBindType(modelType))
            {
                return binder;
            }

            return null;
        }
    }
}