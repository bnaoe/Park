using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace ParkWebApp.Repository.IRepository
{
    public interface IRepository<T> where T: class
    {
        Task<T> GetAsync(string url, int id);
        Task<IEnumerable<T>> GetAllAsync(string url);
        Task<bool> CreateAsync(string url, T objCreate);
        Task<bool> UpdateAsync(string url, T objTUpdate);
        Task<bool> DeleteAsync(string url, int id);

    }
}
