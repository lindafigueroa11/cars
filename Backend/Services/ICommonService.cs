using Backend.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services
{
    public interface ICommonService<T, TI, TU>
    {
        Task<IEnumerable<T>> Get();
        Task<T> GetById(int Id);
        Task<T> Add( TI carInsertDTOs );
        Task<T> Update(int Id, TU carUpdateDTOs);
        Task<T> Delete(int id);
    }
}
