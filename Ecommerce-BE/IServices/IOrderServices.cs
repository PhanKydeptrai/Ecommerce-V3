using Domain.DTO_Models;
using Domain.Models;

namespace Ecommerce_BE.IServices
{
    public interface IOrderServices
    {
        //Get all orders
        Task<IEnumerable<Order>> GetAllOrders();
        //Get list of orders by user id
        Task<IEnumerable<Order>> GetOrdersByUserID(Guid id);
        //Get Order by order ID
        Task<Order> GetOrderByID(Guid id);
        //Get Order by Status
        Task<IEnumerable<Order>> GetOrderByStatus(string status);
        //Create new order
        Task<bool> CreateNewOrder(OrderCreationDto order);
        //Update order status
        Task<bool> UpdateStatus(OrderCreationDto order);

        //Get order by order id
        Task<bool> UpdateOrderPrice(Order order);
    }
}
