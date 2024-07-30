using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class Alumno
    {
        public int codigo {get; set;}
        public string nombres {get; set;}
        public string carrera {get; set;}
        public string domicilio {get; set;}
    }
}

// codigo INT PRIMARY KEY,
// nombres NVARCHAR(100),
// carrera NVARCHAR(100),
// domicilio NVARCHAR(200)