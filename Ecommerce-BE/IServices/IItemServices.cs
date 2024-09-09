using Domain.Models;

namespace Ecommerce_BE.IServices
{
    public interface IItemServices
    {
        Task<Item> CreateItem(string type);
        Task<IEnumerable<Item>> GetItems();
        Task<bool> UpdateItemStatus(Item item);
        Task<bool> RemoveItemById(Guid id);
        Task<Item> GetItemById(Guid id);
    }
}
