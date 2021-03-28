using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using MiInmobiliaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Controllers
{
    public class PropietarioController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly PropietarioData repositorio;
        private readonly TipoPersonaData repositorioTipoPersona;
        private readonly PersonaData repositorioPersona;

        public PropietarioController(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.repositorio = new PropietarioData(configuration);
            this.repositorioTipoPersona = new TipoPersonaData(configuration);
            this.repositorioPersona = new PersonaData(configuration);
        }
        // GET: PropietarioController
        public ActionResult Index()
        {
            var lista = repositorio.Listar();
            return View(lista);
        }

        // GET: PropietarioController/Details/5
        public ActionResult Details(int id)
        {
            var propietario = repositorio.Obtener(id);
            return View(propietario);
        }

        // GET: PropietarioController/Create
        public ActionResult Create()
        {
            ViewBag.items = repositorioTipoPersona.ListarSelectListItem();
            return View();
        }

        // POST: PropietarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario e)
        {
            Persona p = repositorioPersona.Obtener(e.Persona.Dni, e.Persona.Email);
            if ( p != null )
                e.Persona = p;
            else
            {
                e.Persona.TipoPersona = repositorioTipoPersona.Obtener(e.Persona.TipoPersona.Id);
                e.Persona.Salt = "";
                e.Persona.Formato = "";
                e.Persona.Id = repositorioPersona.Create(e.Persona);
            }

            try
            {
                repositorio.Create(e);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PropietarioController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.items = repositorioTipoPersona.ListarSelectListItem();
            var propietario = repositorio.Obtener(id);
            propietario.Id = id;
            return View(propietario);
        }

        // POST: PropietarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Propietario e)
        {
            try
            {
                e.Persona.Formato = "";
                repositorioPersona.Editar(e.Persona);
                repositorio.Editar(e);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PropietarioController/Delete/5
        public ActionResult Delete(int id)
        {
            var e = repositorio.Obtener(id);
            return View(e);
        }

        // POST: PropietarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Propietario e)
        {
            try
            {
                repositorio.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}