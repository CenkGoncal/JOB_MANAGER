using JOB_MANAGER.CrossCuttingConsers.Loging;
using JOB_MANAGER.CrossCuttingConsers.Loging.Log4net;
using JOB_MANAGER.CrossCuttingConsers.Loging.Log4net.Loggers;
using JOB_MANAGER.Models.Abstact;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
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

        protected TContext context;
        protected LoggerService loggerService;

        public EntityRepositoryBase()
        {
            context = (TContext)Activator.CreateInstance(typeof(TContext));
            loggerService = new FileLogger();
        }


        public ShowState AddorUpdate(TEntity param, Expression<Func<TEntity, bool>> filter)
        {            
            using (context)
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
                    else
                    {
                        List<LogParameter> Parameters = new List<LogParameter>();
                        Parameters.Add(new LogParameter
                        {
                            Name = typeof(TEntity).Name,
                            Type = "ErrorMessage",
                            Value = param
                        });
                        CreateLog(this.GetType().FullName, "AddorUpdate", Parameters, showState.ErrorMessage, "Error");
                    }
                    
                }, showState);            

                return showState;
            }
        }

        public ShowState Delete(TEntity param)
        {
            using (context)
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
                    else
                    {
                        List<LogParameter>  Parameters = new List<LogParameter>();
                        Parameters.Add(new LogParameter
                        {
                            Name = typeof(TEntity).Name,
                            Type = "ErrorMessage",
                            Value = param
                        }
                        );

                        CreateLog(this.GetType().FullName, "Delete", Parameters, showState.ErrorMessage, "Error");
                    }
                    context.SaveChanges();
                }, showState);

                return showState;
            }
        }

        private void CreateLog(string fullName,string methodName, List<LogParameter> parameters, string message,string logType)
        {
            LogDetail logDetail = new LogDetail();
            logDetail.Fullname = fullName;
            logDetail.MethodName = methodName;
            logDetail.Parameters = new List<LogParameter>();

            logDetail.Parameters.AddRange(parameters);
            logDetail.Message = message;

            if(logType == "Error")
                loggerService.Error(logDetail);
            else
            if(logType == "Fatal")
                loggerService.Fatal(logDetail);

        }

        protected ShowState HandleExepciton(Action action, ShowState showState)
        {
            try
            {
                
                //var parameters = action.GetType().GetMethod(.Name)

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
                var st = new StackTrace(new StackFrame(1));

              
                List<LogParameter> Parameters = new List<LogParameter>();                
                Parameters.Add(new LogParameter
                {
                    Name = typeof(TEntity).Name,
                    Type = "ErrorMessage",
                    Value = st.GetFrame(0).GetMethod().GetParameters()
                }
                );

                CreateLog(this.GetType().FullName, st.GetType().Name, Parameters, dbEx.Message, "Fatal");

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

                var a = context.Set<TEntity>().Where(filter).ToList();

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); //if you call this, prevent GC to call this. If you did not call this, GC will call this
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if(isDisposing)
            {
                if(context != null)
                {
                    context.Dispose();
                    context = null;
                }
            }
        }

        
    }
}
