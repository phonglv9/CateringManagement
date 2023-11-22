using CateringManagement.IRepository;
using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace CateringManagement.Repository.Genneric
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected ApplicationDbContext db { get; set; }
        protected DbSet<T> table = null;

        public GenericRepository()
        {
            db = new ApplicationDbContext();
            table = db.Set<T>();
        }

        public GenericRepository(ApplicationDbContext db)
        {
            this.db = db;
            table = db.Set<T>();
        }


        public async Task<List<T>> GetAllList()
        {
            return await table.ToListAsync();
        }

        public async Task<T> GetByID(Guid id)
        {
            return await table.FindAsync(id);
        }

        public async Task<int> Create(T item)
        {
             table.Add(item);
            return await db.SaveChangesAsync();
        }

        public async Task<int> Update(T item)
        {
            table.Attach(item);
            db.Entry(item).State = EntityState.Modified;
            return await db.SaveChangesAsync();
        }

        public async Task<int> Delete(Guid id)
        {
            table.Remove(await table.FindAsync(id));
            return await db.SaveChangesAsync();
        }
    }
}
