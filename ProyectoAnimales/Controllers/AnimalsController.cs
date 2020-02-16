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
    public class AnimalsController : BaseController
    {
        [HttpGet]
        public Reply Get(SecurityViewModel model)
        {
            Reply oR = new Reply();
            oR.Result = 0;

            if (!Verify(model.Token))
            {
                oR.Message = "No tienes permiso, logueate nuevamente";
                return oR;
            }

            try
            {
                using (AnimalesContext db = new AnimalesContext() )
                {
                    var list = (from anim in db.Animalito
                                where anim.IdEstado.Equals(1)
                                select new
                                {
                                    id = anim.Id,
                                    nombre = anim.Nombre,
                                    patas = anim.Patas

                                }).ToList();

                    if(list.Count > 0)
                    {
                        oR.Data = list;
                        oR.Count = list.Count();
                        oR.Message = "Se encontraron datos";
                        oR.Result = 1;
                    }
                    else
                    {
                        oR.Message = "No se encontraron datos";
                    }
                }

            }
            catch (Exception ex)
            {
                oR.Message = "Ha ocurrido un error al traer el listado. " + ex;
            }

            return oR;

        }


        [HttpPost]
        public Reply Add([FromBody]Animalito model)
        {
            Reply oR = new Reply();
            oR.Result = 0;

            try
            {
                Animalito ani = new Animalito();
                ani.Nombre = model.Nombre;
                ani.Patas = model.Patas;
                ani.IdEstado = model.IdEstado;

                using (AnimalesContext db = new AnimalesContext())
                {
                    db.Animalito.Add(ani);
                    db.SaveChanges();
                }

                oR.Data = ani;
                oR.Message = "Se ha guardado correctamente";
                oR.Result = 1;
                oR.Count = 1;

            }
            catch (Exception ex)
            {
                oR.Message = "Se ha producido un error al guardar" + ex;
            }

            return oR;

        }


        [HttpPost]
        public Reply Edit([FromBody] AnimalitoViewModel model)
        {
            Reply oR = new Reply();
            oR.Result = 0;

            if (!Verify(model.Token))
            {
                oR.Message = "No tienes permiso, logueate nuevamente";
                return oR;
            }

            try
            {
                using (AnimalesContext db = new AnimalesContext())
                {
                    Animalito ani = db.Animalito.Find(model.Id);
                    ani.Nombre = model.Nombre;
                    ani.Patas = model.Patas;
                    ani.IdEstado = model.IdEstado;

                    db.Entry(ani).State = EntityState.Modified;
                    db.SaveChanges();

                }

                oR.Data = model;
                oR.Message = "Se ha editado correctamente";
                oR.Result = 1;
                oR.Count = 1;

            }
            catch (Exception ex)
            {
                oR.Message = "Se ha producido un error al editar" + ex;
            }

            return oR;

        }


        [HttpDelete("{id}")]
        public Reply Delete(int id)
        {
            Reply oR = new Reply();
            oR.Result = 0;

            try
            {
                using (AnimalesContext db = new AnimalesContext())
                {
                    Animalito ani = db.Animalito.Find(id);

                    if (ani != null)
                    {
                        if (ani.IdEstado == 2)
                        {
                            oR.Message = "Ese animalito ya fue eliminado ¬¬";
                        }

                        if (ani.IdEstado == 1)
                        {
                            ani.IdEstado = 2;
                            db.Entry(ani).State = EntityState.Modified;
                            db.SaveChanges();

                            oR.Data = ani;
                            oR.Message = "Se ha eliminado correctamente :)";
                            oR.Result = 1;
                            oR.Count = 1;
                        }
                    }
                    else
                    {
                        oR.Message = "No existe ese animalito, animalito! >:(";
                    }


                }

            }
            catch (Exception ex)
            {
                oR.Message = "Se ha producido un error al eliminar" + ex;
            }

            return oR;

        }


    }
}