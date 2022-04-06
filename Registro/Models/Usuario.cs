using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB;

namespace Registro.Models
{
    public class Usuario
    {
        public string Nombre { set; get; }
        public string Apellido { set; get; }
        public string Edad { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        public string ProfilePictureId { set; get; }
        //public string ProfilePicturePath { set; get; }
        

        public Usuario(string nombre, string apellido, string edad, string email, string password, string picid)
        {
            Nombre = nombre;
            Apellido = apellido;
            Edad = edad;
            Email = email;
            Password = password;
            ProfilePictureId = picid;
        }

        public Usuario()
        {
            Nombre = null;
            Apellido = null;
            Edad = null;
            Email = null;
        }

    }
}