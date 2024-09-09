using AutoMapper;
using Data;
using Domain.DTO_Models;
using Domain.Models;
using Ecommerce_BE.IServices;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_BE.Services
{
    public class OrderDetailServices :IOrderDetailServices
    {
        private readonly EcommerceDbContext _context;
        private readonly IMapper mapper;
        public OrderDetailServices() { }
        public OrderDetailServices(EcommerceDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        //Create new OrderDetail
        public async Task<bool> CreateNewOrderDetail(OrderDetailCreationDto orderDetailCreationDto)
        {
            await _context.OrderDetails.AddAsync(mapper.Map<OrderDetail>(orderDetailCreationDto));
            int check = await _context.SaveChangesAsync();
            if (check > 0)
            {
                return true;
            }
            return false;
        }
        //Update OrderDetail
        
        //Remove OrderDetail (Xoá luôn)
        public async Task<bool> RemoveOrderDetail(Guid id)
        {
            _context.OrderDetails.Remove(await GetOrderDetailByID(id));
            int check = await _context.SaveChangesAsync();
            if (check > 0)
            {
                return true;
            }
            return false;
        }

        //Get list of OrderDetail by OrderId
        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderID(Guid id)
        {
            return await _context.OrderDetails.Where(a => a.OrderId == id).ToListAsync();
        }
        //Get OrderDetail by id
        public async Task<OrderDetail> GetOrderDetailByID(Guid id)
        {
            return await _context.OrderDetails.Include(a => a.Order).FirstOrDefaultAsync(x => x.OrderDetailId == id);
        }

        //Get all orderdetails
        public async Task<IEnumerable<OrderDetail>> GetOrderDetails()
        {
            return await _context.OrderDetails.ToListAsync();
        }

        public async Task<bool> UpdateOrderDetail(OrderDetail orderDetailUpdateDto)
        {
            _context.OrderDetails.Update(orderDetailUpdateDto);
            int check = await _context.SaveChangesAsync();
            if(check > 0)
            {
                return true;
            }
            return false;
        }
    }
}
