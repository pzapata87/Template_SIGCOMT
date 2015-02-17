using System;

namespace SIGCOMT.Persistence.EntityFramework
{
    public interface IDatabaseFactory : IDisposable
    {
        DbContextBase Get();
    }
}