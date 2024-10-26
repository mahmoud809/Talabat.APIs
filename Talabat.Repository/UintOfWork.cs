using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UintOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbContext;
        
        private Hashtable _repositories;

        public UintOfWork(StoreContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();
        }
        //This Function will help us to generate a centerin oject of TEntity when we need in any Ctor Like Ctor od order that needed just 3 objects.[GenericRepository<Product> ,GenericRepository<DeliveryMethod> ,GenericRepository<Order> ]
        //And it will store any object created for further usage. Instead of Creating a new of of the same object Created before.[In HashTable["Key(TEntity)" , "Value(Its instance)"] Pair]
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name; //For ex : Product

            if(!_repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<TEntity>(_dbContext);

                _repositories.Add(type, repository);
            }
            return _repositories[type] as IGenericRepository<TEntity>;
        }
        public async Task<int> Complete()
            => await _dbContext.SaveChangesAsync();
        

        public async ValueTask DisposeAsync()
            => await _dbContext.DisposeAsync();

    }
}
