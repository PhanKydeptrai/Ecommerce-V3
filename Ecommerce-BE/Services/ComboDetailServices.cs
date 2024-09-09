using AutoMapper;
using Data;
using Domain.DTO_Models;
using Domain.Models;
using Ecommerce_BE.IServices;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_BE.Services
{
    public class ComboDetailServices : IComboDetailServices
    {
        private readonly EcommerceDbContext _context;
        private readonly IMapper _mapper;
        public ComboDetailServices() { }
        public ComboDetailServices(EcommerceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<ComboDetail>> GetComboDetails()
        {
            return await _context.ComboDetails.ToListAsync();
        }

        public async Task<IEnumerable<ComboDetail>> GetComboDetailByComboId(Guid id)
        {
            IEnumerable<ComboDetail> a =  await _context.ComboDetails.Where(a => a.ComboId == id).ToListAsync();

            return a;
        }

        public async Task<ComboDetail> GetComboDetailById(Guid id)
        {
            return await _context.ComboDetails.FirstOrDefaultAsync(a => a.ComboDetailId == id);
        }

        public async Task<bool> AddComboDetail(ComboDetailCreationDto comboDetailCreation)
        {
            await _context.ComboDetails.AddAsync(_mapper.Map<ComboDetail>(comboDetailCreation));
            int check = await _context.SaveChangesAsync();
            if(check > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateComboDetail(Guid ComboDetailId ,ComboDetailCreationDto comboDetail)
        {
            
            comboDetail.ComboDetailId = ComboDetailId;
            _context.ComboDetails.Update(_mapper.Map<ComboDetail>(comboDetail));
            int check = await _context.SaveChangesAsync();
            if(check > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveComboDetail(Guid combodetail)
        {
            _context.ComboDetails.Remove(await GetComboDetailById(combodetail));
            int check = await _context.SaveChangesAsync();
            if (check > 0)
            {
                return true;
            }
            return false;

        }

        public async Task<IEnumerable<ComboDetail>> GetComboDetailsByProductId(Guid id)
        {
            return await _context.ComboDetails.Where(a => a.ProductId == id).ToListAsync();
        }
    }
}
