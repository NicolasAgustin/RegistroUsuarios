using Registro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Registro.Controllers
{

    /*
     ViewData y ViewBag me sirve para enviar informacion a la vista que no tenga que ver con el modelo que se esta utilizando
     Una vista si o si necesita un accion
    El _layoutpage es una pagina index, en la carpeta shared que contiene recursos compartidos que puede utilizar cualquier vista
     */
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
            List<Usuario> resultado = this.dbservice.ObtenerUsuariosByName(nombre);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Logged(Usuario u)
        {

            return View(u);
        }

        public string GetPicture(string id)
        {
            ObjectId parsedId = ObjectId.Parse(id);
            this.dbservice = new DatabaseService();
            byte[] obtainedPicture = this.dbservice.GetProfilePicture(parsedId);
            return Convert.ToBase64String(obtainedPicture);
            //return File(obtainedPicture, "image/jpg");
        }

        [HttpPost]
        public RedirectToRouteResult Register(Usuario u)
        {
            //if (!ModelState.IsValid)
            //    return View();

            Usuario nuevo = new Usuario("Nicolas", "Farum", "02/02/2022", "cacaculo@gmail.com", "mipass2", "");

            this.dbservice = new DatabaseService();

            Stream fileUploaded = this.Request.InputStream;
            
            if (fileUploaded != null)
            {
                try
                {
                    string fileName = this.Request.Params.Get("file");
                    nuevo.ProfilePictureId = this.dbservice.SaveFile(fileUploaded, fileName).ToString();
                } catch (Exception e)
                {
                    nuevo.ProfilePictureId = MongoDB.Bson.ObjectId.Empty.ToString();
                }
            }
               
            this.dbservice.AddUser(nuevo);

            return RedirectToAction("Logged", "Home", nuevo);
        }



        //public ActionResult ListUsers()
        //{
        //    this.db = new MDatabase("prueba");
        //    List<Usuario> usuarios = this.db.ObtenerUsuarios();

        //    Console.WriteLine("Obtenidos los usuarios");

        //    return View(new ResultadoUsuarios(usuarios));
        //}

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