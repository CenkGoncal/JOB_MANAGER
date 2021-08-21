using JOB_MANAGER.DATAACESS.Models;
using System.Collections.Generic;
using static JOB_MANAGER.DATAACESS.Models.Entity;

namespace JOB_MANAGER.Business.Abstract
{
    public interface IService<T, TDto>
        //where T : class, IEntity, new()        
        where TDto : class, IViewDto, new()
    {
        List<TDto> GetAll();        
        T Get(T param);
        ShowState AddorUpdate(T param);        
        ShowState Delete(T param);
    }
}
