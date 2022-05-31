using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace Registro.Models
{
    public class ListaTareas
    {
        public ObjectId _id { get; set; }
        public List<ObjectId> Tareas { get; set; }

    }
}