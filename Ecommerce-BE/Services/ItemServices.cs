using Data;
using Domain.Models;
using Ecommerce_BE.IServices;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_BE.Services
{
    public class ItemServices : IItemServices
    {
        public ItemServices() { }
        private readonly EcommerceDbContext _context;
        public ItemServices(EcommerceDbContext context)
        {
            _context = context;
        }

        //Create new item
        public async Task<Item> CreateItem(string type)
        {
            Item item = new Item
            {
                ItemId = Guid.NewGuid(),
                Status = "hd",
                ItemType = type
            };
            
            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        //Get all items
        public async Task<IEnumerable<Item>> GetItems()
        {
            return await _context.Items.ToListAsync();
        }

        //Update status of item
        public async Task<bool> UpdateItemStatus(Item item)
        {
            _context.Items.Update(item);
            int check = await _context.SaveChangesAsync();
            if (check > 0)
            {
                return true;
            }
            return false;
        }

        //remove item by id
        public async Task<bool> RemoveItemById(Guid id)
        {
            Item item = _context.Items.FirstOrDefault(a => a.ItemId == id);
            item.Status = "nhd";
            _context.Items.Update(item);
            int check = await _context.SaveChangesAsync();
            if (check > 0)
            {
                return true;
            }
            return false;
        }

        //Get item by id
        public async Task<Item> GetItemById(Guid id)
        {
            return await _context.Items.FirstOrDefaultAsync(a => a.ItemId == id);
        }
    }
}
