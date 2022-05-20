using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB;
using Microsoft;
using MongoDB.Bson.Serialization.Attributes;

namespace Registro.Models
{
    public class Usuario
    {
        public string Nombre { set; get; }
        public string Apellido { set; get; }
        public string Edad { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        public HttpPostedFileBase Photo { set; get; }

        public Usuario(string nombre, string apellido, string edad, string email, string password, HttpPostedFileBase photo)
        {
            Nombre = nombre;
            Apellido = apellido;
            Edad = edad;
            Email = email;
            Password = password;
            Photo = photo;
        }

        public Usuario()
        {
            Nombre = null;
            Apellido = null;
            Edad = null;
            Email = null;
        }

    }
    [BsonIgnoreExtraElements]
    public class UsuarioDB
    {
        public ObjectId _id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Edad { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfilePictureServerPath { get; set; }
        public List<Role> Roles { get; set; }

        public UsuarioDB(string nombre, string apellido, string edad, string email, string pass, string path, List<Role> roles)
        {
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.Edad = edad;
            this.Email = email;
            this.Password = pass;
            this.ProfilePictureServerPath = path;
            this.Roles = roles;
        }

        public UsuarioDB()
        {
            Nombre = String.Empty;
            Apellido = String.Empty;
            Edad = String.Empty;
            Email = String.Empty;
            Password = String.Empty;
            ProfilePictureServerPath = String.Empty;
            this.Roles = null;
        }
        public List<Role> getRoles()
        {
            return this.Roles;
        }
        public void addRole(Role r)
        {
            if(this.Roles is null)
            {
                this.Roles = new List<Role>();
            }
            this.Roles.Add(r);
        }

    }

    [Serializable]
    public class UserProfileSessionData
    {
        public ObjectId UserId { get; set; }
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}