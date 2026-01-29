using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Common
{
    public class DiasSemana
    {
        public int Dia { get; set; }
        public string DiaNombre { get; set; }



        public string ObtenerDiaSemana(int dia)
        {
            string diaSemana = "";

            if (dia == 1)
            {
                diaSemana = "Lunes";
            }
            else
                if (dia == 2)
            {
                diaSemana = "Martes";
            }
            else
                     if (dia == 3)
            {
                diaSemana = "Miercoles";
            }
            else
                     if (dia == 4)
            {
                diaSemana = "Jueves";
            }
            else
                     if (dia == 5)
            {
                diaSemana = "Viernes";
            }
            else
                          if (dia == 6)
            {
                diaSemana = "Sábado";
            }
            else
                          if (dia == 7)
            {
                diaSemana = "Domingo";
            }
            

                return diaSemana;
        }
    }




}
