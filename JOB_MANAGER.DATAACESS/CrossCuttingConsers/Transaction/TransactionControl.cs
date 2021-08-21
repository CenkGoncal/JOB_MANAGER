using System;
using System.Transactions;
using System.Web.Mvc;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.DATAACESS.CrossCuttingConsers
{
    public class TransactionControl : ActionFilterAttribute
    {        
        private TransactionScope _transaction;
        private TransactionManager _transactionManager;
        public TransactionControl(TransactionType type)
        {
            _transactionManager = new TransactionManager(type);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
            _transaction = new TransactionScope(_transactionManager.transactionoptions.GetTransactionScopeOption(),
                                                _transactionManager.transactionoptions.GetTransactionOptions());

            base.OnActionExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)                   
        {
            try
            {
                base.OnResultExecuted(filterContext);
                
                if(_transactionManager.IsWrite)
                _transaction.Complete();
            }
            catch (Exception)
            {
                _transaction.Dispose();
            }

            
        }

        private abstract class TransactionFactory
        {
            public abstract TransactionScopeOption GetTransactionScopeOption();
            public abstract TransactionOptions GetTransactionOptions();
        }

        private class ReadTransactionScope : TransactionFactory
        {
            public override TransactionScopeOption GetTransactionScopeOption()
            {
                return TransactionScopeOption.RequiresNew;
            }

            public override TransactionOptions GetTransactionOptions()
            {
                return new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted,
                    Timeout = new System.TimeSpan(0, 0, 15, 0, 0)
                };
            }
        }

        private class WriteTransactionScope : TransactionFactory
        {
            public override TransactionScopeOption GetTransactionScopeOption()
            {
                return TransactionScopeOption.Required;
            }

            public override TransactionOptions GetTransactionOptions()
            {
                return new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                    Timeout = new System.TimeSpan(0, 0, 15, 0, 0)
                };
            }
        }

        private class TransactionManager
        {
            public TransactionManager(TransactionType TransactionTypeId)
            {
                if (TransactionType.Read == TransactionTypeId)
                {
                    transactionoptions = new ReadTransactionScope();
                    IsRead = true;
                }
                else
                if (TransactionType.Write == TransactionTypeId)
                {
                    transactionoptions = new WriteTransactionScope();
                    IsWrite = true;
                }
            }

            public TransactionFactory transactionoptions;
            public bool IsRead { get; set; }
            public bool IsWrite { get; set; }

        }
    }
}
