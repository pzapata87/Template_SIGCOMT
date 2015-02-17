using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SIGCOMT.BusinessLogic.Core;
using SIGCOMT.Domain;

namespace SIGCOMT.BusinessLogic.Interfaces
{
    public interface IFormularioBL : IPaging<Formulario>
    {
        Formulario GetById(int id);
        IQueryable<Formulario> FindAll(Expression<Func<Formulario, bool>> where);
        List<Formulario> Formularios(Usuario usuario);
    }
}