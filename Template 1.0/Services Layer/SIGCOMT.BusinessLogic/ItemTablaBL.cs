using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SIGCOMT.Aspects;
using SIGCOMT.BusinessLogic.Interfaces;
using SIGCOMT.Common;
using SIGCOMT.Domain;
using SIGCOMT.Persistence.Aspects;
using SIGCOMT.Repository;

namespace SIGCOMT.BusinessLogic
{
    [TryCatch(ExceptionTypeExpected = typeof (Exception), RethrowException = true)]
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
            foreach (ItemTabla itemTabla in entities)
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