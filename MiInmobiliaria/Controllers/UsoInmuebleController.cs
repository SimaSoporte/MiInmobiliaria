using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MiInmobiliaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Controllers
{
    public class UsoInmuebleController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly RepositorioUsoInmueble repositorio;

        public UsoInmuebleController(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.repositorio = new RepositorioUsoInmueble(configuration);
        }

        // GET: UsoInmuebleController
        public ActionResult Index()
        {
            var lista = repositorio.Listar();
            ViewData[nameof(UsoInmueble)] = lista;
            ViewData["Title"] = nameof(UsoInmueble);
            return View(lista);
        }

        // GET: UsoInmuebleController/Details/5
        public ActionResult Details(int id)
        {
            var e = repositorio.Obtener(id);
            //ViewData[nameof(UsoInmueble)] = e;
            return View(e);
        }

        // GET: UsoInmuebleController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsoInmuebleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UsoInmueble e)
        {
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

        // GET: UsoInmuebleController/Edit/5
        public ActionResult Edit(int id)
        {
            var e = repositorio.Obtener(id);
            return View(e);
        }

        // POST: UsoInmuebleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UsoInmueble e)
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

        // GET: UsoInmuebleController/Delete/5
        public ActionResult Delete(int id)
        {
            var e = repositorio.Obtener(id);
            return View(e);
        }

        // POST: UsoInmuebleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, UsoInmueble e)
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
