using System.Web;
using System.Web.Mvc;

using Registro.Models;

namespace Registro
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            // Agregamos el filtro para logear en un txt de forma global
            filters.Add(new LoggingFilter());
        }
    }
}
