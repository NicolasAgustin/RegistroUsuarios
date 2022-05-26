using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Registro.Models
{
    public class TypesRepository
    {
        private MDatabase db;
        private string dbName;

        public TypesRepository(string dbName)
        {
            this.dbName = dbName;
        }

        public List<TaskType> GetById(ObjectId id)
        {
            using (this.db = new MDatabase(this.dbName))
            {
                return this.db.GetTypesByCreator(id);
            }
        }

        public List<TaskType> GetAll()
        {
            using (this.db = new MDatabase(this.dbName))
            {
                return this.db.GetAllTypes();
            }
        }

        public ObjectId CreateType(TaskType newType)
        {
            using(this.db = new MDatabase(this.dbName))
            {
                return this.db.CreateNewType(newType);
            }
        }

    }
}