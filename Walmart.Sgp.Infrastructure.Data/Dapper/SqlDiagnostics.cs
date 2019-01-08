#if SQL_DIAGNOSTICS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Walmart.Sgp.Infrastructure.Data.Dapper
{
    /// <summary>
    /// Utilitário para facilitar o diagnóstico de lentidão em comandos SQL.
    /// <remarks>
    /// Se a diretiva de compilação SQL_DIAGNOSTICS estiver habilitada, serão registrados as informações abaixo na janela "Output/Debug" do Visual Studio:
    /// <example>
    /// --------------------------------------------------------------------------------
    /// SGP :: SQL DIAGNOSTICS
    /// --------------------------------------------------------------------------------
    /// DECLARE @userName VARCHAR(1000)
    /// SET @userName = 'lfteixe';
    /// SELECT Id, Username, Passwd, FullName, Email, IsApproved, LastActivityDate, LastLoginDate, CreationDate, IsLockedOut, LastLockoutDate, PasswdFormat FROM CWIUser WHERE UserName = @UserName
    /// Args: { userName = lfteixe }
    /// Records count: 1
    /// Elapsed milliseconds: 166
    /// ---------------------------------------------------------------------------------
    /// </example>
    /// </remarks>
    /// </summary>
    public static class SqlDiagnostics
    {
        /// <summary>
        /// Executa o comando informado na DapperQuery e log na janela de Output: o comando SQL, os argumentos para o comando, número de registros retornados e o tempo consumido na execução.
        /// </summary>
        /// <typeparam name="TResult">O tipo de retorno da consulta.</typeparam>
        /// <param name="query">O DapperQuery.</param>
        /// <param name="command">O comando a ser executado.</param>
        /// <returns>O resultado do comando.</returns>
        public static IEnumerable<TResult> ExecuteWithDiagnostics<TResult>(this IDapperQuery query, Func<IEnumerable<TResult>> command)
        {
            return ExecuteWithDiagnostics(
                query.Sql,
                query.Args,
                command,
                (result) =>
                {
                    Debug.WriteLine("Records count: {0}", result.Count());
                });
        }

        /// <summary>
        /// Executa o comando informado no DapperProxy e log na janela de Output: o comando SQL, os argumentos para o comando, o resultado. e o tempo consumido na execução.
        /// </summary>
        /// <typeparam name="TResult">O tipo de retorno da consulta.</typeparam>
        /// <param name="proxy">O DapperProxy.</param>
        /// <param name="sql">O comando SQL.</param>
        /// <param name="args">Os argumentos do comando SQL.</param>
        /// <param name="command">O comando a ser executado.</param>
        /// <returns>O resultado do comando.</returns>
        public static TResult ExecuteWithDiagnostics<TResult>(this DapperProxy proxy, string sql, object args, Func<TResult> command)
        {
            return ExecuteWithDiagnostics(sql, args, command, (result) => Debug.WriteLine("Result: {0}", result));
        }

        private static TResult ExecuteWithDiagnostics<TResult>(string sql, object args, Func<TResult> command, Action<TResult> writeResultInfo)
        {
            var separator = String.Empty.PadRight(80, '-');
            Debug.WriteLine(String.Empty);
            Debug.WriteLine(separator);
            Debug.WriteLine("SGP :: SQL DIAGNOSTICS");
            Debug.WriteLine(separator);
            Debug.WriteLine(CreateSqlParameters(args));
            Debug.WriteLine(sql);
            Debug.WriteLine("Args: {0}", args);
            Debug.Flush();

            var sw = new Stopwatch();
            sw.Start();
            var result = command();
            sw.Stop();

            writeResultInfo(result);

            Debug.WriteLine("Elapsed milliseconds: {0}", sw.ElapsedMilliseconds);
            Debug.WriteLine(separator);
            Debug.Flush();

            return result;
        }

        private static string CreateSqlParameters(object args)
        {
            if (args == null)
            {
                return String.Empty;
            }

            var objType = args.GetType();
            var properties = objType.GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            var declares = properties.Select(p => "@{0} {1}".With(p.Name, GetPropertyName(p)));
            var sets = properties.Select(p => "SET @{0} = {1};".With(p.Name, GetPropertyValue(args, p)));

            return "DECLARE {0}\n{1}".With(String.Join(", ", declares), String.Join("\n", sets));
        }

        private static string GetPropertyName(PropertyInfo p)
        {
            if (p.PropertyType == typeof(int) || p.PropertyType == typeof(int?) || p.PropertyType == typeof(byte) || p.PropertyType == typeof(byte?))
            {
                return "INT";
            }

            if (p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?))
            {
                return "DATE";
            }

            return "VARCHAR(1000)";
        }

        private static string GetPropertyValue(object args, PropertyInfo p)
        {
            string formattedValue;
            var value = p.GetValue(args);

            if (value == null)
            {
                formattedValue = "NULL";
            }
            else if (p.PropertyType == typeof(string))
            {
                formattedValue = "'{0}'".With(value);
            }
            else if (p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?))
            {
                formattedValue = "CONVERT(DATE, '{0:yyyy-MM-dd HH:mm:ss}', 120)".With(value);
            }
            else
            {
                formattedValue = value.ToString();
            }

            return formattedValue;
        }
    }
}
#endif