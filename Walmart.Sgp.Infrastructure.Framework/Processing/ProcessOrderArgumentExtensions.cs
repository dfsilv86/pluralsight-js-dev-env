using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.FileVault;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Infrastructure.Framework.Processing
{
    /// <summary>
    /// Extensões relacionadas ao gerenciamento, serialização e deserialização de listas de parâmetros de processamentos.
    /// </summary>
    /// <remarks>
    /// <para>Regras:</para>
    /// <para>1. Tipos primitivos, nuláveis de primitivos, e comuns (DateTime, TimeSpan, FixedValues, FileVaultTicket) são válidos;</para>
    /// <para>2. Listas e coleções comuns (Dictionary, List, ArrayList,...) são válidos; tentamos obedecer a ordem dentro das listas, mas isso não é garantido.</para>
    /// <para>2.1. Dicionários ou enumeráveis onde o tipo do elemento é Object serão deserializados ou como strings ou como Dictionary de chave e elemento strings (já que perdemos a informação de tipo).</para>
    /// <para>3. Objetos marcados com atributo Serializable são válidos, porém apenas propriedades leitura+escrita (sem atributo NonSerialize) são serializados.</para>
    /// <para>4. Demais casos são inválidos (podem causar erro imediatamente ou não)</para>
    /// </remarks>
    public static class ProcessOrderArgumentExtensions
    {
        /// <summary>
        /// Cria uma lista de argumentos (parâmetros) para um processo.
        /// </summary>
        /// <param name="parameters">Os parâmetros.</param>
        /// <param name="exposedParameters">Os parâmetros expostos.</param>
        /// <param name="moveToInputFile">O delegate que move um arquivo de entrada armazenado no FileVault para o diretório de arquivos de entrada do processamento.</param>
        /// <returns>A lista de argumentos.</returns>
        public static IList<ProcessOrderArgument> ToArgumentList(this IReadOnlyDictionary<string, object> parameters, IEnumerable<string> exposedParameters, Func<FileVaultTicket, string> moveToInputFile)
        {
            List<ProcessOrderArgument> result = new List<ProcessOrderArgument>();

            Stack<Tuple<object, string, bool>> work = new Stack<Tuple<object, string, bool>>();

            foreach (var kvp in parameters)
            {
                work.Push(new Tuple<object, string, bool>(kvp.Value, kvp.Key, exposedParameters.Any(x => x == kvp.Key)));
            }

            while (work.Count > 0)
            {
                var item = work.Pop();
                if (null == item.Item1)
                {
                    result.Add(new ProcessOrderArgument() { Name = item.Item2, Value = null, IsExposed = item.Item3 });
                    continue;
                }

                var valueType = item.Item1.GetType();

                if (valueType.IsPrimitive || valueType == typeof(string))
                {
                    result.Add(new ProcessOrderArgument() { Name = item.Item2, Value = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", item.Item1), IsExposed = item.Item3 });
                }
                else if (valueType == typeof(DateTime) || valueType == typeof(TimeSpan))
                {
                    string serializedValue = null;

                    if (null != item.Item1)
                    {
                        serializedValue = ((IFormattable)item.Item1).ToString(valueType == typeof(DateTime) ? "yyyy-MM-dd HH:mm:ss.fff" : "c", System.Globalization.CultureInfo.InvariantCulture);
                    }

                    result.Add(new ProcessOrderArgument() { Name = item.Item2, Value = serializedValue ?? string.Empty, IsExposed = item.Item3 });
                }
                else if (valueType == typeof(FileVaultTicket))
                {
                    result.Add(new ProcessOrderArgument() { Name = item.Item2, Value = moveToInputFile((FileVaultTicket)item.Item1), IsExposed = item.Item3 });
                }
                else if (typeof(System.Collections.IDictionary).IsAssignableFrom(valueType))
                {
                    System.Collections.IDictionary dict = (System.Collections.IDictionary)item.Item1;

                    foreach (System.Collections.DictionaryEntry kvp in dict)
                    {
                        work.Push(new Tuple<object, string, bool>(dict[kvp.Key], item.Item2 + "[" + kvp.Key.ToString() + "]", item.Item3));
                    }
                }
                else if (typeof(System.Collections.IList).IsAssignableFrom(valueType))
                {
                    System.Collections.IList list = (System.Collections.IList)item.Item1;

                    for (var index = 0; index < list.Count; index++)
                    {
                        work.Push(new Tuple<object, string, bool>(list[index], item.Item2 + "[{0}]".With(index), item.Item3));
                    }
                }
                else if (typeof(System.Collections.IEnumerable).IsAssignableFrom(valueType))
                {
                    System.Collections.IEnumerable list = (System.Collections.IEnumerable)item.Item1;

                    long index = 0;
                    foreach (var listItem in list)
                    {
                        work.Push(new Tuple<object, string, bool>(listItem, item.Item2 + "[{0}]".With(index), item.Item3));
                        index++;
                    }
                }
                else if (typeof(IFixedValue).IsAssignableFrom(valueType))
                {
                    work.Push(new Tuple<object, string, bool>(((IFixedValue)item.Item1).ValueAsObject, item.Item2, item.Item3));
                }
                else if (valueType.GetCustomAttributes(typeof(SerializableAttribute), true).Count() > 0)
                {
                    var validProperties = from props in valueType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                                          where props.CanRead && props.CanWrite && props.GetCustomAttributes(typeof(NonSerializedAttribute), true).Count() == 0
                                          let exposedParam = (ExposedParameterAttribute)props.GetCustomAttributes(typeof(ExposedParameterAttribute), true).SingleOrDefault() ?? ExposedParameterAttribute.Default
                                          select new Tuple<object, string, bool>(props.GetValue(item.Item1), item.Item2 + "." + props.Name, exposedParam.IsExposed);

                    validProperties.ToList().ForEach(y => work.Push(y));
                }
                else
                {
                    throw new NotSupportedException(Texts.ProcessArgumentCannotConvertType.With(valueType.FullName));
                }
            }

            return result;
        }

        /// <summary>
        /// Cria uma lista de parâmetros a partir da lista de argumentos de um processo.
        /// </summary>
        /// <param name="arguments">A lista de argumentos.</param>
        /// <param name="parameterTypes">Os tipos dos valores dos parâmetros.</param>
        /// <param name="saveInputFile">Delegate que salva o arquivo permanentemente como arquivo de entrada.</param>
        /// <returns>Os parâmetros.</returns>
        public static IReadOnlyDictionary<string, object> ToServiceParameters(this IList<ProcessOrderArgument> arguments, IReadOnlyDictionary<string, Type> parameterTypes, Func<string, FileVaultTicket> saveInputFile)
        {
            // cria placeholders para os argumentos que correspondem diretamente aos parametros do metodo
            var rootArguments = parameterTypes.Select(kvp => new SerializedArgumentInformation
                {
                    Name = kvp.Key,
                    StandardizedName = kvp.Key,
                    Tokens = new string[] { kvp.Key },
                    Level = 1,
                    ParameterType = kvp.Value,
                    IsSupportedDeserializableType = IsSupportedDeserializableType(kvp.Value)
                }).ToList();  // cria lista de parametros do metodo

            // Para cada argumento na lista de valores serializados, cria um info que serve como item de processamento para deserializar o valor
            var argumentDetails = (from arg in arguments ?? new List<ProcessOrderArgument>()
                                   let stdName = arg.Name.Replace("[", ".")
                                   let tokens = stdName.Split('.')
                                   let rank = tokens.Length
                                   orderby stdName
                                   let rootArgument = rootArguments.SingleOrDefault(rootArgument => rank == 1 && rootArgument.Name == arg.Name)  // se este argumento se refere diretamente a um parametro do metodo, pega o parametro
                                   let rootArgumentValueAssignment = null != rootArgument ? rootArgument.SerializedValue = arg.Value : null      // se encontrou o parametro, atribui o valor serializado
                                   select rootArgument ?? new   // se nao encontrou o parametro (ou não é diretamente um parâmetro), cria um novo info de argumento
                                       SerializedArgumentInformation
                                       {
                                           Name = arg.Name,
                                           SerializedValue = arg.Value,
                                           StandardizedName = stdName,
                                           Tokens = tokens,
                                           Level = rank,
                                           ParameterType = (rank == 1) ? parameterTypes[tokens[0]] : null,
                                           IsSupportedDeserializableType = rank == 1 ? IsSupportedDeserializableType(parameterTypes[tokens[0]]) : (bool?)null,
                                           ParameterValue = null
                                       }) // esta lista contem rootArguments existentes de tipo simples com valor serializado atribuido OU novos SerializedArgumentInformation de argumentos de tipos complexos (valores de props de classes, itens de arrays, etc)
                                    .Union(rootArguments) // este union adiciona na lista os rootArguments de tipo complexo que sobraram e ainda nao possuem valor serializado
                                    .ToList();

            // para argumentos que nao sao filhos diretos dos parametros, prepara um placeholder que corresponde a deserializacao do objeto ao qual o argumento pertence
            argumentDetails.Where(a => a.Level > 2).ToList().ForEach(argument => PrepareNonRootParentArguments(argumentDetails, argument));

            // Determina cada nodo da árvore e seus filhos imediatos.
            var argumentsAndChildren = argumentDetails.ToDictionary(
                k => k,
                arg => argumentDetails.Where(childArg => childArg.Level == arg.Level + 1 && childArg.StandardizedName.StartsWith(arg.StandardizedName + ".", StringComparison.OrdinalIgnoreCase)).ToDictionary(k => k.Name, v => v));

            // Etapa 1: propaga tipos através da estrutura a partir dos tipos dos parâmetros do método.
            argumentsAndChildren.OrderBy(k => k.Key.Level).ToList().ForEach(PropagateParameterTypeInformation);

            // Etapa 2: deserializa valores e monta o grafo de objetos
            argumentsAndChildren.OrderByDescending(k => k.Key.Level).ToList().ForEach((item) => MaterializeNestedParameterValues(item, saveInputFile));

            // Neste momento, todos os argumentos de nível 1 são os parâmetros do método de serviço, e devem estar devidamente deserializados.
            return argumentsAndChildren.Where(a => a.Key.Level == 1).ToDictionary(k => k.Key.Name, v => v.Key.ParameterValue);
        }

        /// <summary>
        /// Deserializa um valor.
        /// </summary>
        /// <param name="theValue">O valor serializado.</param>
        /// <param name="parameterType">O tipo do parâmetro.</param>
        /// <param name="saveInputFile">Delegate que salva o arquivo permanentemente como arquivo de entrada.</param>
        /// <returns>O valor deserializado.</returns>
        public static object DeserializeValue(string theValue, Type parameterType, Func<string, FileVaultTicket> saveInputFile)
        {
            if (null == theValue || parameterType == typeof(string) || parameterType == typeof(object))
            {
                return theValue;
            }

            if (typeof(IFixedValue).IsAssignableFrom(parameterType))
            {
                Type fixedValueType = parameterType;

                while (fixedValueType != fixedValueType.BaseType && fixedValueType.BaseType != null && !fixedValueType.FullName.StartsWith(typeof(FixedValuesBase<>).FullName, StringComparison.OrdinalIgnoreCase))
                {
                    fixedValueType = fixedValueType.BaseType;
                }

                Type underlyingType = fixedValueType.GenericTypeArguments[0];
                object underlyingValue = DeserializeSimpleValue(theValue, underlyingType, saveInputFile);

                var conversionOperator = FixedValuesHelper.GetConversionOperator(parameterType);

                return conversionOperator.Invoke(null, new object[] { underlyingValue });
            }
            else if (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                Type nullableTyle = typeof(Nullable<>).MakeGenericType(parameterType.GenericTypeArguments);
                var ctor = nullableTyle.GetConstructor(parameterType.GenericTypeArguments);

                Type underlyingType = nullableTyle.GenericTypeArguments[0];
                object underlyingValue = DeserializeSimpleValue(theValue, underlyingType, saveInputFile);

                return null != theValue ? ctor.Invoke(new object[] { underlyingValue }) : null;
            }
            else
            {
                return DeserializeSimpleValue(theValue, parameterType, saveInputFile);
            }
        }

        /// <summary>
        /// Obtém a lista de nomes de parâmetros.
        /// </summary>
        /// <param name="arguments">Os argumentos.</param>
        /// <returns>Os nomes de parâmetros.</returns>
        public static IEnumerable<string> GetParameterNames(this IList<ProcessOrderArgument> arguments)
        {
            if (null == arguments)
            {
                return new string[0];
            }

            return (from arg in arguments
                    let stdName = arg.Name.Replace("[", ".")
                    let tokens = stdName.Split('.')
                    select tokens[0]).Distinct().ToArray();
        }

        /// <summary>
        /// Obtém a lista de nomes de parâmetros.
        /// </summary>
        /// <param name="parameters">Os parâmetros.</param>
        /// <returns>Os nomes de parâmetros.</returns>
        public static IEnumerable<string> GetParameterNames(this IReadOnlyDictionary<string, object> parameters)
        {
            return (from kvp in parameters group kvp by kvp.Key.Split('.')[0] into firstNames select firstNames.Key).ToArray();
        }

        private static object DeserializeSimpleValue(string theValue, Type parameterType, Func<string, FileVaultTicket> saveInputFile)
        {
            if (parameterType == typeof(TimeSpan))
            {
                return TimeSpan.Parse((string)theValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (parameterType == typeof(DateTime))
            {
                return DateTime.Parse((string)theValue, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (parameterType == typeof(FileVaultTicket))
            {
                return saveInputFile((string)theValue);
            }
            else
            {
                return null != theValue ? Convert.ChangeType(theValue, parameterType, System.Globalization.CultureInfo.InvariantCulture) : null;
            }
        }

        private static void PrepareNonRootParentArguments(List<SerializedArgumentInformation> argumentDetails, SerializedArgumentInformation argument)
        {
            if (!argumentDetails.Any(aa => aa.Level == argument.Level - 1 && argument.StandardizedName.StartsWith(aa.StandardizedName + ".", StringComparison.OrdinalIgnoreCase)))
            {
                string parentName = argument.Tokens[argument.Tokens.Count - 2];
                string[] parentTokens = argument.Tokens.Take(argument.Tokens.Count - 1).ToArray();
                string parentStdName = string.Join(".", parentTokens);

                argumentDetails.Add(new SerializedArgumentInformation
                {
                    Name = parentName,
                    StandardizedName = parentStdName,
                    Tokens = parentTokens,
                    Level = argument.Level - 1
                });
            }
        }

        private static void PropagateParameterTypeInformation(KeyValuePair<SerializedArgumentInformation, Dictionary<string, SerializedArgumentInformation>> arg)
        {
            Type parameterType = arg.Key.ParameterType;

            if (arg.Key.IsSupportedDeserializableType.HasValue && arg.Key.IsSupportedDeserializableType.Value)
            {
                // Neste caso não precisa fazer nada; a conversão será realizada na segunda etapa.
            }
            else if (IsSupportedCollectionType(arg.Key.ParameterType) || (arg.Key.ParameterType == typeof(object) && arg.Value.Count > 0 && null == arg.Key.SerializedValue))
            {
                if (arg.Key.ParameterType == typeof(object))
                {
                    arg.Key.ParameterType = typeof(Dictionary<string, object>);
                }

                Type elementType = GetElementType(arg.Key.ParameterType);

                foreach (var kvp in arg.Value)
                {
                    kvp.Value.ParameterType = elementType;
                    kvp.Value.IsSupportedDeserializableType = IsSupportedDeserializableType(kvp.Value.ParameterType);
                }
            }
            else
            {
                foreach (var kvp in arg.Value)
                {
                    string propertyName = kvp.Value.Tokens.Last();
                    kvp.Value.ParentPropertyInfo = parameterType.GetProperty(propertyName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    kvp.Value.ParameterType = kvp.Value.ParentPropertyInfo.PropertyType;
                    kvp.Value.IsSupportedDeserializableType = IsSupportedDeserializableType(kvp.Value.ParameterType);
                }
            }
        }

        private static void MaterializeNestedParameterValues(KeyValuePair<SerializedArgumentInformation, Dictionary<string, SerializedArgumentInformation>> arg, Func<string, FileVaultTicket> saveInputFile)
        {
            Type parameterType = arg.Key.ParameterType;

            //// Aparentemente não acontece
            ////if (!arg.Key.IsSupportedDeserializableType.HasValue)
            ////{
            ////    // Se aqui ainda não sabemos se pode ser deserializado diretamente ou não, então algum objeto no caminho não é suportado.
            ////    // O tipo de objeto para o argumento "{0}" não é suportado.
            ////    throw new NotSupportedException(Texts.UnknownSerializedObject.With(arg.Key.Name));
            ////}
            ////else 
            if (arg.Key.IsSupportedDeserializableType.Value || (arg.Key.ParameterType == typeof(object) && arg.Value.Count() == 0))
            {
                arg.Key.ParameterValue = DeserializeValue(arg.Key.SerializedValue, parameterType, saveInputFile);
            }
            else if (IsSupportedCollectionType(arg.Key.ParameterType))
            {
                var theCollection = DeserializeCollection(arg);

                arg.Key.ParameterValue = theCollection.Value;

                if (null != theCollection.KeyType)
                {
                    var dictionary = (System.Collections.IDictionary)theCollection.Value;

                    foreach (var kvp in arg.Value)
                    {
                        var elementKey = kvp.Value.Tokens.Last();

                        if (!elementKey.EndsWith("]", StringComparison.OrdinalIgnoreCase))
                        {
                            // O valor sendo deserializado não corresponde a um item de um dicionário.
                            throw new InvalidOperationException(Texts.ValueBeingDeserializedIsNotDictionaryValue);
                        }

                        var key = DeserializeValue(elementKey.Substring(0, elementKey.Length - 1), theCollection.KeyType, saveInputFile);

                        dictionary[key] = theCollection.ElementType != typeof(object) && IsSupportedDeserializableType(theCollection.ElementType) ? Convert.ChangeType(kvp.Value.ParameterValue, theCollection.ElementType, System.Globalization.CultureInfo.InvariantCulture) : kvp.Value.ParameterValue;
                    }
                }
                else
                {
                    var list = (System.Collections.IList)theCollection.Value;

                    foreach (var kvp in arg.Value.OrderBy(n => n.Key))
                    {
                        list.Add(Convert.ChangeType(kvp.Value.ParameterValue, theCollection.ElementType, System.Globalization.CultureInfo.InvariantCulture));
                    }
                }
            }
            else
            {
                object value = parameterType.GetConstructor(Type.EmptyTypes).Invoke(null);

                foreach (var kvp in arg.Value)
                {
                    kvp.Value.ParentPropertyInfo.SetValue(value, kvp.Value.ParameterValue);
                }

                arg.Key.ParameterValue = value;
            }
        }

        private static DeserializedCollectionInfo DeserializeCollection(KeyValuePair<SerializedArgumentInformation, Dictionary<string, SerializedArgumentInformation>> arg)
        {
            object value = null;
            Type keyType = GetKeyType(arg.Key.ParameterType);
            Type elementType = GetElementType(arg.Key.ParameterType);

            if (arg.Key.ParameterType.IsInterface)
            {
                if (null != keyType)
                {
                    value = typeof(Dictionary<,>).MakeGenericType(new Type[] { keyType, elementType }).GetConstructor(Type.EmptyTypes).Invoke(null);
                }
                else
                {
                    value = typeof(List<>).MakeGenericType(new Type[] { elementType }).GetConstructor(Type.EmptyTypes).Invoke(null);
                }
            }
            else
            {
                value = arg.Key.ParameterType.GetConstructor(Type.EmptyTypes).Invoke(null);
            }

            return new DeserializedCollectionInfo { ElementType = elementType, KeyType = keyType, Value = value };
        }

        private static bool IsSupportedCollectionType(Type type)
        {
            return typeof(System.Collections.IDictionary).IsAssignableFrom(type) ||
                typeof(System.Collections.ICollection).IsAssignableFrom(type) ||
                typeof(System.Collections.IEnumerable).IsAssignableFrom(type);
        }

        private static Type GetElementType(Type targetType)
        {
            Type[] genericTypes;

            genericTypes = ((targetType.Name == "IDictionary`2" ? targetType : null) ?? (targetType.Name == "IEnumerable`1" ? targetType : null) ?? targetType.GetInterface("IDictionary`2") ?? targetType.GetInterface("IEnumerable`1") ?? typeof(object)).GetGenericArguments();

            return genericTypes.LastOrDefault() ?? typeof(object);
        }

        private static Type GetKeyType(Type type)
        {
            if (type.GetGenericArguments().Count() == 2 && (typeof(IDictionary<,>).IsAssignableFrom(type.GetGenericTypeDefinition()) || type.GetGenericTypeDefinition().GetInterfaces().Any(x => x.Name == "IDictionary`2")))
            {
                return type.GetGenericArguments().FirstOrDefault() ?? typeof(object);
            }

            if (typeof(System.Collections.IDictionary).IsAssignableFrom(type))
            {
                return typeof(object);
            }

            return null;
        }

        private static bool IsSupportedDeserializableType(Type pType)
        {
            return (pType.IsGenericType && pType.GetGenericTypeDefinition() == typeof(Nullable<>) && pType.GenericTypeArguments[0].IsPrimitive) || pType.IsPrimitive || pType == typeof(string) || pType == typeof(DateTime) || pType == typeof(TimeSpan) || pType == typeof(FileVaultTicket) || typeof(IFixedValue).IsAssignableFrom(pType);
        }
    }
}
