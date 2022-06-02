using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Registro.Models;

namespace Registro.Controllers
{
    public class GroupsController : Controller
    {
        // GET: Groups
        [HttpGet]
        [Authorize]
        [AuthorizeRole(Role.USER, Role.ADMIN)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        [AuthorizeRole(Role.USER, Role.ADMIN)]
        public ActionResult JoinGroup()
        {
            return View();
        }
    }
}