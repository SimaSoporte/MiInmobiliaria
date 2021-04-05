using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MiInmobiliaria.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Controllers
{
    public class PersonaController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly RepositorioPersona repositorio;
        private readonly RepositorioTipoPersona repositorioTipoPersona;

        public PersonaController(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.repositorio = new RepositorioPersona(configuration);
            this.repositorioTipoPersona = new RepositorioTipoPersona(configuration);
        }

        // GET: PersonaController
        public ActionResult Index()
        {
            var lista = repositorio.Listar();
            ViewData[nameof(Persona)] = lista;
            ViewData["Title"] = nameof(Persona);
            ViewData["Error"] = TempData["Error"];
            return View(lista);
        }

        // GET: PersonaController/Details/5
        public ActionResult Details(int id)
        {
            var e = repositorio.Obtener(id);
            return View(e);
        }

        // GET: PersonaController/Create
        public ActionResult Create()
        {
            ViewBag.items = repositorioTipoPersona.ListarSelectListItem();
            return View();
        }

        // POST: PersonaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Persona e)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PersonaController/Edit/5
        public ActionResult Edit(int id)
        {
            //ViewBag.items = repositorioTipoPersona.ListarSelectListItem();
            ViewBag.tipoPersona = repositorioTipoPersona.Listar();
            var e = repositorio.Obtener(id);
            return View(e);
        }

        // POST: PersonaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Persona e)
        {
            try
            {
                repositorio.Editar(e);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PersonaController/Delete/5
        public ActionResult Delete(int id)
        {
            var e = repositorio.Obtener(id);
            return View(e);
        }

        // POST: PersonaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Persona e)
        {
            try
            {
                repositorio.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex)
            {
                TempData["Error"] = ex.Number == 547 ? "No se puede borrar el tipo Persona porque esta utilizado" : "Ocurrio un error.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ocurrio un error." + ex.ToString();
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
