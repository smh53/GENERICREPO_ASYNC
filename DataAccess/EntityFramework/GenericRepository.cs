using Core.Utilities.Results;
using DataAccess.Context;
using DataAccess.Entities.BaseEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly GenericRepoContext _context;
        private DbSet<TEntity>? _entities = null;

        public GenericRepository(GenericRepoContext context)
        {
            _context = context;
            _entities = _context.Set<TEntity>();
        }

        protected virtual DbSet<TEntity> Entities => _entities ??= _context.Set<TEntity>();
        public IQueryable<TEntity> Table => Entities;
        public IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();

        public async Task<IDataResult<TEntity>> Get(Expression<Func<TEntity, bool>> filter)
        {
            var result = await Entities.SingleOrDefaultAsync(filter);
            if (result != null)
                return new SuccessDataResult<TEntity>(result);

            return new ErrorDataResult<TEntity>();
        }

        public virtual async Task<IResult> Create(TEntity entity)
        {
          
          try
            {
                await _context.Set<TEntity>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return new SuccessResult();
            }
          catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
            
        }

        public async Task<IResult> Delete(int id)
        {
            try
            {
                var entity = GetById(id);
                _context.Set<TEntity>().Remove(entity.Data);
                await _context.SaveChangesAsync();
                return new SuccessResult();
            }
           catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }

        public virtual IDataResult<IQueryable<TEntity>> GetAll(Expression<Func<TEntity, bool>>? filter = null)
        {
            try
            {
                var result = filter == null ?
                   _context.Set<TEntity>().AsNoTracking() :
                    _context.Set<TEntity>().Where(filter).AsNoTracking();

                return new SuccessDataResult<IQueryable<TEntity>>(result);
            }
           catch (Exception ex)
            {
                return new ErrorDataResult<IQueryable<TEntity>>(ex.Message);
            }
        }

        public virtual  IDataResult<TEntity> GetById(int id)
        {
            var result = _context.Set<TEntity>().AsNoTracking().FirstOrDefault(f => f.Id == id);
            
            if(result != null)
                return  new  SuccessDataResult<TEntity>(result);


            return new ErrorDataResult<TEntity>();
        }

        public virtual async Task<IResult> Update(int id, TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Update(entity);
                await _context.SaveChangesAsync();
                return new SuccessResult();
            }
            catch(Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
            
        }

        public IEnumerable<TEntity> GetSql(string sql)
        {
            return Entities.FromSqlRaw(sql);
        }

        
    }
}
