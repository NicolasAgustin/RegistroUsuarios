using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Registro.Models;
using System.IO;
using MongoDB.Bson;
using System.Web.Security;

namespace Registro.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        [HttpGet]
        [Authorize]
        [AuthorizeRole(Role.USER, Role.ADMIN)]
        public ActionResult Index()
        {
            DatabaseService dbservice = new DatabaseService();
            UserProfileSessionData session = (UserProfileSessionData)this.Session["UserProfile"];
            if (session is null)
            {
                throw new Exception("Sesion nula.");
            }
            this.Session["ProfilePicture"] = "data:image/jpg;base64," + GetUserPicture(session.EmailAddress);

            IEqualityComparer<UsuarioDB> customComparer =
                   new PropertyComparer<UsuarioDB>("_id");

            List<TareaDB> tareas = dbservice.ObtenerTareasByAsigneeOrOwner(session.UserId);
            //List<TareaDB> tareasOwner = dbservice.ObtenerTareasByOwner(session.UserId);
            //List<TareaDB> joinedTasksList = tareas.Concat(tareasOwner).ToList();
            this.Session["Tasks"] = tareas;//joinedTasksList.GroupBy(doc => doc._id).Select(x => x.First()).ToList();
            return View(tareas);
        }
        [HttpGet]
        public ActionResult TaskDetails(int index)
        {

            List<TareaDB> tareas = (List<TareaDB>)this.Session["Tasks"];
            if (tareas is null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                TareaDB tareaDisplay = tareas[index];
                return PartialView("_TaskDetails", tareaDisplay);
            }
            catch(Exception e)
            {
                return RedirectToAction("Index");
            }
        }
        public string GetNameById(string id)
        {
            DatabaseService dbservice = new DatabaseService();
            ObjectId nid = ObjectId.Parse(id);
            UsuarioDB usr = dbservice.GetUserById(nid);
            if (usr == null)
            {
                return String.Empty;
            }

            return usr.Nombre + " " + usr.Apellido;

        }
        public string GetUserPicture(string email)
        {
            DatabaseService dbservice = new DatabaseService();
            UsuarioDB user = dbservice.ObtenerUsuarioByEmail(email);

            if (user == null)
                return null;

            byte[] obtainedPicture = System.IO.File.ReadAllBytes(user.ProfilePictureServerPath);

            return Convert.ToBase64String(obtainedPicture, 0, obtainedPicture.Length);
        }
        [HttpGet]
        [Authorize]
        [AuthorizeRole(Role.USER, Role.ADMIN)]
        public ActionResult CreateTask()
        {
            DatabaseService dbservice = new DatabaseService();
            List<UsuarioDB> users = dbservice.ObtenerUsuarios();
            List<String> asigneesNames = new List<String>();
            foreach (UsuarioDB usr in users)
            {
                asigneesNames.Add(usr.Nombre + " " + usr.Apellido);
            }
            ViewBag.data = asigneesNames;
            return PartialView("_CreateTask");
        }
        [Authorize]
        [AuthorizeRole(Role.USER, Role.ADMIN)]
        [HttpPost]
        public ActionResult CreateTask(TareaForm t)
        { 
            DatabaseService dbservice = new DatabaseService();
            UserProfileSessionData session = (UserProfileSessionData)this.Session["UserProfile"];

            Tarea newTarea = new Tarea();

            newTarea.Owner = session.UserId;
            UsuarioDB user = dbservice.ObtenerUsuariosByName(t.Asignee);
            newTarea.Asignee = user._id;
            newTarea.TEstimated = t.TEstimated;
            newTarea.TTracked = 0.0;
            newTarea.Description = t.Description;
            newTarea.Title = t.Title;
            //t.Type = new TaskType { Title = t.Type };
            dbservice.CreateTarea(newTarea);
            return RedirectToAction("Index");
        }
        [Authorize]
        [AuthorizeRole(Role.USER, Role.ADMIN)]
        public List<UsuarioDB> GetUsuarios()
        {
            DatabaseService dbservice = new DatabaseService();
            return dbservice.ObtenerUsuarios();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Auth");
        }
    }
}