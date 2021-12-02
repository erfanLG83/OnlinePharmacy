using OnlinePharmacy.Data.Repositories.Contract;
using OnlinePharmacy.Data.Repositories.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePharmacy.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(OnlinePharmacyDbContext context)
        {
            Context = context;
        }

        public OnlinePharmacyDbContext Context { get; }

        public IRepositoryBase<TEntity> GetDynamicRepository<TEntity>() where TEntity:class
        {
            return new RepositoryBase<TEntity>(Context);
        }

        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
