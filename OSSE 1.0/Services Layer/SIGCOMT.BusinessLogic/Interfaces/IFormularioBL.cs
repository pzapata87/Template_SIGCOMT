using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SIGCOMT.BusinessLogic.Core;
using SIGCOMT.Domain;

namespace SIGCOMT.BusinessLogic.Interfaces
{
    public interface IFormularioBL : IPaging<Formulario>
    {
        Formulario GetById(int id);
        IList<Formulario> FindAll(Expression<Func<Formulario, bool>> where);
        List<Formulario> Formularios(Usuario usuario);
    }
}