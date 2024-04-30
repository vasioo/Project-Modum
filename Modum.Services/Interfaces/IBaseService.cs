﻿using Modum.Models.MainModel;
using Modum.Models.BaseModels.Interfaces;
using System.Linq.Expressions;

namespace Modum.Services.Interfaces
{
    public interface IBaseService<T> where T : IEntity
    {
        Task<Guid> AddAsync(T entity);
        Task<int> AddRangeAsync(IEnumerable<T> entities);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> IQueryableGetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task<int> RemoveAsync(Guid id);
        Task<int> RemoveRangeAsync(IEnumerable<T> entities);
        Task<int> UpdateAsync(T entity);
        Task<int> UpdateRangeAsync(IEnumerable<T> entity);
        Task<int> GetCountOfAllItems();
        IQueryable<ApplicationUser> IQueryableGetUsersThatAreWorkers();
        IQueryable<ApplicationUser> IQueryableGetUsersThatAreAdmins();
    }
}
