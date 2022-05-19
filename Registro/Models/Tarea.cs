using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace Registro.Models
{
    public class Tarea
    {
        public ObjectId Owner { set; get; }
        public ObjectId Asignee { set; get; }
        public string Description { set; get; }
        public double TEstimated { set; get; }
        public double TTracked { set; get; }
        public TaskType Type { set; get; }

        public Tarea()
        {
            Owner = ObjectId.Empty;
            Asignee = ObjectId.Empty;
            Description = String.Empty;
            TEstimated = 0.0;
            TTracked = 0.0;
            Type = null;
        }

        public Tarea(ObjectId owner, ObjectId asignee, string desc, double estimated, double tracked, TaskType type)
        {
            Owner = owner;
            Asignee = asignee;
            Description = desc;
            TEstimated = estimated;
            TTracked = tracked;
            Type = type;
        }

        public Tarea(Tarea tc)
        {
            Owner = tc.Owner;
            Asignee = tc.Asignee;
            Description = tc.Description;
            TEstimated = tc.TEstimated;
            TTracked = tc.TTracked;
            Type = tc.Type;
        }

    }

    public class TareaDB : Tarea
    {
        public ObjectId _id { get; set; }
        public TareaDB() : base()
        {
            _id = ObjectId.Empty;
        }
        public TareaDB(ObjectId _id, ObjectId owner, ObjectId asignee, string desc, double estimated, double tracked, TaskType type) : base(owner, asignee, desc, estimated, tracked, type)
        {
            this._id = _id;
        }
        public TareaDB(ObjectId _id, Tarea tcopy) : base(tcopy)
        {
            this._id = _id;
        }
    } 

}