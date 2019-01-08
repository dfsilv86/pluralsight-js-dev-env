using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace System
{
    /// <summary>
    /// Extension methods para string.
    /// </summary>
    public static class StringExtensions
    {
        // Nitriq reclamou do nome
        private static IEnumerable<string> s_executableFileExtensions = new string[]
            {
                ".EXE", ".PIF", ".APPLICATION", ".GADGET", ".MSI", ".MSP", ".COM", ".SCR", ".HTA", ".CPL", ".MSC", ".JAR", ".BAT", ".CMD", ".VB",
                ".VBS", ".VBE", ".JS", ".JSE", ".WS", ".WSF", ".WSC", ".WSH", ".PS1", ".PS1XML", ".PS2", ".PS2XML", ".PSC1", ".PSC2", ".MSH",
                ".MSH1", ".MSH2", ".MSHXML", ".MSH1XML", ".MSH2XML", ".SCF", ".LNK", ".INF", ".REG"
            };

        /// <summary>
        /// Formata a string informada utilizando InvariantCulture.
        /// </summary>
        /// <param name="value">A string.</param>
        /// <param name="args">Os argumentos para formatação da string.</param>
        /// <returns>A string formatada.</returns>
        public static string With(this string value, params object[] args)
        {
            return string.Format(CultureInfo.InvariantCulture, value, args);
        }

        /// <summary>
        /// Escapa { e } para ser utilizado no format.
        /// </summary>
        /// <param name="value">A string.</param>
        /// <returns>Uma <c>string</c> com os parenteses escapados.</returns>
        public static string EscapeForFormat(this string value)
        {
            if (null == value)
            {
                return null;
            }

            return value.Replace("{", "{{").Replace("}", "}}");
        }

        /// <summary>
        /// Determina se a string corresponde a alguma extensão de arquivo executável.
        /// </summary>
        /// <param name="value">A string.</param>
        /// <returns>True caso seja uma extensão executável, False caso contrário.</returns>
        public static bool IsExecutableExtension(this string value)
        {
            if (null == value)
            {
                return false;
            }

            return s_executableFileExtensions.Contains(value.ToUpperInvariant());
        }

        /// <summary>
        /// Concatena os itens da lista como palavras, adicionando ',' entre os elementos e ' e ' entre o penúltimo e último elemento.
        /// </summary>
        /// <param name="words">As palavras.</param>
        /// <returns>As palavras concatenadas.</returns>
        public static string JoinWords(this IEnumerable<string> words)
        {
            return JoinWords(words, Texts.And);
        }

        /// <summary>
        /// Concatena os itens da lista como palavras, adicionando ',' entre os elementos e ' e ' entre o penúltimo e último elemento.
        /// </summary>
        /// <param name="words">As palavras.</param>
        /// <param name="lastSeparator">O último separador.</param>
        /// <returns>As palavras concatenadas.</returns>
        public static string JoinWords(this IEnumerable<string> words, string lastSeparator)
        {
            var wordsCount = words == null ? 0 : words.Count();

            if (wordsCount == 0)
            {
                return string.Empty;
            }

            var lastWord = words.LastOrDefault();

            if (wordsCount == 1)
            {
                return lastWord;
            }

            var firstWords = words.Take(wordsCount - 1);

            return "{0} {1} {2}".With(string.Join(", ", firstWords), lastSeparator, lastWord);
        }

        /// <summary>
        /// Concatena os itens da lista como palavras, adicionando ',' entre os elementos e ' e ' entre o penúltimo e último elemento.
        /// </summary>
        /// <param name="words">As palavras.</param>
        /// <returns>As palavras concatenadas.</returns>
        public static string JoinWords(this IEnumerable<int> words)
        {
            if (words == null)
            {
                return string.Empty;
            }

            return words.Select(w => w.ToString(CultureInfo.InvariantCulture)).JoinWords();
        }

        /// <summary>
        /// Obtém a palavra especificada pelo índice.
        /// </summary>
        /// <param name="str">A string.</param>
        /// <param name="wordIndex">O índice da palavra.</param>
        /// <returns>A (n) palavra da string.</returns>
        public static string GetWord(this string str, int wordIndex)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }

            var words = str.Split(' ');

            return wordIndex >= words.Length ? null : words[wordIndex];
        }

        /// <summary>
        /// Converte para uma string "S" ou "N".
        /// </summary>
        /// <param name="value">O booleano.</param>
        /// <returns>
        /// <c>"S"</c> quando o valor for <c>true</c>;
        /// caso contrário <c>"N"</c>.
        /// </returns>
        public static string ToStringSN(this bool value)
        {
            return value ? "S" : "N";
        }

        /// <summary>
        /// Converte uma string para int.
        /// </summary>
        /// <param name="instance">String que será convertida.</param>
        /// <returns>O valor convertido ou default(int) caso contrário.</returns>
        public static int ToInt32(this string instance)
        {
            int output;

            if (int.TryParse(instance, out output))
            {
                return output;
            }

            return default(int);
        }
    }
}
