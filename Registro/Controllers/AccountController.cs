﻿using System;
using MongoDB.Bson;
using System.Web.Mvc;
using Registro.Models;
using System.Web.Security;
using System.Globalization;
using System.Collections.Generic;

namespace Registro.Controllers
{
    [Authorize]
    [AuthorizeRole(Role.USER, Role.ADMIN)]
    public class AccountController : Controller
    {
        // GET: Account
        [HttpGet]
        public ActionResult Index()
        {
            DatabaseService dbservice = new DatabaseService();

            UserProfileSessionData session = 
                (UserProfileSessionData)this.Session["UserProfile"];

            //dbservice.CreateGroup(new Group { Nombre = "Grupo1", Creator = session.UserId });

            if (session is null)
                throw new Exception("Sesion nula.");

            if (this.Session["ProfilePicture"] is null)
            {
                this.Session["ProfilePicture"] = "data:image/jpg;base64,"
                                     + GetUserPicture(session.EmailAddress);
            }

            List<TareaDB> tareas = dbservice
                                   .ObtenerTareasByAsigneeOrOwner(session.UserId);

            List<Group> grupos = dbservice.ObtenerGrupos();

            this.Session["Grupos"] = grupos;
            this.Session["Tasks"] = tareas;

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

            byte[] obtainedPicture = System.IO.File.ReadAllBytes(
                user.ProfilePictureServerPath
            );

            return Convert.ToBase64String(obtainedPicture,
                                          0,
                                          obtainedPicture.Length);
        }
        [HttpGet]
        public ActionResult CreateTask()
        {
            DatabaseService dbservice = new DatabaseService();
            List<TaskType> tiposDisponibles = dbservice.ObtenerTiposTareas();
            List<UsuarioDB> users = dbservice.ObtenerUsuarios();
            List<string> asigneesNames = new List<string>();
            List<string> types = new List<string>();
            foreach (UsuarioDB usr in users)
            {
                asigneesNames.Add(usr.Nombre + " " + usr.Apellido);
            }

            foreach (TaskType t in tiposDisponibles)
            {
                types.Add(t.Title);
            }

            ViewBag.data = asigneesNames;
            ViewBag.tipos = types;
            return PartialView("_CreateTask");
        }
        
        [HttpPost]
        public ActionResult CreateTask(TareaForm t)
        { 
            DatabaseService dbservice = new DatabaseService();
            
            UserProfileSessionData session = 
                (UserProfileSessionData)this.Session["UserProfile"];

            TareaDB newTarea = new TareaDB();

            newTarea.Owner = session.UserId;
            UsuarioDB user = dbservice.ObtenerUsuariosByName(t.Asignee);
            newTarea.Asignee = user._id;
            newTarea.TEstimated = TimeSpan.ParseExact(t.TEstimated,
                                                      @"hh\:mm",
                                                      CultureInfo.InvariantCulture);
            newTarea.TTracked = TimeSpan.ParseExact(t.TTracked,
                                                      @"hh\:mm",
                                                      CultureInfo.InvariantCulture);
            newTarea.Description = t.Description;
            newTarea.Title = t.Title;
            newTarea.Type = dbservice.ObtenerTipoByName(t.Title);
            ObjectId nid = dbservice.CreateTarea(newTarea);
            dbservice.AgregarTareaAGrupo("Grupo1", nid);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ShowTasksInGroup(string name)
        {
            DatabaseService dbservice = new DatabaseService();
            Group found = dbservice.ObtenerGrupoByName(name);
            List<ObjectId> tareasIds = found.Listas;
            if (tareasIds is null)
            {
                return View("Index");
            }

            List<TareaDB> tareas = new List<TareaDB>();
            foreach(var tid in tareasIds)
            {
                TareaDB t = dbservice.ObtenerTareaById(tid);
                tareas.Add(t);
            }

            this.Session["Tasks"] = tareas;

            return View("Index", tareas);

        }

        public List<TaskType> GetTaskTypes()
        {
            DatabaseService dbservice = new DatabaseService();
            return dbservice.ObtenerTiposTareas();
        }

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