using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RRHH_WEB_API.Features.Upload.Dtos
{
    public class RepositoryGroupCRUDDto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }

        public int TipoCRUD { get; set; }

    }
}
