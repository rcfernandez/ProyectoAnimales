using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoAnimales.Models;
using ProyectoAnimales.Models.EF;

namespace ProyectoAnimales.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccessController : ControllerBase
    {

        [HttpPost]
        public Reply Login([FromBody] AccessViewModel model)
        {
            Reply oR = new Reply();
            oR.Result = 0;

            try
            {
                using (AnimalesContext db = new AnimalesContext())
                {
                    oR.Result = 1;
                    var list = db.Usuario.Where(u => u.Email == model.Email && u.Pass == model.Pass && u.IdEstado == 1);

                    if (list.Count() > 0)
                    {
                        oR.Data = Guid.NewGuid().ToString();
                        
                        Usuario oUsuario = list.First();
                        oUsuario.Token = (string)oR.Data;
                        oUsuario.FechaExpiracion = DateTime.Now.AddSeconds(240);

                        db.Entry(oUsuario).State = EntityState.Modified;
                        db.SaveChanges();

                        oR.Count = 1;
                        oR.Message = "Se ha generado el token correctamente";
                    }

                    else
                    {
                        oR.Message = "Usuario no valido";
                    }

                }

            }
            catch (Exception ex)
            {
                oR.Message = "Login fallo. " + ex.Message;
            }

            return oR;

        }




    }
}