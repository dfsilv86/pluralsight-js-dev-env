using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CodigoDescricao = System.Tuple<int, string>;

namespace Walmart.Sgp.Infrastructure.Data.Dtos
{
    /// <summary>
    /// Utilitários para mapeamento de DTOs, que estão isolados na camada de dados, para entidades do domínio.
    /// </summary>
    public static class DtoHelper
    {
        /// <summary>
        /// Cria uma nova instância de T definindo as propriedades código e nome que estão definidos diretamente numa propriedade (texto) no formato "CÓDIGO - NOME".
        /// </summary>
        /// <typeparam name="T">O tipo a ser criado.</typeparam>
        /// <param name="texto">O texto de onde serão extraídos código e nome.</param>
        /// <param name="seletorCodigo">O seletor de código.</param>
        /// <param name="seletorTexto">O seletor de texto.</param>
        /// <returns>A instância do tipo criada e com as propriedades de código e nome configuradas.</returns>
        public static T DefinirCodigoDescricao<T>(string texto, Expression<Func<T, object>> seletorCodigo, Expression<Func<T, string>> seletorTexto)
           where T : class, new()
        {
            var codigoDescricao = ObterCodigoDescricao(texto);
            if (codigoDescricao == null)
            {
                return null;
            }

            var result = new T();
            var typeT = typeof(T);

            typeT.GetProperty(GetPropertyName(seletorCodigo)).SetValue(result, codigoDescricao.Item1);
            typeT.GetProperty(GetPropertyName(seletorTexto)).SetValue(result, codigoDescricao.Item2);
            return result;
        }

        /// <summary>
        /// Processa o valor texto no formato "CÓDIGO - NOME" e chama a action passando as informações de forma fortemente tipada.
        /// </summary>
        /// <param name="texto">O texto de onde serão extraídos código e nome.</param>
        /// <param name="action">A action para tratar código e descrição localizados em texto.</param>
        public static void DefinirCodigoDescricao(string texto, Action<CodigoDescricao> action)
        {
            var codigoDescricao = ObterCodigoDescricao(texto);
            if (codigoDescricao != null)
            {
                action(codigoDescricao);
            }
        }

        private static CodigoDescricao ObterCodigoDescricao(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return null;
            }

            var parts = texto.Split(new[] { " - " }, StringSplitOptions.None);
            if (parts.Length < 2)
            {
                return null;
            }

            int code;
            if (!int.TryParse(parts[0], out code))
            {
                code = 0;
            }

            var description = string.Join(" - ", parts.Skip(1).ToArray());

            return new CodigoDescricao(code, description);
        }

        private static string GetPropertyName<TRoot, TPropType>(Expression<Func<TRoot, TPropType>> exp)
        {
            MemberExpression body = exp.Body as MemberExpression;

            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)exp.Body;
                body = ubody.Operand as MemberExpression;
            }

            return body.Member.Name;
        }
    }
}
