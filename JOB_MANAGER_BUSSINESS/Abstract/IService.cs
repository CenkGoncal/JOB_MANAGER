using JOB_MANAGER.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static JOB_MANAGER.Models.Abstact.Entity;

namespace JOB_MANAGER_BUSSINESS.Abstract
{
    public interface IService<T, TDto>
        where T : class, IEntity, new()        
        where TDto : class, IViewDto, new()
    {
        List<TDto> GetAll();        
        T Get(T param);
        ShowState Add(T param);
        ShowState Update(T param);
        ShowState Delete(T param);
    }
}
