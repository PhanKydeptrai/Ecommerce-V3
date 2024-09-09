using Domain.DTO_Models;
using Domain.Models;

namespace Ecommerce_BE.IServices
{
    public interface IComboDetailServices
    {
        //Get all combo detail
        Task<IEnumerable<ComboDetail>> GetComboDetails();
        //Get combo detail by id
        Task<ComboDetail> GetComboDetailById(Guid id);
        //Get get combodetail by comboId
        Task<IEnumerable<ComboDetail>> GetComboDetailByComboId(Guid id);
        //Add new combo detail
        Task<bool> AddComboDetail(ComboDetailCreationDto comboDetail);
        //Update combo detail
        Task<bool> UpdateComboDetail(Guid ComboDetailId, ComboDetailCreationDto comboDetail);
        //Remove combo detail
        Task<bool> RemoveComboDetail(Guid combodetail);
        //
        Task<IEnumerable<ComboDetail>> GetComboDetailsByProductId(Guid id);
    }
}
