using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API._Entidades
{
    public class BenefitDeduction
    {
        public int Id { get; set; }
        public int? ContractId { get; set; }
        public Contract Contract { get; set; }
        public int? ConceptId { get; set; }
        public Concept Concept { get; set; }
        public decimal? Value { get; set; }
        public bool? Active { get; set; }
        public string  Type { get; set; }
    }
}
