using Domain.DTO_Models;
using Domain.Models;

namespace Ecommerce_BE.IServices
{
    public interface IComboServices
    {
        //Gets all combos
        Task<IEnumerable<Combo>> GetCombos();
        //Gets combo by id
        Task<Combo> GetComboById(Guid id);
        
        //Get combo by name
        Task<Combo> GetComboByName(string name);
        //Adds new combo
        Task<bool> AddCombo(ComboCreationDto combo);
        //Updates combo
        Task<bool> UpdateCombo(ComboCreationDto combo);
        //Removes combo by ItemId
        Task<bool> RemoveCombo(Guid itemid);
        
    }
}
