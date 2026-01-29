using System;

namespace RRHH_WEB_API.Features.Maestros.Dtos
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Pin { get; set; }
        public string BarCode { get; set; }
        public DateTime BirthDay { get; set; }
    }
}
