using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Concrete;
using JOB_MANAGER_BUSSINESS.Abstract;

namespace JOB_MANAGER.Bussiness.Concrete
{
    public class NoteTypeManager : IService<NOTE_TYPES, NoteTypeExtented>
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
