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
    public class InmuebleController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        private readonly RepositorioInmueble repositorio;
        private readonly IRepositorioPropietario repositorioPropietario;
        private readonly IRepositorioAgencia repositorioAgencia;
        private readonly IRepositorioUsoInmueble repositorioUsoInmueble;
        private readonly IRepositorioTipoInmueble repositorioTipoInmueble;
        private readonly Utils utils;

        public InmuebleController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.repositorio = new RepositorioInmueble(configuration);

            this.repositorioPropietario = new RepositorioPropietario(configuration);
            this.repositorioAgencia = new RepositorioAgencia(configuration);
            this.repositorioUsoInmueble = new RepositorioUsoInmueble(configuration);
            this.repositorioTipoInmueble = new RepositorioTipoInmueble(configuration);
            this.utils = new Utils(configuration, environment);
        }

        // GET: InmuebleController
        public ActionResult Index()
        {
            var lista = repositorio.getAll();
            return View(lista);
        }

        public ActionResult getByPropietario(int id)
        {
            var list = repositorio.getAll(id);
            return View("Index", list);
        }

        public ActionResult getByDisponible()
        {
            var list = repositorio.getAllDisponible();
            return View("Index", list);
        }

        // GET: InmuebleController/Details/5
        public ActionResult Details(int id)
        {
            var e = repositorio.getById(id);
            return View(e);
        }



        // GET: InmuebleController/Create
        public ActionResult Create()
        {
            ViewBag.Propietarios = repositorioPropietario.getAll();
            ViewBag.Agencias = repositorioAgencia.getAll();
            ViewBag.Usos = repositorioUsoInmueble.getAll();
            ViewBag.Tipos = repositorioTipoInmueble.getAll();
            return View();
        }

        // POST: InmuebleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble e)
        {
            try
            {
                e.Avatar = "";
                e.Id = repositorio.Create(e);
                if (e.AvatarFile != null)
                {
                    e.Avatar = utils.uploadFile(e);
                }
                repositorio.Edit(e);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ocurrio un error." + ex.ToString();
                return View();
            }
        }



        // GET: InmuebleController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Propietarios = repositorioPropietario.getAll();
            ViewBag.Agencias = repositorioAgencia.getAll();
            ViewBag.Usos = repositorioUsoInmueble.getAll();
            ViewBag.Tipos = repositorioTipoInmueble.getAll();
            var e = repositorio.getById(id);
            return View(e);
        }

        // POST: InmuebleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inmueble e)
        {
            try
            {
                e.Avatar = "";
                if (e.AvatarFile != null)
                {
                    e.Avatar = utils.uploadFile(e);
                }
                repositorio.Edit(e);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ocurrio un error." + ex.ToString();
                return View();
            }
        }



        // GET: InmuebleController/Delete/5
        public ActionResult Delete(int id)
        {
            var e = repositorio.getById(id);
            return View(e);
        }

        // POST: InmuebleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Inmueble e)
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
