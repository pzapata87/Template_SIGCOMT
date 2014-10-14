using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using OSSE.BusinessLogic.Core;
using OSSE.Domain;

namespace OSSE.BusinessLogic.Interfaces
{
    public interface IItemTablaBL : IPaging<ItemTabla>
    {
        ItemTabla Get(Expression<Func<ItemTabla, bool>> where);
        IList<ItemTabla> FindAll(Expression<Func<ItemTabla, bool>> where);
        void Add(ItemTabla entity);
        void Update(ItemTabla entity);
        void Update(IList<ItemTabla> entities);
        ItemTabla GetById(long id);
    }
}