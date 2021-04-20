using Microsoft.AspNetCore.Hosting;
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
    public class ContratoController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        private readonly RepositorioContrato repositorio;
        private readonly RepositorioInmueble repositorioInmueble;
        private readonly RepositorioInquilino repositorioInquilino;
        private readonly RepositorioGarante repositorioGarante;

        public ContratoController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.repositorio = new RepositorioContrato(configuration);

            this.repositorioInmueble = new RepositorioInmueble(configuration);
            this.repositorioInquilino = new RepositorioInquilino(configuration);
            this.repositorioGarante = new RepositorioGarante(configuration);
        }

        // GET: ContratoController
        public ActionResult Index()
        {
            var lista = repositorio.getAll();
            return View(lista);
        }



        // GET: ContratoController/Details/5
        public ActionResult Details(int id)
        {
            var e = repositorio.getById(id);
            return View(e);
        }



        // GET: ContratoController/Create
        public ActionResult Create()
        {
            ViewBag.Inmuebles = repositorioInmueble.getAll();
            ViewBag.Inquilinos = repositorioInquilino.getAll();
            ViewBag.Garantes = repositorioGarante.getAll();
            return View();
        }

        // POST: ContratoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato e)
        {
            try
            {
                e.Precio = repositorioInmueble.getById(e.InmuebleId).Precio;
                e.Id = repositorio.Create(e);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ocurrio un error." + ex.ToString();
                return View();
            }
        }



        // GET: ContratoController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Inmuebles = repositorioInmueble.getAll();
            ViewBag.Inquilinos = repositorioInquilino.getAll();
            ViewBag.Garantes = repositorioGarante.getAll();
            var e = repositorio.getById(id);
            return View(e);
        }

        // POST: ContratoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: ContratoController/Delete/5
        public ActionResult Delete(int id)
        {
            var e = repositorio.getById(id);
            return View(e);
        }

        // POST: ContratoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Contrato e)
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


        // GET: ContratoController
        public ActionResult Vigentes()
        {
            var lista = repositorio.getAllVigentes();
            return View("Index",lista);
        }

        // GET: ContratoController
        public ActionResult getByInmueble(int Id)
        {
            var lista = repositorio.getByInmueble(Id);
            return View("Index", lista);
        }

    }
}
