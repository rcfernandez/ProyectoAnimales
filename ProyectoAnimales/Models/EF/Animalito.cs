using System;
using System.Collections.Generic;

namespace ProyectoAnimales.Models.EF
{
    public partial class Animalito
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Patas { get; set; }
        public int IdEstado { get; set; }
    }
}
