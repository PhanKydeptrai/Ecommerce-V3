using Domain.DTO_Models;
using Domain.Models;

namespace Ecommerce_BE.IServices
{
    public interface IOrderDetailServices
    {
        Task<bool> CreateNewOrderDetail(OrderDetailCreationDto orderDetailCreationDto);
        Task<bool> RemoveOrderDetail(Guid id);
        Task<OrderDetail> GetOrderDetailByID(Guid id);
        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderID(Guid id);
        Task<IEnumerable<OrderDetail>> GetOrderDetails();
        Task<bool> UpdateOrderDetail(OrderDetail orderDetail);
    }
}
