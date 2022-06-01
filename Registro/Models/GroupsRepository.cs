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
        public List<Group> GetGroupByName(string name)
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
    }
}