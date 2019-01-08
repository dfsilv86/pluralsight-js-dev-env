namespace Walmart.Sgp.Infrastructure.Framework.Helpers
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Helper para exceções.
    /// </summary>
    public static class ExceptionHelper
    {
        /// <summary>
        /// Levanta <see cref="ArgumentNullException"/> para o parâmetro em argumentName, caso o valor em argumentValue seja null.
        /// </summary>
        /// <param name="argumentName">O nome do parâmetro.</param>
        /// <param name="argumentValue">O valor do parâmetro.</param>
        public static void ThrowIfNull(string argumentName, object argumentValue)
        {
            if (null == argumentValue)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        /// <summary>
        /// Lança uma exceção do tipo ArgumentNullException se qualquer propriedade do objeto for nulo.
        /// </summary>
        /// <remarks>
        /// O uso mais comum é passar um objeto anônimo com os argumentos do método, por exemplo:
        /// <example>
        /// public void Foo(string bar1, string bar2)
        /// {
        ///     Throw.AnyNull(new { bar1, bar2 });
        ///     ...
        /// }        
        /// </example>
        /// </remarks>
        /// <param name="target">O objeto alvo da verificação.</param>
        public static void ThrowIfAnyNull(object target)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            var properties = target.GetType().GetProperties();

            foreach (var p in properties)
            {
                if (p.GetValue(target, null) == null)
                {
                    throw new ArgumentNullException(p.Name);
                }
            }
        }
    }
}
