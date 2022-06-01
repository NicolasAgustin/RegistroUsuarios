using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace Registro.Models
{
    public class TypesRepository : Repository<TaskType>
    {
        private MDatabase db;
        private string dbName;
        private string collName;

        public TypesRepository(string dbName)
        {
            this.dbName = dbName;
            this.collName = "task-types";
        }

        public TaskType GetById(ObjectId id)
        {
            using (this.db = new MDatabase(this.dbName))
            {
                try
                {
                    IMongoCollection<TaskType> collection = this.db.dbInstance.GetCollection<TaskType>(collName);
                    TaskType found = collection.Find(doc => doc._id == id).FirstOrDefault();
                    return found;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return default;
                }

            }
        }

        public List<TaskType> GetAll()
        {
            using (this.db = new MDatabase(this.dbName))
            {
                try
                {
                    IMongoCollection<TaskType> collection = this.db.dbInstance.GetCollection<TaskType>(collName);
                    List<TaskType> found = collection.Find(_ => true).ToList();
                    return found;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return default;
                }

            }
        }

        public ObjectId Insert(TaskType newType)
        {
            using (this.db = new MDatabase(this.dbName))
            {
                //var session = this.db.CreateSession();
                //session.StartTransaction();
                try 
                {
                    IMongoCollection<TaskType> collection = this.db.dbInstance.GetCollection<TaskType>(collName);
                    newType._id = ObjectId.GenerateNewId();
                    collection.InsertOne(newType);
                    //session.CommitTransaction();
                    return newType._id;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    //session.AbortTransaction();
                    return ObjectId.Empty;
                }

            }
        }

        public List<TaskType> GetTypesByCreator(ObjectId creator)
        {
            using (this.db = new MDatabase(this.dbName))
            {
                try
                {
                    IMongoCollection<TaskType> collection = this.db.dbInstance.GetCollection<TaskType>(collName);
                    List<TaskType> result = collection.Find(doc => creator == doc.Creator).ToList();
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