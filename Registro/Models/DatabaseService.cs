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
        // Tareas
        public ObjectId CreateTarea(TareaDB t)
        {
            return this.trepo.Insert(t);
        }
        public TareaDB ActualizarTarea(TareaDB t)
        {
            return this.trepo.Update(t);
        }
        public TareaDB ObtenerTareaById(ObjectId id)
        {
            return trepo.GetById(id);
        }
        public List<TareaDB> ObtenerTareasByAsignee(ObjectId id)
        {
            return trepo.GetByAsignee(id);
        }
        public List<TareaDB> ObtenerTareasByIds(List<ObjectId> ids)
        {
            return trepo.GetManyById(ids);
        }
        public List<TareaDB> ObtenerTareasByOwner(ObjectId id)
        {
            return trepo.GetByOwner(id);
        }
        public List<TareaDB> ObtenerTareasByAsigneeOrOwner(ObjectId id)
        {
            return trepo.GetByAsigneeOrOwner(id);
        }
        // Tipos
        public ObjectId CreateType(TaskType newType)
        {
            return this.tyrepo.Insert(newType);
        }
        public List<TaskType> ObtenerTiposTareas()
        {
            return this.tyrepo.GetAll();
        }
        public TaskType ObtenerTipoByName(string nombre)
        {
            return this.tyrepo.GetByName(nombre);
        }
        // Usuarios
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
        // Grupos
        public ObjectId CreateGroup(Group g)
        {
            return grepo.Insert(g);
        }
        public Group ObtenerGrupoByName(string gname)
        {
            return grepo.GetGroupByName(gname);
        }
        public List<Group> ObtenerGrupos()
        {
            return grepo.GetAll();
        }
        public List<Group> ObtenerGruposByMember(ObjectId id)
        {
            return grepo.GetGroupsByParticipant(id);
        }
        public List<TareaDB> ObtenerTareasFromGroup(string gname)
        {
            Group g = grepo.GetGroupByName(gname);
            if (g.Listas == null)
                return default;
            
            return trepo.GetManyById(g.Listas);
        }
        /// <summary>
        /// Class <c>Point</c> models a point in a two-dimensional plane.
        /// </summary>
        public Group AgregarTareaAGrupo(string gname, ObjectId id)
        {
            return grepo.AddTask(gname, id);
        }
        public Group MoverTareaAGrupo(string gname, ObjectId id)
        {
            return grepo.MoveTask(gname, id);
        }
        public Group AgregarMiembroAGrupo(string gname, ObjectId id)
        {
            return grepo.AddMemberToGroup(gname, id);
        }

        public Group EliminarTareaDeGrupo(string gname, ObjectId id)
        {
            return this.grepo.RemoveTaskFromGroup(gname, id);
        }
    }
}