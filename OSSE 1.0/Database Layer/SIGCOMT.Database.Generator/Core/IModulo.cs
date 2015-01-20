using System.Data.Entity;

namespace SIGCOMT.DataBase.Generator.Core
{
    public interface IModulo
    {
        void Registrar(DbContext context);
    }
}