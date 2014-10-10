using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using OSSE.BusinessLogic.Core;
using OSSE.Domain;

namespace OSSE.BusinessLogic.Interfaces
{
    public interface IFormularioBL : IPaging<Formulario>
    {
        Formulario GetById(int id);
        IList<Formulario> FindAll(Expression<Func<Formulario, bool>> where);
        List<Formulario> Formularios(Usuario usuario);
    }
}
