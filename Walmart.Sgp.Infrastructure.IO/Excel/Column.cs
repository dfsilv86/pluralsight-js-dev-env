using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Infrastructure.IO.Excel.Specs;

namespace Walmart.Sgp.Infrastructure.IO.Excel
{
    /// <summary>
    /// Representa uma coluna do arquivo.
    /// </summary>
    public class Column
    {
        /// <summary>
        /// Obtém ou define configuração da coluna (tipo, precisão, ...).
        /// </summary>
        public ColumnMetadata Metadata { get; set; }

        /// <summary>
        /// Obtém ou define Conteúdo da coluna.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Valida a coluna com base na configuração (tipo, precisão, ...).
        /// </summary>
        /// <returns>Retorna a lista de validações não satisfeitas.</returns>
        public IEnumerable<string> Validate()
        {
            var notSatisfiedSpecReasons = new List<string>();

            if (!IsEmpty() || !this.Metadata.IgnoreEmpty)
            {
                if (this.Metadata.ColumnType == typeof(decimal))
                {
                    ValidateDecimalColumn(notSatisfiedSpecReasons);
                }
                else
                {
                    ValidateColumn(notSatisfiedSpecReasons);
                }
            }

            ApplyCustomValidates(notSatisfiedSpecReasons);

            return notSatisfiedSpecReasons;
        }

        /// <summary>
        /// Verifica se o valor da coluna está vazio.
        /// </summary>
        /// <returns>Retorna true caso a coluna esteja vazia, do contrário retorna false.</returns>
        public bool IsEmpty()
        {
            return Value == null || string.IsNullOrWhiteSpace(Value.ToString());
        }

        private void ValidateDecimalColumn(IList<string> notSatisfiedSpecException)
        {
            var typeSpec = new ColumnTypeSpec();
            var typeSpecResult = typeSpec.IsSatisfiedBy(this);

            if (!typeSpecResult.Satisfied)
            {
                notSatisfiedSpecException.Add(typeSpecResult.Reason);
                return;
            }

            var decimalIntegerPartSpec = new ColumnDecimalIntegerPartSpec();
            var decimalFractionalPartSpec = new ColumnDecimalFractionalPartSpec();

            var decimalIntegerPartSpecResult = decimalIntegerPartSpec.IsSatisfiedBy(this);
            var decimalFractionalPartSpecResult = decimalFractionalPartSpec.IsSatisfiedBy(this);

            if (!decimalIntegerPartSpecResult.Satisfied)
            {
                notSatisfiedSpecException.Add(decimalIntegerPartSpecResult.Reason);
            }

            if (!decimalFractionalPartSpecResult.Satisfied)
            {
                notSatisfiedSpecException.Add(decimalFractionalPartSpecResult.Reason);
            }
        }

        private void ValidateColumn(IList<string> notSatisfiedSpecException)
        {
            var typeSpec = new ColumnTypeSpec();
            var lengthSpec = new ColumnLengthSpec();

            var typeSpecResult = typeSpec.IsSatisfiedBy(this);
            var lengthSpecResult = lengthSpec.IsSatisfiedBy(this);

            if (!typeSpecResult.Satisfied)
            {
                notSatisfiedSpecException.Add(typeSpecResult.Reason);
            }

            if (!lengthSpecResult.Satisfied)
            {
                notSatisfiedSpecException.Add(lengthSpecResult.Reason);
            }
        }

        private void ApplyCustomValidates(IList<string> notSatisfiedSpecReasons)
        {
            if (this.Metadata.CustomValidate == null)
            {
                return;
            }

            foreach (var spec in this.Metadata.CustomValidate)
            {
                var result = spec.IsSatisfiedBy(this);

                if (!result.Satisfied)
                {
                    notSatisfiedSpecReasons.Add(result.Reason);
                }
            }
        }
    }
}
