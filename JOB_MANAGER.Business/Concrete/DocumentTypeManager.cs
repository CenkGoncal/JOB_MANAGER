using System.Collections.Generic;
using System.Linq;
using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.Business.Concrete
{
    public class DocumentTypeManager: IDocumentTypeService
    {
        private DocumentTypeDal _dal;

        public DocumentTypeManager(DocumentTypeDal documentType)
        {
            _dal = documentType;
        }

        public List<DocumentTypeExtented> GetAll()
        {
            return _dal.GetAll2();
        }

        public DOCUMENT_TYPES Get(DOCUMENT_TYPES param)
        {
            return _dal.GetAll(f => f.DOCUMENT_TYPE_ID == param.DOCUMENT_TYPE_ID).FirstOrDefault();
        }

        public ShowState AddorUpdate(DOCUMENT_TYPES param)
        {
            return _dal.AddorUpdate(param,f => f.DOCUMENT_TYPE_ID == param.DOCUMENT_TYPE_ID);
        }

        public ShowState Delete(DOCUMENT_TYPES param)
        {
            return _dal.Delete(param);
        }
    }
}
