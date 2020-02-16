using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoAnimales.Models.EF
{
    public class AnimalitoViewModel : SecurityViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Patas { get; set; }
        public int IdEstado { get; set; }

    }
}
