using System.Collections.Generic;

namespace RRHH_WEB_API._Entidades
{
    public class UserLevel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Enable { get; set; } = true;
        public List<UserDelegation> UserDelegations { get; set; }
    }

    public enum UserLevelEnum
    {
        Administrador = 1,
        Usuario = 2
    }
}
