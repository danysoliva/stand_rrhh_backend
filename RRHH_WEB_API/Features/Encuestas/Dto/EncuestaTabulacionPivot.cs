using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.Encuestas.Dto
{
    public class EncuestaTabulacionPivot
    {
        public List<DataFieldPivot> DataFieldPivots { get; set; }
        public StorePivot Pivote { get; set; }


        public class DataFieldPivot
        {
            public string DataField { get; set; }
            public string Area { get; set; }
        }

        public class StorePivot
        {
            public string Type { get; set; }
            public string URL { get; set; }
            public string Catalog { get; set; }
            public string Cube { get; set; }
        }
    }
}
