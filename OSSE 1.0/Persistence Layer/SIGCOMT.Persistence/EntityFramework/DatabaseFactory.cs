namespace SIGCOMT.Persistence.EntityFramework
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private DbContextBase _dataContext;

        public DbContextBase Get()
        {
            return _dataContext ?? (_dataContext = new DbContextBase());
        }

        protected override void DisposeCore()
        {
            if (_dataContext != null)
                _dataContext.Dispose();
        }
    }
}