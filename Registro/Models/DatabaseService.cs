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
        private TypesRepository tyrepo;
        private GroupsRepository grepo;
        private string dbName;
        public DatabaseService()
        {
            this.dbName = "prueba";
            this.urepo = new UsersRepository(this.dbName);
            this.trepo = new TareasRepository(this.dbName);
            this.tyrepo = new TypesRepository(this.dbName);
            this.grepo = new GroupsRepository(this.dbName);
        }

        public ObjectId CreateType(TaskType newType)
        {
            return this.tyrepo.Insert(newType);
        }
        public List<TareaDB> ObtenerTareasByAsignee(ObjectId id)
        {
            return trepo.GetByAsignee(id);
        }
        public List<TareaDB> ObtenerTareasByOwner(ObjectId id)
        {
            return trepo.GetByOwner(id);
        }
        public List<TareaDB> ObtenerTareasByAsigneeOrOwner(ObjectId id)
        {
            return trepo.GetByAsigneeOrOwner(id);
        }
        public List<TaskType> ObtenerTiposTareas()
        {
            return this.tyrepo.GetAll();
        }
        public List<UsuarioDB> ObtenerUsuarios()
        {
            return this.urepo.GetAll();
        }
        public UsuarioDB ObtenerUsuariosByName(string nombre)
        {
            return this.urepo.GetByName(nombre);
        }
        public UsuarioDB ObtenerUsuarioByEmail(string email)
        {
            return this.urepo.GetByEmail(email);
        }
        public UsuarioDB ObtenerUsuarioByEmailAndPass(string email, string password)
        {
            return this.urepo.GetByEmailAndPass(email, password);
        }
        public void CreateTarea(TareaDB t)
        {
            this.trepo.Insert(t);
        }
        public UsuarioDB GetUserById(ObjectId id)
        {
            return this.urepo.GetById(id);
        }
        public ObjectId AddUser(UsuarioDB u)
        {
            return this.urepo.Insert(u);
        }
        public ObjectId SaveFile(Stream file, string name)
        {
            return ObjectId.Empty;
        }
        public ObjectId CreateGroup(Group g)
        {
            return grepo.Insert(g);
        }

    }
}