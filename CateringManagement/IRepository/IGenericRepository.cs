namespace CateringManagement.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByID(int id);
        Task<int> Create(T item);
        Task<int> Update(T item);
        Task<int> Delete(int id);
    }
}
