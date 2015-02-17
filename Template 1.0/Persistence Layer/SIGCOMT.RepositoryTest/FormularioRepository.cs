using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIGCOMT.Repository;
using StructureMap;

namespace SIGCOMT.RepositoryTest
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