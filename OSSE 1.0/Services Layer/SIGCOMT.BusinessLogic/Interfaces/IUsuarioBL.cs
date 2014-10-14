using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SIGCOMT.BusinessLogic.Core;
using SIGCOMT.Domain;

namespace SIGCOMT.BusinessLogic.Interfaces
{
    public interface IUsuarioBL : IPaging<Usuario>
    {
        Usuario Get(Expression<Func<Usuario, bool>> where);
        void Add(Usuario entity);
        void Add(Usuario entity, IList<int> listaRolSelected);
        void Update(Usuario entity);
        Usuario GetById(int id);
        void Update(Usuario entity, IList<int> listaRoleSelected);
        IQueryable<Usuario> FindAll(Expression<Func<Usuario, bool>> where);
        Usuario ValidateUser(string username, string password);
    }
}