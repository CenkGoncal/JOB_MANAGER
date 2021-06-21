using JOB_MANAGER.Models.Abstact;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using static JOB_MANAGER.Models.Abstact.Entity;

namespace JOB_MANAGER.Models.Concrete
{
    public class EntityRepositoryBase<TEntity, TContext, TDto > : IEntityRepository<TEntity,TDto>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
        where TDto : class, IViewDto, new()
    {

        public ShowState AddorUpdate(TEntity param, Expression<Func<TEntity, bool>> filter)
        {
            using (TContext context = new TContext())
            {
                ShowState showState = new ShowState();

                showState = HandleExepciton(() =>
                {
                    SaveControls(param, showState);

                    if (!showState.isError)
                    {
                        var isOld = context.Set<TEntity>().Where(filter).FirstOrDefault();
                        SaveHelper_ModifyBeforeSave(param, isOld);

                        if (isOld == null)
                        {
                            var added = context.Entry(param);
                            added.State = EntityState.Added;
                        }
                        else
                        {                            
                            context.Entry(isOld).State = EntityState.Detached;

                            context.Set<TEntity>().Attach(param);
                            context.Entry(param).State = EntityState.Modified;
                        }

                        context.SaveChanges();
                    }                    
                }, showState);            

                return showState;
            }
        }

        public ShowState Delete(TEntity param)
        {
            using (TContext context = new TContext())
            {
                ShowState showState = new ShowState();             

                showState = HandleExepciton(() =>
                {
                    DeleteControls(param, showState);

                    if (!showState.isError)
                    {
                        var deleted = context.Entry(param);
                        deleted.State = EntityState.Deleted;
                    }

                    context.SaveChanges();
                }, showState);

                return showState;
            }
        }

        private ShowState HandleExepciton(Action action, ShowState showState)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception dbEx)
            {
                //string strError = "DbEntityValidationException Error From WorkOrder.Save() for context.SaveChanges()...<br/>";
                //foreach (var validationErrors in dbEx.EntityValidationErrors)
                //{
                //    foreach (var validationError in validationErrors.ValidationErrors)
                //    {
                //        string tempStr = string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                //        strError = strError + " " + tempStr + "<br/>";
                //    }
                //}

                showState.isError = true;
                showState.ErrorMessage = dbEx.Message;
            }

            return showState;
        }

        public virtual TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            return GetAll(filter).FirstOrDefault();
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            using (TContext context = new TContext())
            {
                return  filter != null ? context.Set<TEntity>().Where(filter).ToList()
                                : context.Set<TEntity>().ToList();
            }
        }

        public virtual List<TDto> GetAll2(Expression<Func<TEntity, bool>> filter = null)
        {
            return null;
        }

        public virtual void SaveControls(TEntity entity, ShowState showState)
        {            
        }

        public virtual void DeleteControls(TEntity entity, ShowState showState)
        {      
        }

        public virtual void SaveHelper_ModifyBeforeSave(TEntity param, TEntity dbitem)
        {

        }

    }
}