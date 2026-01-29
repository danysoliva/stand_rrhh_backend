using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.Maestros.Dtos
{
    public enum Deduccion
    {
        Cafeteria=4,
        OtrasDeducciones=5,
        SeguroMedico=6,
        Embargos=7,
        Bancos=8,
        OtrosIngresos=9,
        AhorroFijoCoop=10,
        PrestamoCoop=11,
        AportVariasCoop=12,
        AhorroRetCoop=13,
        PlanDental=15,
        PrestamoRAP=16,
        OtrasDeduccionesCooperativa=19,
        AFP=21,
        ISR=380,
        CuotaBomba=384,
        AdelantoDecimoCuartoMes=385,
        USULA=386
    }

    public class DeduccionDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Monto { get; set; }

    }
}
