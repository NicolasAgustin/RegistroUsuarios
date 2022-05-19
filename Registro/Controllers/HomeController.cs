using Registro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Registro.Controllers
{

    public class HomeController : Controller
    {
        private DatabaseService dbservice;
        public ActionResult Prueba()
        {
            return View("Prueba");
        }
        // Pagina home
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetUser(string nombre)
        {
            List<UsuarioDB> resultado = this.dbservice.ObtenerUsuariosByName(nombre);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            // El tempdata se mantiene hacia la siguiente request, por lo que si no es null, una request de login anterior tuvo lugar
            if (TempData["error"] is null)
            {
                ViewData["error"] = false;
            } else
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
            UsuarioDB auth = this.dbservice.ObtenerUsuarioByEmail(u.Email);
            if (auth is null) {
                TempData["error"] = true;
                return RedirectToAction("Login", "Home");
            }

            if (u.Password == auth.Password)
            {
                TempData["error"] = false;
                this.Session["UserProfile"] = this.CreateSessionData(auth);
                return RedirectToAction("Logged", "Home", auth);
            }

            TempData["error"] = true;
            return RedirectToAction("Login", "Home");

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
        public ActionResult Logged()
        {
            this.dbservice = new DatabaseService();
            UserProfileSessionData session = (UserProfileSessionData)this.Session["UserProfile"];
            this.Session["ProfilePicture"] = "data:image/jpg;base64," + GetUserPicture(session.EmailAddress);
            return View();
        }

        [HttpPost]
        public ActionResult CreateTask(Tarea t)
        {
            this.dbservice = new DatabaseService();
            UserProfileSessionData session = (UserProfileSessionData)this.Session["UserProfile"];
            t.Owner = session.UserId;
            t.Asignee = session.UserId;
            t.TEstimated = 0.0;
            t.TTracked = 0.0;
            t.Type = new TaskType { Title = "task development" };
            this.dbservice.CreateTarea(t);
            return View("Logged");
        }

        public string GetUserPicture(string email)
        {
            this.dbservice = new DatabaseService();
            UsuarioDB user = this.dbservice.ObtenerUsuarioByEmail(email);
            
            if (user == null)
                return null;

            byte[] obtainedPicture = System.IO.File.ReadAllBytes(user.ProfilePictureServerPath);

            return Convert.ToBase64String(obtainedPicture, 0, obtainedPicture.Length);
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
                    DirectoryInfo info = new DirectoryInfo("C:\\Users\\Nico\\Desktop\\Server");
                    if (!info.Exists)
                    {
                        info.Create();
                    }

                    string fileName = u.Photo.FileName;
                    
                    path = Path.Combine("C:\\Users\\Nico\\Desktop\\Server", fileName);
                    
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    u.Photo.SaveAs(path);
                    
                } catch (Exception e)
                {
                }
            }
            
            UsuarioDB nuevo = new UsuarioDB(u.Nombre, u.Apellido, u.Edad, u.Email, u.Password, path);
            
            ObjectId id = this.dbservice.AddUser(nuevo);

            this.Session["UserProfile"] = this.CreateSessionData(nuevo);

            return RedirectToAction("Logged", "Home", nuevo);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        
        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Contact(Usuario u)
        {

            Register(u);

            return View();
        }
    }
}