namespace RRHH_WEB_API.Features.Login.Dtos
{
    public class LoginDto
    {
        public int EmpleadoId { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public int UserLevelId { get; set; }
        public bool HasStaffInCharge { get; set; }
        public string Token { get; set; }
    }
}
