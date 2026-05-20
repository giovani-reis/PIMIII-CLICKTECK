using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Extensions.Hosting;
using NuGet.Protocol.Plugins;
using PIM_TechTrust.Models;
using PIM_TechTrust.Models.Enums;
using PIMIII_CLICKTECK.Data;
using PIMIII_CLICKTECK.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;

namespace PIMIII_CLICKTECK.Controllers
{
    public class CadastroController : Controller
    {
        private readonly Context _context;
        private readonly IWebHostEnvironment _environment;

        public CadastroController(Context context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Usuarios
/*        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }*/

        // GET: Usuarios/Details/5
/*        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }*/

        // GET: Usuarios/Create
        public IActionResult Cadastro() // Create
        {
            return View();
        }

        public IActionResult Usuario() // Create
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Usuario([Bind("Id,Nome,Email,Senha,ConfirmarSenha,Role")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
                _context.Add(usuario);
                await _context.SaveChangesAsync();

                    
                TempData["SucessoCadastro"] = true;
                return RedirectToAction("Index", "Login");

            }
            return View(usuario);
        }


        public async Task<IActionResult> Tecnico() // Create
        {

            var especialidadesDoBanco = await _context.Especialidades.ToListAsync();
            ViewBag.Especialidades = especialidadesDoBanco;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Tecnico(RegistroTecnicoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Se der erro, recarrega as especialidades para a View não quebrar
                ViewBag.Especialidades = await _context.Especialidades.ToListAsync();
                return View(model);
            }

            // 1. Criar o Usuário Primeiro (é a base de tudo)
            var usuario = new Usuario
            {
                Nome = model.Nome,
                Email = model.Email,
                Senha =  BCrypt.Net.BCrypt.HashPassword(model.Senha),
                Role = Role.Tecnico
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync(); // Gerou o ID do Usuário

            // 2. Tratar o Upload da Foto
            string nomeUnicoArquivo = null;
            if (model.Foto != null)
            {
                string pastaUploads = Path.Combine(_environment.WebRootPath, "fotos_perfil");

                // Garante que a pasta existe
                if (!Directory.Exists(pastaUploads)) Directory.CreateDirectory(pastaUploads);

                nomeUnicoArquivo = Guid.NewGuid().ToString() + "_" + model.Foto.FileName;
                string caminhoCompleto = Path.Combine(pastaUploads, nomeUnicoArquivo);

                using (var fileStream = new FileStream(caminhoCompleto, FileMode.Create))
                {
                    await model.Foto.CopyToAsync(fileStream);
                }
            }

            // 3. Criar o Perfil do Técnico
            var perfil = new TecnicoPerfil
            {
                UsuarioId = usuario.Id,
                Descricao = model.Descricao,
                FotoUrl = nomeUnicoArquivo, // Salvamos apenas o NOME/CAMINHO no banco
                Disponivel = true
            };

            _context.TecnicoPerfis.Add(perfil);
            await _context.SaveChangesAsync(); // Gerou o ID do Perfil

            // 4. Salvar as Especialidades (Muitos-para-Muitos)
            if (model.EspecialidadesIds != null && model.EspecialidadesIds.Any())
            {
                foreach (var espId in model.EspecialidadesIds)
                {
                    _context.TecnicoEspecialidades.Add(new TecnicoEspecialidade
                    {
                        TecnicoPerfilId = perfil.Id,
                        EspecialidadeId = espId
                    });
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Login");
        }



        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
