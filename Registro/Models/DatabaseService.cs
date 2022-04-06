using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Registro.Models
{
    public class DatabaseService
    {
        private MDatabase db;

        public DatabaseService()
        {
            this.db = new MDatabase("prueba");
        }

        public List<Usuario> ObtenerUsuariosByName(string nombre)
        {
            return this.db.GetUsersByName(nombre);
        }

        public void AddUser(Usuario u)
        {
            this.db.AddUser(u);
        }

        public ObjectId SaveFile(Stream file, string name)
        {
            return this.db.UploadFile(file, name);
        }

        public byte[] GetProfilePicture(ObjectId id)
        {
            return this.db.GetProfilePicture(id);
        }

    }
}