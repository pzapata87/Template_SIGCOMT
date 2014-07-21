using System;

namespace OSSE.Persistence.EntityFramework
{
    public interface IDatabaseFactory : IDisposable
    {
        DbContextBase Get();
    }
}