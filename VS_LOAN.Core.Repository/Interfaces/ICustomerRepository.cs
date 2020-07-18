using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.Model;

namespace VS_LOAN.Core.Repository.Interfaces
{
    public interface ICustomerRepository
    {
        Task<int> Create(Customer customer);
        Task<bool> AddNote(CustomerNote note);
        Task<bool> Update(Customer customer);
        Task<Customer> GetById(int customerId);
        Task<bool> DeleteCustomerCheck(int customerId);
        Task<List<int>> GetCustomerCheckByCustomerId(int customerId);
        Task<bool> InserCustomerCheck(CustomerCheck check);
        Task<int> Count(string freeText, int userId);
        Task<List<Customer>> Gets(
            string freeText,
            int page,
            int limit
            , int userId);
        Task<List<CustomerNoteViewModel>> GetNoteByCustomerId(int customerId);
    }
}
