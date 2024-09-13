using apitask.Models;

namespace apitask.DAO
{
    public interface IPersonDao
    {
        Task<int> AddPerson(Person person);
        Task<List<Person>> GetAllPersons();
        Task<bool> DeletePersonByAadharCardNumber(string aadharCardNumber);

        Task<bool> UpdatePersonNameById(int id, string newName);

        Task<bool> UpdatePersonNameByAadhar(string aadharCardNumber,string newName);
    }
}
