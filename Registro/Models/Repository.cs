using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace Registro.Models
{
    public interface Repository<T>
    {
        List<T> GetAll();
        T GetById(ObjectId id);
        ObjectId Insert(T n);
    }
}