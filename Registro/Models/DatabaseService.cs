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
        private UsersRepository urepo;

        public DatabaseService()
        {
            this.urepo = new UsersRepository("prueba");
        }

        public List<Tarea> ObtenerTareasByAsignee(ObjectId id)
        {
            return null;
        }

        public List<UsuarioDB> ObtenerUsuariosByName(string nombre)
        {
            return null;
        }
        public UsuarioDB ObtenerUsuarioByEmail(string email)
        {
            return this.urepo.GetUserByEmail(email);
        }
        public UsuarioDB ObtenerUsuarioByEmailAndPass(string email, string password)
        {
            return this.urepo.GetUserDetails(email, password);
        }

        public void CreateTarea(Tarea t)
        {
            // Falta el repo para las tareas
        }

        public ObjectId AddUser(UsuarioDB u)
        {
            return this.urepo.addUser(u);
        }

        public ObjectId SaveFile(Stream file, string name)
        {
            return ObjectId.Empty;
        }

        public byte[] GetProfilePicture(ObjectId id)
        {
            return this.urepo.GetProfilePhoto(id);
        }

    }
}