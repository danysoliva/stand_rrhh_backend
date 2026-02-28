using System.ComponentModel.DataAnnotations;

namespace RRHH_WEB_API._Entidades
{
    public class UserDelegation
    {
        [Key]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int UserLevelId { get; set; }
        public UserLevel UserLevel { get; set; }
        public bool Enable { get; set; } = true;


        public UserDelegation()
        {
            UserLevelId = 0;
        }
    }
}
