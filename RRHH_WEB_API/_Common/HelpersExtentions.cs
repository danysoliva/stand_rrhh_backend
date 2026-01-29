using System;

namespace RRHH_WEB_API._Common
{
    public static class HelpersExtentions
    {
        public static decimal GetYears(this DateTime fecha)
        {
            int YearDiff = 12 * DateTime.Now.Year - 12 * fecha.Year;
            int MonthDiff = DateTime.Now.Month - fecha.Month;
            decimal value = Math.Round(Convert.ToDecimal((YearDiff + MonthDiff) * 1.0 / 12), 2);

            return value;
        }

        public static bool IsNull(this object objeto)
        {
            bool result = (objeto == null);
            return result;
        }

        public static bool IsNotNull(this object objeto)
        {
            bool result = (!objeto.IsNull());
            return result;
        }

        public static bool EsEntero(this decimal numero)
        {
            bool esDecimal = (numero % 1) == 0;
            return esDecimal;
        }

        public static bool EsDecimal(this decimal numero)
        {
            bool esDecimal = (numero % 1) != 0;
            return esDecimal;
        }
    }
}
