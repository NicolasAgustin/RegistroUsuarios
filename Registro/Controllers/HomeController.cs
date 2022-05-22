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
        // Pagina home
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }   
    }
}