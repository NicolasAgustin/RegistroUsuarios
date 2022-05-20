using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Registro.Models
{
    public class UsersRepository
    {
        private MDatabase db;
        private string dbName;
        public UsersRepository(string dbname)
        {
            this.dbName = dbname;
        }
        public UsuarioDB GetUserDetails(string email, string password)
        {
            UsuarioDB user = new UsuarioDB();
            using (MDatabase db = new MDatabase(this.dbName))
            {
                user = db.GetUserByEmailAndPass(email, password);
            }
            return user;
        }
        public UsuarioDB GetUserByEmail(string email)
        {
            UsuarioDB user = new UsuarioDB();
            using (MDatabase db = new MDatabase(this.dbName))
            {
                user = db.GetUserByEmail(email);
            }
            return user;
        }
        public byte[] GetProfilePhoto(ObjectId id)
        {
            byte[] photo = null;
            using (MDatabase db = new MDatabase(this.dbName))
            {
                photo = db.GetProfilePicture(id);
            }
            return photo;
        }
        public ObjectId addUser(UsuarioDB u)
        {
            using (MDatabase db = new MDatabase(this.dbName))
            {
                return db.AddUser(u);
            }
        }
    }
}