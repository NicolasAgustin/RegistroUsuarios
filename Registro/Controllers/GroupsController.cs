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
        
    }
}