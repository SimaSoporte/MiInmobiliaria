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
    public class InquilinoController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly RepositorioInquilino repositorio;
        private readonly RepositorioTipoPersona repositorioTipoPersona;
        private readonly RepositorioPersona repositorioPersona;

        public InquilinoController(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.repositorio = new RepositorioInquilino(configuration);
            this.repositorioTipoPersona = new RepositorioTipoPersona(configuration);
            this.repositorioPersona = new RepositorioPersona(configuration);
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
            var e = repositorio.Obtener(id);
            return View(e);
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
        public ActionResult Create(Inquilino e)
        {
            Persona p = repositorioPersona.Obtener(e.Persona.Dni, e.Persona.Email);
            if (p != null)
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
            var e = repositorio.Obtener(id);
            e.Id = id;
            return View(e);
        }

        // POST: PropietarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inquilino e)
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
        public ActionResult Delete(int id, Inquilino e)
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