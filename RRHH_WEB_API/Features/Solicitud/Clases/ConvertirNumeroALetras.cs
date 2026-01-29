using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.Solicitud.Clases
{
    public class ConvertirNumeroALetras
    {
        private static readonly string[] Unidades = { "", "un", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve" };
        private static readonly string[] Decenas = { "", "diez", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa" };
        private static readonly string[] Especiales = { "diez", "once", "doce", "trece", "catorce", "quince", "dieciséis", "diecisiete", "dieciocho", "diecinueve" };
        private static readonly string[] Centenas = { "", "ciento", "doscientos", "trescientos", "cuatrocientos", "quinientos", "seiscientos", "setecientos", "ochocientos", "novecientos" };

        public string NumeroALetras(int numero)
        {
            if (numero == 0)
            {
                return "cero";
            }

            if (numero < 0 || numero > 999_999_999)
            {
                throw new ArgumentException("El número debe estar entre 0 y 999,999,999");
            }

            string resultado = "";

            // Convertir millones
            int millones = numero / 1_000_000;
            if (millones > 0)
            {
                resultado += ConvertirGrupoALetras(millones) + " millones ";
                numero %= 1_000_000;
            }

            // Convertir miles
            int miles = numero / 1_000;
            if (miles > 0)
            {
                resultado += ConvertirGrupoALetras(miles) + " mil ";
                numero %= 1_000;
            }

            // Convertir unidades
            if (numero > 0)
            {
                resultado += ConvertirGrupoALetras(numero);
            }

            return resultado.Trim();
        }

        private static string ConvertirGrupoALetras(int grupo)
        {
            string resultado = "";

            // Convertir centenas
            int centenas = grupo / 100;
            if (centenas > 0)
            {
                resultado += Centenas[centenas] + " ";
                grupo %= 100;
            }

            // Convertir decenas y unidades
            if (grupo > 0)
            {
                if (grupo < 10)
                {
                    resultado += Unidades[grupo];
                }
                else if (grupo < 20)
                {
                    resultado += Especiales[grupo - 10];
                }
                else
                {
                    int decenas = grupo / 10;
                    int unidades = grupo % 10;

                    resultado += Decenas[decenas];
                    if (unidades > 0)
                    {
                        resultado += " y " + Unidades[unidades];
                    }
                }
            }

            return resultado;
        }
    }
}
