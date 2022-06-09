using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace Registro.Models
{
    public class TareasRepository : Repository<TareaDB>
    {
        private MDatabase db;
        private string dbName;
        private string collName;
        public TareasRepository(string dbname)
        {
            this.dbName = dbname;
            this.collName = "tareas";
        }
        public TareaDB GetById(ObjectId id)
        {
            using (this.db = new MDatabase(this.dbName))
            {
                try
                {
                    IMongoCollection<TareaDB> collection = this.db.dbInstance.GetCollection<TareaDB>(collName);
                    TareaDB found = collection.Find(doc => doc._id == id).FirstOrDefault();
                    return found;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return default;
                }
            }
        }
        public ObjectId Insert(TareaDB t)
        {
            using (this.db = new MDatabase(this.dbName))
            {
                //var session = this.db.CreateSession();
                //session.StartTransaction();
                try
                {
                    IMongoCollection<TareaDB> collection = this.db.dbInstance.GetCollection<TareaDB>(collName);
                    t._id = ObjectId.GenerateNewId();
                    collection.InsertOne(t);
                    //session.CommitTransaction();
                    return t._id;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    //session.AbortTransaction();
                    return default;
                }

            }
        }
        public List<TareaDB> GetAll()
        {
            using (this.db = new MDatabase(this.dbName))
            {
                try
                {
                    IMongoCollection<TareaDB> collection = this.db.dbInstance.GetCollection<TareaDB>(collName);
                    List<TareaDB> result = collection.Find(_ => true).ToList();
                    return result;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return default;
                }

            }
        }
        public List<TareaDB> GetByAsigneeOrOwner(ObjectId id)
        {
            using (this.db = new MDatabase(this.dbName))
            {
                try
                {
                    IMongoCollection<TareaDB> collection = this.db.dbInstance.GetCollection<TareaDB>(collName);
                    List<TareaDB> result = collection.Find(doc => doc.Asignee == id || doc.Owner == id).ToList();
                    return result;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return default;
                }

            }
        }
        public List<TareaDB> GetByAsignee(ObjectId asignee)
        {
            using (this.db = new MDatabase(this.dbName))
            {
                try
                {
                    IMongoCollection<TareaDB> collection = this.db.dbInstance.GetCollection<TareaDB>(collName);
                    List<TareaDB> result = collection.Find(doc => doc.Asignee == asignee).ToList();
                    return result;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return default;
                }
            }
        }
        public List<TareaDB> GetByOwner(ObjectId owner)
        {
            using (this.db = new MDatabase(this.dbName))
            {
                try
                {
                    IMongoCollection<TareaDB> collection = this.db.dbInstance.GetCollection<TareaDB>(collName);
                    List<TareaDB> result = collection.Find(doc => doc.Owner == owner).ToList();
                    return result;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return default;
                }
            }
        }

        public List<TareaDB> GetManyById(List<ObjectId> ids)
        {
            using (this.db = new MDatabase(this.dbName))
            {
                try
                {
                    IMongoCollection<TareaDB> collection = 
                        this.db.dbInstance.GetCollection<TareaDB>(collName);
                    List<TareaDB> result = 
                        collection.Find(doc => ids.Contains(doc._id)).ToList();
                    return result;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return default;
                }
            }
        }

    }
}