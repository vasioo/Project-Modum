using Microsoft.EntityFrameworkCore;
using Modum.DataAccess;
using Modum.Models.MainModel;
using Modum.Services.Interfaces;
using System.Linq.Expressions;
using IEntity = Modum.Models.BaseModels.Interfaces.IEntity;

namespace Modum.Services.Services
{
    public class BaseService<T> : IBaseService<T> where T : class, IEntity
    {
        private readonly DataContext _context;

        public BaseService(DataContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<int> AddRangeAsync(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
            await _context.SaveChangesAsync();

            return entities.Count();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public IQueryable<T> IQueryableGetAllAsync()
        {
            return _context.Set<T>();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
            {
                return Activator.CreateInstance<T>();
            }
            return entity;
        }

        public async Task<int> GetCountOfAllItems()
        {
            return await _context.Set<T>().CountAsync();
        }

        public async Task<int> RemoveAsync(Guid id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
                return 0;

            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> RemoveRangeAsync(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(T entity)
        {
            if (!_context.Set<T>().Local.Any(e => e.Id == entity.Id))
            {
                _context.Set<T>().Attach(entity);
            }
            _context.Entry(entity).State = EntityState.Modified;

            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateRangeAsync(IEnumerable<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
            return await _context.SaveChangesAsync();
        }

        public IQueryable<ApplicationUser> IQueryableGetUsersThatAreWorkers()
        {
            var users = (from user in _context.Users
                         join userRole in _context.UserRoles
                         on user.Id equals userRole.UserId
                         join role in _context.Roles
                         on userRole.RoleId equals role.Id
                         where role.Name == "WORKER"
                         select user);
            return users;
        }
        public IQueryable<ApplicationUser> IQueryableGetUsersThatAreAdmins()
        {
            var users = (from user in _context.Users
                         join userRole in _context.UserRoles
                         on user.Id equals userRole.UserId
                         join role in _context.Roles
                         on userRole.RoleId equals role.Id
                         where role.Name == "WORKER"
                         select user);
            return users;
        }
    }
}
