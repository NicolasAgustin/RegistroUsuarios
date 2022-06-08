using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using System.Diagnostics;

namespace Registro.Models
{
    public class GroupsRepository : Repository<Group>
    {
        private MDatabase db;
        private string dbName;
        private string collName;

        public GroupsRepository(string dbName)
        {
            this.dbName = dbName;
            this.collName = "groups";
        }

        public Group GetById(ObjectId id)
        {
            using (this.db = new MDatabase(this.dbName))
            {
                try
                {
                    IMongoCollection<Group> collection = this.db.dbInstance.GetCollection<Group>(collName);
                    Group found = collection.Find(doc => doc._id == id).FirstOrDefault();
                    return found;    
                } catch(Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return default;
                }
                
            }
        }

        public List<Group> GetAll()
        {
            using (this.db = new MDatabase(this.dbName))
            {
                try
                {
                    IMongoCollection<Group> collection = this.db.dbInstance.GetCollection<Group>(collName);
                    List<Group> groups = collection.Find(_ => true).ToList();
                    return groups;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return default;
                }
            }
        }

        public ObjectId Insert(Group newGroup)
        {
            using (this.db = new MDatabase(this.dbName))
            {
                //var session = this.db.CreateSession();
                //session.StartTransaction();
                try
                {
                    IMongoCollection<Group> collection = this.db.dbInstance.GetCollection<Group>(collName);
                    newGroup._id = ObjectId.GenerateNewId();
                    collection.InsertOne(newGroup);
                    //session.CommitTransaction();
                    return newGroup._id;
                } catch(Exception e)
                {
                    Debug.WriteLine(e.Message);
                    //session.AbortTransaction();
                    return ObjectId.Empty;
                }
            }
        }

        public List<Group> GetGroupsByParticipant(ObjectId participant)
        {
            using (this.db = new MDatabase(this.dbName))
            {
                try
                {
                    IMongoCollection<Group> collection = this.db.dbInstance.GetCollection<Group>(collName);
                    List<Group> result = collection.Find(doc => doc.Members.Contains(participant)
                                                                || doc.Creator == participant
                                                                || doc.Editors.Contains(participant)
                                                         ).ToList();
                    return result;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return default;
                }
            }
        }
        public List<Group> GetGroupsByName(string name)
        {
            using (this.db = new MDatabase(this.dbName))
            {
                try
                {
                    IMongoCollection<Group> collection = this.db.dbInstance.GetCollection<Group>(collName);
                    List<Group> result = collection.Find(doc => doc.Nombre == name).ToList();
                    return result;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return default;
                }
            }
        }

        public Group GetGroupByName(string name)
        {
            using (this.db = new MDatabase(this.dbName))
            {
                try
                {
                    IMongoCollection<Group> collection = this.db.dbInstance.GetCollection<Group>(collName);
                    Group result = collection.Find(doc => doc.Nombre == name).FirstOrDefault();
                    return result;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return default;
                }
            }
        }

        public Group AddTask(string gname, ObjectId id)
        {
            using (this.db = new MDatabase(this.dbName))
            {
                try
                {
                    Group found = this.GetGroupByName(gname);
                    if (found.Listas is null)
                        found.Listas = new List<ObjectId>();
                    found.Listas.Add(id);
                    IMongoCollection<Group> collection =
                        this.db.dbInstance.GetCollection<Group>(collName);

                    var filter = Builders<Group>.Filter.Eq(doc => doc._id, found._id);
                    // Por ahora usamos listas para poner las tareas
                    // En realidad cada grupo puede tener muchas listas de tareas
                    var update = Builders<Group>.Update.Set(doc => doc.Listas, found.Listas);

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
    }
}