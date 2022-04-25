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
        public ActionResult Logged(UsuarioDB u)
        {
            ViewBag.Image = "data:image/jpg;base64," + GetUserPicture(u.Email);
            return View(u);
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
            this.dbservice.AddUser(nuevo);

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