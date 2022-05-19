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

        public List<Tarea> ObtenerTareasByAsignee(ObjectId id)
        {
            return null;
        }

        public List<UsuarioDB> ObtenerUsuariosByName(string nombre)
        {
            return this.db.GetUsersByName(nombre);
        }

        public UsuarioDB ObtenerUsuarioByEmail(string email)
        {
            return this.db.GetUserByEmail(email);
        }

        public void CreateTarea(Tarea t)
        {
            this.db.CreateTarea(t);
        }

        public ObjectId AddUser(UsuarioDB u)
        {
            return this.db.AddUser(u);
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