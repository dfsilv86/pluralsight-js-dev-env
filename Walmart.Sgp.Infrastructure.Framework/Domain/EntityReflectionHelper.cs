using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Helpers para realizar reflection em entities.
    /// </summary>
    /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
    internal static class EntityReflectionHelper<TEntity>
    {
        /// <summary>
        /// Descobre quais são as propriedades da entidade que são entidades também e que fazer parte deste aggregate root.
        /// </summary>
        /// <returns>As propriedades</returns>
        public static IEnumerable<PropertyInfo> DiscoverChildrenEntitiesProperites()
        {
            var result = new List<PropertyInfo>();
            var properties = typeof(TEntity).GetProperties();
            Func<Type, bool> isChildEntity = (t) => typeof(IEntity).IsAssignableFrom(t) && !typeof(IAggregateRoot).IsAssignableFrom(t);

            foreach (var p in properties)
            {
                if (isChildEntity(p.PropertyType) || p.PropertyType.GetGenericArguments().Any(t => isChildEntity(t)))
                {
                    result.Add(p);
                }
            }

            return result;
        }

        /// <summary>
        /// Copia os valores das proprieades respeitando a projeção informada.
        /// </summary>
        /// <typeparam name="TModel">O tipo do modelo que será retornado.</typeparam>
        /// <param name="projection">A projeção.</param>
        /// <param name="entities">As entidades de onde serão copiados os valores.</param>
        /// <returns>Os modelos preenchidos.</returns>
        public static IEnumerable<TModel> CopyProperties<TModel>(string projection, IEnumerable<TEntity> entities)
        {
            var result = new List<TModel>();
            var modelTypeIsPrimitive = typeof(TModel).IsPrimitive;
            var entityType = typeof(TEntity);

            foreach (var entity in entities)
            {
                // Quando for a projeção de apenas uma proprieade, como um ID ou uma descrição.
                if (modelTypeIsPrimitive)
                {
                    result.Add((TModel)GetProperty(entityType, projection, projection).GetValue(entity));
                }
                else
                {
                    var model = (TModel)Activator.CreateInstance(typeof(TModel));
                    CopyProperties(projection, entity, model);

                    result.Add(model);
                }
            }

            return result;
        }

        /// <summary>
        /// Copia os valores das proprieades respeitando a projeção informada.
        /// </summary>
        /// <typeparam name="TFrom">O tipo de onde serão copiados os valores.</typeparam>
        /// <typeparam name="TTo">O tipo para onde serão copiados os valores.</typeparam>
        /// <param name="projection">A projeção.</param>
        /// <param name="from">De onde serão copiados os valores.</param>
        /// <param name="to">Para onde serão copiados os valores.</param>        
        public static void CopyProperties<TFrom, TTo>(string projection, TFrom from, TTo to)
        {
            var normalizedProjection = projection.ToUpperInvariant();
            var projectionProperties = normalizedProjection.Split(new string[] { " = @", ", " }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            var fromProperties = from.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);            
            var toProperites = to.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite && projectionProperties.Contains(p.Name.ToUpperInvariant())).ToArray();

            for (int i = 0; i < projectionProperties.Length; i += 2)
            {
                var fromPropertyName = i < projectionProperties.Length - 1 ? projectionProperties[i + 1] : projectionProperties[i];
                var fromProperty = fromProperties.FirstOrDefault(p => p.Name.ToUpperInvariant() == fromPropertyName);
                var toPropertyName = projectionProperties[i];
                var toProperty = toProperites.FirstOrDefault(p => p.Name.ToUpperInvariant() == toPropertyName);

                toProperty.SetValue(to, fromProperty.GetValue(from));
            }            
        }

        /// <summary>
        /// Copia os valores das proprieades respeitando a projeção informada.
        /// </summary>
        /// <typeparam name="TFrom">O tipo de onde serão copiados os valores.</typeparam>
        /// <typeparam name="TTo">O tipo para onde serão copiados os valores.</typeparam>
        /// <param name="projection">A projeção.</param>
        /// <param name="from">De onde serão copiados os valores.</param>
        /// <param name="to">Para onde serão copiados os valores.</param> 
        public static void CopyProperties<TFrom, TTo>(string projection, TFrom from, IEnumerable<TTo> to)
        {
            foreach (var t in to)
            {
                CopyProperties(projection, from, t);
            }
        }

        /// <summary>
        /// Obtém uma propriedade do tipo informado.
        /// </summary>
        /// <param name="type">O tipo.</param>
        /// <param name="propertyName">O nome da propriedade.</param>
        /// <param name="propertyExpression">A expressão onde a propriedade foi utilizada.</param>
        /// <returns>A propriedade.</returns>
        public static PropertyInfo GetProperty(Type type, string propertyName, string propertyExpression)
        {
            var p = type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);

            if (p == null)
            {
                throw new InvalidOperationException(Texts.PropertyNotFoundOnFilterArgs.With(propertyName, propertyExpression));
            }

            return p;
        }
    }
}
