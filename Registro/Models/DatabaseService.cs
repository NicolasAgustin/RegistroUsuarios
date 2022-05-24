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
        private TareasRepository trepo;
        public DatabaseService()
        {
            this.urepo = new UsersRepository("prueba");
            this.trepo = new TareasRepository("prueba");
        }

        public List<TareaDB> ObtenerTareasByAsignee(ObjectId id)
        {
            List<TareaDB> tareas = null;
            using (MDatabase db = new MDatabase("prueba"))
            {
                tareas = db.GetTareasByAsignee(id);
            }
            return tareas;
        }
        public List<TareaDB> ObtenerTareasByOwner(ObjectId id)
        {
            List<TareaDB> tareas = null;
            using (MDatabase db = new MDatabase("prueba"))
            {
                tareas = db.GetTareasByOwner(id);
            }
            return tareas;
        }
        public List<TareaDB> ObtenerTareasByAsigneeOrOwner(ObjectId id)
        {
            List<TareaDB> tareas = null;
            using (MDatabase db = new MDatabase("prueba"))
            {
                tareas = db.GetTareasByAsigneeOrOwner(id);
            }
            return tareas;
        }


        public List<UsuarioDB> ObtenerUsuarios()
        {
            return this.urepo.GetUsers();
        }

        public UsuarioDB ObtenerUsuariosByName(string nombre)
        {
            return this.urepo.GetUserByName(nombre);
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
            this.trepo.CreateTask(t);
        }

        public UsuarioDB GetUserById(ObjectId id)
        {
            return this.urepo.GetUserById(id);
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