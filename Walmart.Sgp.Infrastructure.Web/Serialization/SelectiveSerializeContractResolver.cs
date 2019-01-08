namespace Walmart.Sgp.Infrastructure.Web.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Walmart.Sgp.Infrastructure.Framework.Domain;

    /// <summary>
    /// Permite o retorno apenas das propriedades selecionadas.
    /// </summary>
    public class SelectiveSerializeContractResolver : CamelCasePropertyNamesContractResolver
    {
        #region Methods
        /// <summary>
        /// Creates a <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for the given <see cref="T:System.Reflection.MemberInfo" />.
        /// </summary>
        /// <param name="member">The member to create a <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for.</param>
        /// <param name="memberSerialization">The member's parent <see cref="T:Newtonsoft.Json.MemberSerialization" />.</param>
        /// <returns>
        /// A created <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for the given <see cref="T:System.Reflection.MemberInfo" />.
        /// </returns>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);           

            if (property.PropertyType == typeof(Money) || property.DeclaringType == typeof(Money))
            {
                property.Writable = true;
            }

            return property;
        }       
        #endregion
    }
}
