using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using OSSE.Aspects;
using OSSE.BusinessLogic.Interfaces;
using OSSE.Common;
using OSSE.Domain;
using OSSE.Persistence.Aspects;
using OSSE.Repository;

namespace OSSE.BusinessLogic
{
    [TryCatch(ExceptionTypeExpected = typeof(Exception), RethrowException = true)]
    public class ItemTablaBL : IItemTablaBL
    {
        private readonly IItemTablaRepository _itemTablaRepository;

        public ItemTablaBL(IItemTablaRepository itemTablaRepository)
        {
            _itemTablaRepository = itemTablaRepository;
        }
        
        public ItemTabla Get(Expression<Func<ItemTabla, bool>> where)
        {
            return _itemTablaRepository.FindOne(where);
        }
        
        public IList<ItemTabla> FindAll(Expression<Func<ItemTabla, bool>> where)
        {
            return _itemTablaRepository.FindAll(where).ToList();
        }
        
        public int Count(Expression<Func<ItemTabla, bool>> where)
        {
            return _itemTablaRepository.Count(where);
        }

        public int Count(Expression<Func<ItemTabla, bool>> where, Expression<Func<ItemTabla, object>> group)
        {
            throw new NotImplementedException();
        }
        
        public IQueryable<ItemTabla> GetAll(FilterParameters<ItemTabla> parameters)
        {
            return _itemTablaRepository.FindAllPaging(parameters);
        }
        
        [CommitsOperation]
        public void Add(ItemTabla entity)
        {
            _itemTablaRepository.Add(entity);
        }
        
        [CommitsOperation]
        public void Update(ItemTabla entity)
        {
            _itemTablaRepository.Update(entity);
        }
        
        [CommitsOperation]
        public void Update(IList<ItemTabla> entities)
        {
            foreach (var itemTabla in entities)
            {
                _itemTablaRepository.Update(itemTabla);
            }
        }
        
        public ItemTabla GetById(long id)
        {
            return _itemTablaRepository.FindOne(p => p.Id == id);
        }
    }
}
