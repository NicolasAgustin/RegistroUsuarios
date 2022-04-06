using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Registro.Models
{
    public class ResultadoUsuarios
    {
        public List<Usuario> resultado { set; get; }
        public ResultadoUsuarios(List<Usuario> usuarios)
        {
            this.resultado = usuarios;
        }

    }
}