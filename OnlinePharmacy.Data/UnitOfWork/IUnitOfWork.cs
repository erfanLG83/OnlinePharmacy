using OnlinePharmacy.Data.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePharmacy.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        public OnlinePharmacyDbContext Context { get;}
        public IRepositoryBase<TEntity> GetDynamicRepository<TEntity>() where TEntity:class;
        public Task SaveChangesAsync();
    }
}
