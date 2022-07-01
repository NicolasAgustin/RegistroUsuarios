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
                try
                {
                    IMongoCollection<TareaDB> collection = this.db.dbInstance.GetCollection<TareaDB>(collName);
                    t._id = ObjectId.GenerateNewId();
                    collection.InsertOne(t);
                    return t._id;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return default;
                }

            }
        }

        public TareaDB Update(TareaDB tsearch)
        {
            using (this.db = new MDatabase(this.dbName))
            {
                try
                {
                    IMongoCollection<TareaDB> collection = this.db.dbInstance.GetCollection<TareaDB>(collName);

                    TareaDB found =
                        collection.Find(doc => doc._id == tsearch._id).FirstOrDefault();

                    found.Title = tsearch.Title;
                    found.Description = tsearch.Description;
                    // Agregar mas campos para modificar

                    var filter = Builders<TareaDB>.Filter.Eq(doc => doc._id, tsearch._id);
                    // Por ahora usamos listas para poner las tareas
                    // En realidad cada grupo puede tener muchas listas de tareas
                    var update = Builders<TareaDB>.Update.Set(doc => doc.Title, found.Title)
                                                         .Set(doc => doc.Description, found.Description)
                                                         .Set(doc => doc.Asignee, found.Asignee)
                                                         .Set(doc => doc.Owner, found.Owner);
                    // Para actualizar mas campos, hay que agregar mas Set


                    UpdateResult result = collection.UpdateOne(filter, update);

                    return found;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
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