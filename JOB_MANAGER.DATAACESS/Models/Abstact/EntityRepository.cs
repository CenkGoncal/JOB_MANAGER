using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace JOB_MANAGER.DATAACESS.Models
{
    public interface IEntityRepository<T,TDto> : IDisposable
    {
        List<T> GetAll(Expression<Func<T, bool>> filter = null);
        List<TDto> GetAll2(Expression<Func<T, bool>> filter = null);
        T Get(Expression<Func<T, bool>> filter);
        ShowState AddorUpdate(T param, Expression<Func<T, bool>> filter);
        ShowState Delete(T param);
    }
}
