using System.Collections.Generic;
using SIGCOMT.Common.Enum;
using SIGCOMT.Domain;

namespace SIGCOMT.DataBase.Generator
{
    public static class RolesStorage
    {
        static RolesStorage()
        {
            Roles = new Dictionary<TipoRol, Rol>();
        }

        public static Dictionary<TipoRol, Rol> Roles { get; set; }         
    }
}