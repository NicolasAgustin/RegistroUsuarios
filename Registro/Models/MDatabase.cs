using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MongoDB.Bson;

namespace Registro.Models
{
    public class MDatabase
    {
        public string ConnectionString { set; get; }
        public string DatabaseName { set; get; }
        public MongoClient Cliente { set; get; }
        private IMongoDatabase dbInstance;
        private GridFSBucket bucket;

        public MDatabase(string database)
        {
            this.ConnectionString = ConfigurationManager.AppSettings["connection_string"];
            Console.WriteLine(ConnectionString);
            this.Cliente = new MongoClient(ConnectionString);
            this.DatabaseName = database;
            this.dbInstance = Cliente.GetDatabase(database);
            this.bucket = new GridFSBucket(this.dbInstance, new GridFSBucketOptions { BucketName = "imagenes"});
        }

        public ObjectId UploadFile(Stream file, string filename)
        {
            try
            {
                ObjectId id = this.bucket.UploadFromStream(filename, file);
                return id;
            } catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return new ObjectId();
            }

        }

        public byte[] GetProfilePicture(ObjectId id)
        {
            var collection = this.dbInstance.GetCollection<ProfilePicture>("imagenes.files");
            ProfilePicture obtained = collection.Find(pic => pic._id == id).FirstOrDefault();
            byte[] picture = null;
            picture = this.bucket.DownloadAsBytesByName(obtained.filename);
            return picture;
        }

        public ObjectId AddUser(UsuarioDB u)
        {
            IMongoCollection<UsuarioDB> usuarios = dbInstance.GetCollection<UsuarioDB>("usuarios");
            ObjectId _newId = ObjectId.GenerateNewId();
            u._id = _newId;
            usuarios.InsertOne(u);
            return _newId;
        }

        public List<UsuarioDB> GetUser()
        {
            if (this.dbInstance == null)
                return null;

            IMongoCollection<UsuarioDB> usuarios = this.dbInstance.GetCollection<UsuarioDB>("usuarios");

            List<UsuarioDB> result = usuarios.Find(_ => true).ToList();

            return result;
        }

        public UsuarioDB GetUserByEmail(string email)
        {
            if (this.dbInstance == null)
                return null;

            IMongoCollection<UsuarioDB> usuarios = this.dbInstance.GetCollection<UsuarioDB>("usuarios");

            UsuarioDB user = usuarios.Find(obj => obj.Email.ToLower() == email.ToLower()).FirstOrDefault();

            return user;

        }

        public void CreateTarea(Tarea t)
        {
            if (this.dbInstance == null)
                return;
            IMongoCollection<TareaDB> tareas = this.dbInstance.GetCollection<TareaDB>("tareas");

            ObjectId _newId = ObjectId.GenerateNewId();

            TareaDB _new = new TareaDB(_newId, t);

            tareas.InsertOne(_new);
        }
        public List<TareaDB> GetTareasByAsignee(ObjectId id)
        {
            if (this.dbInstance == null)
                return null;
            IMongoCollection<TareaDB> tareas = this.dbInstance.GetCollection<TareaDB>("tareas");
            List<TareaDB> result = tareas.Find(doc => doc.Asignee == id).ToList();
            return result;
        }
        public List<UsuarioDB> GetUsersByName(string nombre)
        {
            if (this.dbInstance == null)
                return null;

            IMongoCollection<UsuarioDB> usuarios = this.dbInstance.GetCollection<UsuarioDB>("usuarios");

            List<UsuarioDB> result = usuarios.Find(obj => obj.Nombre.ToLower() == nombre.ToLower()).ToList();

            return result;

        }


    }

}