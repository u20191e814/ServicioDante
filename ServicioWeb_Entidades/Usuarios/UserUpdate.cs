using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWeb_Entidades.Usuarios
{
    public  class UserUpdate
    {
        public int Pk_user { get; set; }
        public bool Habilitado { get; set; }
        public bool Administrador { get; set; }
        public bool Tecnico { get; set; } 
    }
}
