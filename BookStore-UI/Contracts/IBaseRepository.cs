using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_UI.Contracts
{
   public interface IBaseRepository<T>  where T:class
    {
        Task<T> GetById(string url, int id);
        Task<T> GetAll(string url);
        Task<T> Create(string url,T obj, int id);
        Task<bool> Update(string url,T obj);
        Task<bool> Delete(string url,int id);
    }
}
