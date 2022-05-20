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
    }
}