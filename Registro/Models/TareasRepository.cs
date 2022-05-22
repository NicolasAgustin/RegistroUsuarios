using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Registro.Models
{
    public class TareasRepository
    {
        private MDatabase db;
        private string dbName;
        public TareasRepository(string dbname)
        {
            this.dbName = dbname;
        }
        public List<TareaDB> GetTasksByAsignee(ObjectId id)
        {
            using (this.db = new MDatabase(this.dbName))
            {
                return this.db.GetTareasByAsignee(id);
            }
        }
        public ObjectId CreateTask(Tarea t)
        {
            using (this.db = new MDatabase(this.dbName))
            {
                return this.db.CreateTarea(t);
            }
        }
    }
}