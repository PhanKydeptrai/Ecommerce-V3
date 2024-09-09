using AutoMapper;
using Data;
using Domain.DTO_Models;
using Domain.Models;
using Ecommerce_BE.IServices;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_BE.Services
{
    public class ComboServices: IComboServices
    {
        private readonly EcommerceDbContext _context;
        private readonly IMapper _mapper;
        private readonly IItemServices _itemServices;
        
        public ComboServices() { }
        public ComboServices(EcommerceDbContext context, IMapper mapper, IItemServices itemServices)
        {
            _context = context;
            _mapper = mapper;
            _itemServices = itemServices;
        }

        //Get all combo
        public async Task<IEnumerable<Combo>> GetCombos()
        {
            return await _context.Combos.ToListAsync();
        }

        //Get combo by id
        public async Task<Combo> GetComboById(Guid id)
        {

            return await _context.Combos.FirstOrDefaultAsync(a => a.ComboId == id);
        }

        //get combo by name 
        public async Task<Combo> GetComboByName(string name)
        {
            return await _context.Combos.FirstOrDefaultAsync(a => a.ComboName == name);
        }

        //Add new combo
        public async Task<bool> AddCombo(ComboCreationDto combo)
        {
            //Create new item
            var item = await _itemServices.CreateItem("combo");
            combo.ComboId = Guid.NewGuid();
            combo.ItemId = item.ItemId;

            var newCombo = _mapper.Map<Combo>(combo);
            await _context.Combos.AddAsync(newCombo);
            await _context.SaveChangesAsync();
            return true;
        }

        //Update combo
        public async Task<bool> UpdateCombo(ComboCreationDto combo)
        {
            await _context.Combos.AddAsync(_mapper.Map<Combo>(combo));
            int check = await _context.SaveChangesAsync();
            if(check > 0)
            {
                return true;
            }
            return false;
        }

        //Remove combo by id 
        public async Task<bool> RemoveCombo(Guid itemid)
        {
            var combo = await GetComboById(itemid);
            if (combo == null)
            {
                return false;
            }
            return await _itemServices.RemoveItemById(combo.ItemId);
        }
    }
}
