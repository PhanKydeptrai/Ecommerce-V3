using AutoMapper;
using Data;
using Domain.DTO_Models;
using Domain.Models;
using Ecommerce_BE.IServices;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_BE.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly EcommerceDbContext _context;
        private readonly IMapper _mapper;
        public OrderServices() { }
        public OrderServices(EcommerceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //Get all orders
        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _context.Orders.AsNoTracking().ToListAsync();
        }

        //Get list of order by UserID
        public async Task<IEnumerable<Order>> GetOrdersByUserID(Guid id)
        {
            IEnumerable<Order> orders = await _context.Orders.AsNoTracking().Include(o => o.OrderDetails).Where(x => x.UserId == id).ToListAsync();
            return orders;
        }

        //Get order by Id
        public async Task<Order> GetOrderByID(Guid id)
        {
            return await _context.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.OrderId == id);
        }

        //Create new order
        public async Task<bool> CreateNewOrder(OrderCreationDto order)
        {
            await _context.Orders.AddAsync(_mapper.Map<Order>(order));
            int check = await _context.SaveChangesAsync();
            if (check > 0)
            {
                return true;
            }
            return false;
        }

        //Update status of order
        public async Task<bool> UpdateStatus(OrderCreationDto order)
        {
            _context.Orders.Update(_mapper.Map<Order>(order));
            int check = await _context.SaveChangesAsync();
            if (check > 0)
            {
                return true;
            }
            return false;
        }

        //Update order
        public async Task<bool> UpdateOrderPrice(Order order)
        {
            _context.Orders.Update(order);
            int check = await _context.SaveChangesAsync();
            if (check > 0)
            {
                return true;
            }
            return false;
        }

        //Get order by status
        public async Task<IEnumerable<Order>> GetOrderByStatus(string status)
        {
            IEnumerable<Order> orders = await _context.Orders.Where(x => x.Status == status).Include(o => o.OrderDetails).ToListAsync();
            return orders;
        }
    }
}
