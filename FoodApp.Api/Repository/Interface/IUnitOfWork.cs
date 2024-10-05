using FoodApp.Api.Data.Entities;

namespace FoodApp.Api.Repository.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
        Task<int> SaveChangesAsync();

    }
}
