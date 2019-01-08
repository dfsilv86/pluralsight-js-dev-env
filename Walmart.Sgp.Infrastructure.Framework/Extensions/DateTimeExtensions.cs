using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Runtime;

namespace System
{
    /// <summary>
    /// Extension methods para date time.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Retorna a hora e minuto como número inteiro no formato HHmm.
        /// </summary>
        /// <param name="dateTime">A data com horário.</param>
        /// <returns>O inteiro com o horário.</returns>
        public static int ToMilitaryTime(this DateTime dateTime)
        {
            return dateTime.Minute + (dateTime.Hour * 100);
        }

        /// <summary>
        /// Retorna a data/hora do último instante do dia.
        /// </summary>
        /// <param name="dateTime">A data.</param>
        /// <returns>A data/hora do último instante do dia.</returns>
        public static DateTime ToLastDayTime(this DateTime dateTime)
        {
            return dateTime.Date.AddDays(1).AddTicks(-1);
        }

        /// <summary>
        /// Retorna a data/hora do último instante do um mês.
        /// </summary>
        /// <param name="dateTime">A data.</param>
        /// <returns>A data/hora do último instante do mês.</returns>
        public static DateTime ToLastMonthTime(this DateTime dateTime)
        {
            var lastMonthDay = new DateTime(
                dateTime.Year,
                dateTime.Month,
                DateTime.DaysInMonth(dateTime.Year, dateTime.Month));

            return lastMonthDay.ToLastDayTime();
        }

        /// <summary>
        /// Retorna um DateTime baseado em uma string no formato "yyyy-MM-dd".
        /// </summary>
        /// <param name="dateStr">Uma string no formato yyyy-MM-dd.</param>
        /// <returns>Um objeto DateTime.</returns>
        public static DateTime ToDate(this string dateStr)
        {
            try
            {
                return new DateTime(int.Parse(dateStr.Substring(0, 4), RuntimeContext.Current.Culture), int.Parse(dateStr.Substring(5, 2), RuntimeContext.Current.Culture), int.Parse(dateStr.Substring(8, 2), RuntimeContext.Current.Culture));
            }
            catch
            {
                throw new FormatException(Texts.invalidDateFormat);
            }
        }
    }
}
