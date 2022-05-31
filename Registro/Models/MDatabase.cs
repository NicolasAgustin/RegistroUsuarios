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
    public class MDatabase : IDisposable
    {
        public string ConnectionString { set; get; }
        public string DatabaseName { set; get; }
        public MongoClient Cliente { set; get; }
        public IMongoDatabase dbInstance;
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

        public IClientSessionHandle CreateSession()
        {
            return this.Cliente.StartSession();
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

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public byte[] GetProfilePicture(ObjectId id)
        {
            var collection = this.dbInstance.GetCollection<ProfilePicture>("imagenes.files");
            ProfilePicture obtained = collection.Find(pic => pic._id == id).FirstOrDefault();
            byte[] picture = null;
            picture = this.bucket.DownloadAsBytesByName(obtained.filename);
            return picture;
        }
    }

}