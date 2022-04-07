using Core.Utilities.Results;
using DataAccess.Entities.BaseEntities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework
{
    public interface IRepository<TEntity>
    {
        IDataResult<IQueryable<TEntity>> GetAll(Expression<Func<TEntity, bool>>? filter = null);
        IDataResult<TEntity> GetById(int id); 

        Task <IResult> Create(TEntity entity);
        Task <IResult> Update(int id,TEntity entity);
        Task<IResult> Delete(int id);

        

    }
}
