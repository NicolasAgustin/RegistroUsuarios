using Registro.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Mvc;
using MongoDB.Bson;
using System.Web.Security;

namespace Registro.Controllers
{
    // Con el handleerror le indicamos que maneje todas las excepciones que se generen y no esten controladas
    [HandleError]
    public class AuthController : Controller
    {
        private DatabaseService dbservice;

        [HttpGet]
        public ActionResult Login()
        {
            // El tempdata se mantiene hacia la siguiente request, por lo que si no es null, una request de login anterior tuvo lugar
            if (TempData["error"] is null)
            {
                ViewData["error"] = false;
            }
            else
            {
                // En caso de no ser null, trae un error de un intento de login anterior
                // Cargamos el viewdata para esta view especifica
                ViewData["error"] = TempData["error"];
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(Usuario u)
        {
            this.dbservice = new DatabaseService();

            UsuarioDB auth = this.dbservice.ObtenerUsuarioByEmailAndPass(u.Email, u.Password);

            if (auth is null)
            {
                TempData["error"] = true;
                return RedirectToAction("Login", "Auth");
            }

            FormsAuthentication.SetAuthCookie(auth.Email, false);
            FormsAuthentication.SetAuthCookie(Convert.ToString(auth._id), false);

            string roleStr = "";
            if (auth.Roles is null)
            {
                throw new Exception("El usuario no posee roles.");
            }
            foreach(Role r in auth.Roles)
            {
                roleStr = roleStr + "," + r.ToString();
            }

            var authTicket = new FormsAuthenticationTicket(1, auth.Email, DateTime.Now, 
                                                            DateTime.Now.AddMinutes(20), false, roleStr);

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            HttpContext.Response.Cookies.Add(authCookie);

            TempData["error"] = false;
            this.Session["UserProfile"] = this.CreateSessionData(auth);
            //FormsAuthentication.SetAuthCookie(uAuth.EmailAddress, false);
            return RedirectToAction("Index", "Account", auth);
            
        }
        
        private UserProfileSessionData CreateSessionData(UsuarioDB u)
        {
            UserProfileSessionData sessionData = new UserProfileSessionData
            {
                UserId = u._id,
                EmailAddress = u.Email,
                Name = u.Nombre,
                LastName = u.Apellido
            };

            return sessionData;
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public RedirectToRouteResult Register(Usuario u)
        {

            string path = String.Empty;
            this.dbservice = new DatabaseService();

            if (u.Photo != null)
            {
                try
                {
                    string serverPath = ConfigurationManager.AppSettings["server_path"];
                    DirectoryInfo info = new DirectoryInfo(serverPath);
                    if (!info.Exists)
                    {
                        info.Create();
                    }

                    string fileName = u.Photo.FileName;

                    path = Path.Combine(serverPath, fileName);

                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    u.Photo.SaveAs(path);

                }
                catch (Exception e)
                {
                }
            }

            UsuarioDB nuevo = new UsuarioDB(u.Nombre, u.Apellido, u.Edad, u.Email, u.Password, path, new List<Role> { Role.USER });

            ObjectId id = this.dbservice.AddUser(nuevo);
            UserProfileSessionData uAuth = this.CreateSessionData(nuevo);
            this.Session["UserProfile"] = uAuth;
            FormsAuthentication.SetAuthCookie(nuevo.Email, false);
            FormsAuthentication.SetAuthCookie(Convert.ToString(nuevo._id), false);

            string roleStr = "";

            foreach (Role r in nuevo.Roles)
            {
                roleStr = roleStr + "," + r.ToString();
            }

            var authTicket = new FormsAuthenticationTicket(1, nuevo.Email, DateTime.Now,
                                                            DateTime.Now.AddMinutes(20), false, roleStr);

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            HttpContext.Response.Cookies.Add(authCookie);

            return RedirectToAction("Index", "Account", nuevo);
        }
    }
}
