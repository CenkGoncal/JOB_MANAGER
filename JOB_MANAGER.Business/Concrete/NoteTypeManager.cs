using System.Collections.Generic;
using System.Linq;
using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.Business.Concrete
{
    public class NoteTypeManager : INoteTypeService
    {
        private NoteTypeDal _dal;
        public NoteTypeManager(NoteTypeDal noteType)
        {
            _dal = noteType;
        }
        public ShowState AddorUpdate(NOTE_TYPES param)
        {
            return _dal.AddorUpdate(param, f => f.NOTE_TYPE_ID == param.NOTE_TYPE_ID);
        }

        public ShowState Delete(NOTE_TYPES param)
        {
            return _dal.Delete(param);
        }

        public NOTE_TYPES Get(NOTE_TYPES param)
        {
            return _dal.GetAll(f => f.NOTE_TYPE_ID == param.NOTE_TYPE_ID).FirstOrDefault();
        }

        public List<NoteTypeExtented> GetAll()
        {
            return _dal.GetAll2();
        }
    }
}
