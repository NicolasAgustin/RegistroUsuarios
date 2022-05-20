using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace Registro.Models
{
    public class AuthFilter : FilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if (string.IsNullOrEmpty(Convert.ToString(filterContext.HttpContext.Session["UserID"])))
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "Error"
                };
            }
        }
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
        }
    }
}