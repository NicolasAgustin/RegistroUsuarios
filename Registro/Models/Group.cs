using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Registro.Models
{
    public class Group
    {
        public ObjectId _id { get; set; }
        // El creador es editor y no puede ser eliminado del grupo por nadie
        public ObjectId Creator { get; set; }
        // Usuarios que van a poder agregar mas usuarios al grupo, cambiar el nombre etc
        public List<ObjectId> Editors { get; set; }
        // Miembros normales que no van a poder editar el grupo
        public List<ObjectId> Members { get; set; }
        public string Nombre { get; set; }
        // Listas (folders) donde van a estar las tareas que se creen
        public List<ObjectId> Listas { get; set; }

        public Group()
        {
            _id = ObjectId.Empty;
            Creator = ObjectId.Empty;
            Editors = null;
            Members = null;
            Nombre = string.Empty;
            Listas = null;
        }

        public Group(ObjectId _id, ObjectId creator, List<ObjectId> editors, List<ObjectId> members, string nombre, List<ObjectId> listas)
        {
            this._id = _id;
            Creator = creator;
            Editors = editors;
            Members = members;
            Nombre = nombre;
            Listas = listas;
        }

    }
}