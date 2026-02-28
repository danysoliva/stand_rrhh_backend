using System;
using System.ComponentModel.DataAnnotations;

namespace RRHH_WEB_API._Entidades
{
    public class EmployeePicture
    {
        [Key]
        public int Id { get; set; }
        public int IdEmployee { get; set; }
        public string EmployeeCode { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUid { get; set; }
        public bool Active { get; set; }
    }
}
