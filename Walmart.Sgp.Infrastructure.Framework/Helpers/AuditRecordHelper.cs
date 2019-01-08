using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Framework.Helpers
{
    /// <summary>
    /// Helpers para manipular AuditRecord
    /// </summary>
    public static class AuditRecordHelper
    {
        /// <summary>
        /// Cria um mapeamento de um AuditRecord para um Dictionary{string, object} usando as propriedades informadas.
        /// </summary>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <param name="propertyNames">Os nomes das propriedades.</param>
        /// <returns>Um delegate compilado que realiza a cópia.</returns>
        public static Func<AuditRecord<TEntity>, Dictionary<string, object>> CreateMapper<TEntity>(IEnumerable<string> propertyNames)
        {
            /*
             Dictionary<string, object> result = new Dictionary<string, object>();
             result["IdAuditUser"] = auditRecord.IdAuditUser;
             result["DhAuditStamp"] = auditRecord.IdAuditUser;
             result["CdAuditKind"] = auditRecord.IdAuditUser;
             TEntity entity = (TEntity)auditRecord.Record;
             result["Prop1"] = entity.Prop1;
             result["Prop2"] = entity.Prop2;
             ...
             return result;
             */

            Type auditRecordType = typeof(AuditRecord<TEntity>);
            Type entityType = typeof(TEntity);

            var nullConstant = Expression.Constant(null, typeof(object));

            // estas não vem da entidade
            propertyNames = propertyNames.Except(new string[] { "IdAuditRecord", "IdAuditUser", "DhAuditStamp", "CdAuditKind" });

            // representa o acesso ao dicionario via indexador - foo[$param];
            var dictProperty = (from p in typeof(Dictionary<string, object>).GetProperties() let ip = p.GetIndexParameters() where ip.Length == 1 && ip.Single().ParameterType == typeof(string) select p).Single();

            // o parâmetro da função
            var auditRecordParameter = Expression.Parameter(auditRecordType, "auditRecord");

            // a declaração da variável com o resultado
            var localResult = Expression.Variable(typeof(Dictionary<string, object>), "result"); // Dictionary<string, object> result;

            // a declaração da variável com a entidade
            var localEntity = Expression.Variable(entityType, "entity"); // TEntity entity;

            // copia as propriedades da instancia de auditRecord para o resultado
            var arCopy = new string[] { "IdAuditUser", "DhAuditStamp", "CdAuditKind" }
                .Select(n => auditRecordType.GetProperty(n))
                .Select(gg =>
            {
                var dictSetter = Expression.Property(localResult, dictProperty, Expression.Constant(gg.Name, typeof(string)));

                Expression propGetter = Expression.MakeMemberAccess(auditRecordParameter, gg);

                if (gg.PropertyType.IsValueType)
                {
                    propGetter = Expression.Convert(propGetter, typeof(object));
                }
                else if (typeof(IFixedValue).IsAssignableFrom(gg.PropertyType))
                {
                    var test = Expression.MakeBinary(ExpressionType.NotEqual, nullConstant, propGetter);

                    var propAsObjectGetter = Expression.MakeMemberAccess(propGetter, typeof(IFixedValue).GetProperty("ValueAsObject"));

                    propGetter = Expression.Condition(test, propAsObjectGetter, nullConstant);
                }

                return Expression.Assign(dictSetter, propGetter); // result["Prop"] = record.Prop; OU result["Prop"] = null != record.Prop ? record.Prop.ValueAsObject : null;
            });

            // copia as propriedades da instancia de entidade para o resultado
            var entCopy = propertyNames
                .Select(n => entityType.GetProperty(n))
                .Select(gg =>
            {
                var dictSetter = Expression.Property(localResult, dictProperty, Expression.Constant(gg.Name, typeof(string)));

                Expression propGetter = Expression.MakeMemberAccess(localEntity, gg);

                if (gg.PropertyType.IsValueType)
                {
                    propGetter = Expression.Convert(propGetter, typeof(object));
                }
                else if (typeof(IFixedValue).IsAssignableFrom(gg.PropertyType))
                {
                    var test = Expression.MakeBinary(ExpressionType.NotEqual, nullConstant, propGetter);

                    var propAsObjectGetter = Expression.MakeMemberAccess(propGetter, typeof(IFixedValue).GetProperty("ValueAsObject"));

                    propGetter = Expression.Condition(test, propAsObjectGetter, nullConstant);
                }

                return Expression.Assign(dictSetter, propGetter); // result["Prop"] = entity.Prop; OU result["Prop"] = null != entity.Prop ? entity.Prop.ValueAsObject : null;
            });

            // linhas da função
            var expressionBody = new Expression[] { Expression.Assign(localResult, Expression.New(typeof(Dictionary<string, object>))) }  // Dictionary<string, object> result = new Dictionary<string, object>();
                .Concat(arCopy) // result["Prop"] = record.Prop; ...
                .Concat(new Expression[] { Expression.Assign(localEntity, Expression.MakeMemberAccess(auditRecordParameter, auditRecordType.GetProperty("Entity", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))) }) // TEntity entity = record.Entity;
                .Concat(entCopy) // result["Prop"] = entity.Prop; ...
                .Concat(new Expression[] { localResult });  // return result;

            // monta a função em si
            var body = Expression.Block(
                typeof(Dictionary<string, object>), 
                new ParameterExpression[] { localResult, localEntity }, 
                expressionBody); // (record) => { ... }

            // lambda com a função para compilar
            Expression<Func<AuditRecord<TEntity>, Dictionary<string, object>>> expr = LambdaExpression.Lambda<Func<AuditRecord<TEntity>, Dictionary<string, object>>>(body, new ParameterExpression[] { auditRecordParameter });

            return expr.Compile();
        }
    }
}
