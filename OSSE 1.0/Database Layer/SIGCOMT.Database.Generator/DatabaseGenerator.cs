using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIGCOMT.Domain;
using SIGCOMT.Persistence;
using SIGCOMT.Persistence.EntityFramework;

namespace SIGCOMT.DataBase.Generator
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