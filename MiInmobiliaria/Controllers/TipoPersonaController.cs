﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MiInmobiliaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Controllers
{
    public class TipoPersonaController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly RepositorioTipoPersona repositorio;

        public TipoPersonaController(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.repositorio = new RepositorioTipoPersona(configuration);
        }

        // GET: TipoPersonaController
        public ActionResult Index()
        {
            var lista = repositorio.Listar();
            ViewData[nameof(TipoPersona)] = lista;
            ViewData["Title"] = nameof(TipoPersona);
            return View(lista);
        }

        // GET: TipoPersonaController/Details/5
        public ActionResult Details(int id)
        {
            var e = repositorio.Obtener(id);
            //ViewData[nameof(TipoPersona)] = e;
            return View(e);
        }

        // GET: TipoPersonaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoPersonaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TipoPersona e)
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

        // GET: TipoPersonaController/Edit/5
        public ActionResult Edit(int id)
        {
            var e = repositorio.Obtener(id);
            return View(e);
        }

        // POST: TipoPersonaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TipoPersona e)
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

        // GET: TipoPersonaController/Delete/5
        public ActionResult Delete(int id)
        {
            var e = repositorio.Obtener(id);
            return View(e);
        }

        // POST: TipoPersonaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, TipoPersona e)
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
