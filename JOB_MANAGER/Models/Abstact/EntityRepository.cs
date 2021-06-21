using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace JOB_MANAGER.Models.Abstact
{
    public interface IEntityRepository<T,TDto>
    {
        List<T> GetAll(Expression<Func<T, bool>> filter = null);
        List<TDto> GetAll2(Expression<Func<T, bool>> filter = null);
        T Get(Expression<Func<T, bool>> filter);
        ShowState AddorUpdate(T param, Expression<Func<T, bool>> filter);
        ShowState Delete(T param);
    }
}