using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Registro.Models;

namespace Registro.Controllers
{
    [Authorize]
    [AuthorizeRole(Role.USER, Role.ADMIN)]
    public class GroupsController : Controller
    {
        // GET: Groups
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult JoinGroup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult JoinGroup(string gname)
        {
            DatabaseService dbservice = new DatabaseService();
            UserProfileSessionData session =
                (UserProfileSessionData)this.Session["UserProfile"];
            var userId = session.UserId;
            dbservice.AgregarMiembroAGrupo(gname, userId);
            return RedirectToAction("Index", "Account");
        }

        public JsonResult ObtenerGrupos(string termino)
        {
            DatabaseService dbservice = new DatabaseService();
            List<string> grupos = dbservice.ObtenerGrupos()
                .Where(
                    s => s.Nombre.ToLower().StartsWith(termino.ToLower())
                ).Select(x => x.Nombre).ToList();
            return Json(grupos, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateGroup(Group g)
        {
            UserProfileSessionData session =
                (UserProfileSessionData)this.Session["UserProfile"];
            g.Creator = session.UserId;
            DatabaseService dbservice = new DatabaseService();
            dbservice.CreateGroup(g);
            this.Session["CurrentGroup"] = g;
            return RedirectToAction("Index", "Account");
        }

        [HttpGet]
        public ActionResult AddTaskToGroup()
        {
            return RedirectToAction("Account", "_AddToGroup");
        }

        [HttpPost]
        public ActionResult AddTaskToGroup(string gname)
        {
            DatabaseService dbservice = new DatabaseService();
            TareaDB task = (TareaDB)this.Session["model"];
            if (task is null)
            {
                return RedirectToAction("Index", "Account");
            }
            Group group = dbservice.AgregarTareaAGrupo(gname, task._id);
            return RedirectToAction("Index", "Account");
        }
    }
}