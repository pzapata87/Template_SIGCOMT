using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSSE.Domain;
using OSSE.Persistence;
using OSSE.Persistence.EntityFramework;

namespace OSSE.DataBase.Generator
{
    [TestClass]
    public class DataBaseGenerator
    {
        [TestMethod]
        public void CreateDataBaseDesarrollo()
        {
            Database.SetInitializer(new DbContextDropCreateDatabaseAlwaysDesarrollo());
            PersistenceConfigurator.Configure("OSSE", typeof (Usuario).Assembly, typeof (ConnectionFactory).Assembly);
            var target = new DbContextBase();
            target.Database.Initialize(true);
        }
    }
}