using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Registro.Models
{
    public class UsersRepository : Repository<UsuarioDB>
    {
        private MDatabase db;
        private string dbName;
        private string collName;
        public UsersRepository(string dbname)
        {
            this.dbName = dbname;
            this.collName = "usuarios";
        }
        public List<UsuarioDB> GetAll()
        {
            using (this.db = new MDatabase(this.dbName))
            {
                try
                {
                    IMongoCollection<UsuarioDB> collection = this.db.dbInstance.GetCollection<UsuarioDB>(collName);
                    List<UsuarioDB> result = collection.Find(_ => true).ToList();
                    return result;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return default;
                }

            }
        }
        public UsuarioDB GetByEmailAndPass(string email, string password)
        {
            using (this.db = new MDatabase(this.dbName))
            {
                try
                {
                    IMongoCollection<UsuarioDB> collection = this.db.dbInstance.GetCollection<UsuarioDB>(collName);
                    UsuarioDB found = collection.Find(obj => obj.Email.ToLower() == email.ToLower()
                                                             && obj.Password == password
                                                      ).FirstOrDefault();
                    return found;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return default;
                }
            }
        }
        public UsuarioDB GetById(ObjectId id)
        {
            using (this.db = new MDatabase(this.dbName))
            {
                try
                {
                    IMongoCollection<UsuarioDB> collection = this.db.dbInstance.GetCollection<UsuarioDB>(collName);
                    UsuarioDB found = collection.Find(doc => doc._id == id).FirstOrDefault();
                    return found;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return default;
                }

            }
        }
        public UsuarioDB GetByName(string name)
        {
            using (this.db = new MDatabase(this.dbName))
            {
                try
                {
                    IMongoCollection<UsuarioDB> collection = this.db.dbInstance.GetCollection<UsuarioDB>(collName);
                    var builder = Builders<UsuarioDB>.Filter;
                    var filter = builder.Eq(doc => doc.Nombre, name.Split(' ')[0])
                                & builder.Eq(doc => doc.Apellido, name.Split(' ')[1]);
                    UsuarioDB found = collection.Find(filter).FirstOrDefault();
                    return found;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return default;
                }
            }
        }
        public List<UsuarioDB> GetByNames(string name)
        {
            using (this.db = new MDatabase(this.dbName))
            {
                try
                {
                    IMongoCollection<UsuarioDB> collection = this.db.dbInstance.GetCollection<UsuarioDB>(collName);
                    List<UsuarioDB> result = collection.Find(obj => obj.Nombre.ToLower() == name.ToLower()).ToList();
                    return result;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return default;
                }
            }
        }
        public UsuarioDB GetByEmail(string email)
        {
            using (this.db = new MDatabase(this.dbName))
            {
                try
                {
                    IMongoCollection<UsuarioDB> collection = this.db.dbInstance.GetCollection<UsuarioDB>(collName);

                    UsuarioDB found = collection.Find(doc => doc.Email == email).FirstOrDefault();
                    return found;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return default;
                }

            }
        }
        public byte[] GetProfilePhoto(ObjectId id)
        {
            byte[] photo = null;
            using (MDatabase db = new MDatabase(this.dbName))
            {
                photo = db.GetProfilePicture(id);
            }
            return photo;
        }
        public ObjectId Insert(UsuarioDB u)
        {
            using (this.db = new MDatabase(this.dbName))
            {
                var session = this.db.CreateSession();
                try
                {
                    IMongoCollection<UsuarioDB> collection = this.db.dbInstance.GetCollection<UsuarioDB>(collName);
                    u._id = ObjectId.GenerateNewId();
                    collection.InsertOne(u);
                    session.CommitTransaction();
                    return u._id;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    session.AbortTransaction();
                    return default;
                }

            }
        }
    }
}