using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSSE.Repository;
using StructureMap;

namespace OSSE.RepositoryTest
{
    [TestClass]
    public class FormularioRepository
    {
        [TestMethod]
        public void TestMethod1()
        {
            #region Arrange

            var repository = ObjectFactory.GetInstance<IFormularioRepository>();

            #endregion

            #region Act

            #endregion

            #region Assert


            #endregion
        }
    }
}
