﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MiInmobiliaria.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Controllers
{
    public class AgenciaController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        private readonly RepositorioAgencia repositorio;
        private readonly RepositorioPersona repositorioPersona;
        private readonly RepositorioTipoPersona repositorioTipoPersona;
        private readonly Utils utils;

        public AgenciaController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.repositorio = new RepositorioAgencia(configuration);
            this.repositorioTipoPersona = new RepositorioTipoPersona(configuration);
            this.repositorioPersona = new RepositorioPersona(configuration);
            this.environment = environment;
            this.utils = new Utils(configuration, environment);
        }

        // GET: AgenciaController
        public ActionResult Index()
        {
            var lista = repositorio.getAll();
            return View(lista);
        }



        // GET: AgenciaController/Details/5
        public ActionResult Details(int id)
        {
            var e = repositorio.getById(id);
            return View(e);
        }



        // GET: AgenciaController/Create
        public ActionResult Create()
        {
            ViewBag.items = repositorioTipoPersona.ListarSelectListItem();
            return View();
        }

        // POST: AgenciaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Agencia e)
        {
            Persona p = repositorioPersona.getByDniEmail(e.Persona.Dni, e.Persona.Email);
            if (p != null)
                e.Persona = p;
            else
            {
                e.Persona.TipoPersona = repositorioTipoPersona.Obtener(e.Persona.TipoPersona.Id);
                e.Persona.TipoPersonaId = e.Persona.TipoPersona.Id;
                e.Persona.Password = "";
                //Fuente: https://es.coredump.biz/questions/4538894/get-index-of-a-keyvalue-pair-in-a-c-dictionary-based-on-the-value
                e.Persona.Rol = Persona.ObtenerRoles().First(kvp => kvp.Value.Equals("Agencia")).Key;
                e.Persona.Avatar = "";
                e.Persona.Id = repositorioPersona.Create(e.Persona);
                if (e.Persona.AvatarFile != null)
                {
                    e.Persona.Avatar = utils.uploadFile(e.Persona);
                }
                repositorioPersona.Edit(e.Persona);
            }

            try
            {
                e.Activo = true;
                repositorio.Create(e);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ocurrio un error." + ex.ToString();
                return View();
            }
        }



        // GET: AgenciaController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.TipoPersona = repositorioTipoPersona.Listar();
            var e = repositorio.getById(id);
            return View(e);
        }

        // POST: AgenciaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Agencia e)
        {
            try
            {
                e.Persona.TipoPersona = repositorioTipoPersona.Obtener(e.Persona.TipoPersona.Id);
                e.Persona.TipoPersonaId = e.Persona.TipoPersona.Id;
                e.Persona.Password = "";
                //Fuente: https://es.coredump.biz/questions/4538894/get-index-of-a-keyvalue-pair-in-a-c-dictionary-based-on-the-value
                e.Persona.Rol = Persona.ObtenerRoles().First(kvp => kvp.Value.Equals("Agencia")).Key;
                e.Persona.Avatar = "";
                if (e.Persona.AvatarFile != null)
                {
                    e.Persona.Avatar = utils.uploadFile(e.Persona);
                }
                repositorioPersona.Edit(e.Persona);

                e.Activo = true;
                repositorio.Edit(e);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                TempData["Error"] = "Ocurrio un error." + ex.ToString();
                return View();
            }
        }



        // GET: AgenciaController/Delete/5
        public ActionResult Delete(int id)
        {
            var e = repositorio.getById(id);
            return View(e);
        }

        // POST: AgenciaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Agencia e)
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
