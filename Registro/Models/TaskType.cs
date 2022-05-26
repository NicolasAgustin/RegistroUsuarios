using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using MongoDB.Bson;

namespace Registro.Models
{
    public class TaskType
    {
        public ObjectId _id { get; set; }
        public ObjectId Creator { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }

        public TaskType()
        {
            _id = ObjectId.Empty;
            Creator = ObjectId.Empty;
            Description = String.Empty;
            Title = String.Empty;
        }

        public TaskType(ObjectId id, ObjectId creator, string desc, string title, Color color)
        {
            _id = id;
            Title = title;
            Creator = creator;
            Description = desc;
        }

        public override string ToString()
        {

            return this.Title;
        }
    }
}