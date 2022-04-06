using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson;

namespace Registro.Models
{
    public class ProfilePicture
    {
        public ObjectId _id { set; get; }
        public long length { set; get; }
        public int chunkSize { set; get; }
        public BsonDateTime uploadDate { set; get; }
        public string md5 { set; get; }
        public string filename { set; get; }


    }
}