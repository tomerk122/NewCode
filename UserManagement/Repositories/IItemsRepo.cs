using UserManagement.Entites;

namespace UserManagement.Repositories
{
    public interface IItemsRepo
    {
        Task<Item> CreateAsync(Item item);
        Task<IReadOnlyCollection<Item>> GetAllAsync();
        Task<Item> GetAsync(Guid id);
        Task<Email> CrearteEmailAsync(Email email);
    }
}