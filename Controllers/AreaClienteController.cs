using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PIMIII_CLICKTECK.Data;
using PIM_TechTrust.Models;
using PIMIII_CLICKTECK.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace PIMIII_CLICKTECK.Controllers
{

    public class AreaClienteController : Controller
    {
        private readonly Context _context;

        public AreaClienteController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var usuLogado = TempData["Usu"];
            if (usuLogado == null)
            {
                return RedirectToAction("Index", "Login");
            }
            TempData.Keep("Usu");

            int idUsuario = (int)usuLogado;


            var tecnicos = _context.TecnicoPerfis
                .Include(t => t.Usuario)
                .Where(t => t.Disponivel)
                .Select(t => new DashBoardViewModel
                {
                    Id = t.Id,
                    IdTecnico = t.UsuarioId,
                    Nome = t.Usuario.Nome,
                    FotoUrl = t.FotoUrl ?? "/img/default-avatar.jpg",
                    Descricao = t.Descricao,

                    // 1. Média de Avaliações (Trata nulos se não houver notas)
                    Avaliacao = _context.Avaliacoes
                        .Where(a => a.TecnicoId == t.Id)
                        .Average(a => (double?)a.Nota) ?? 0.0,

                    // 2. Contagem de Reparos (Retorna 0 se não houver correspondência)
                    QtdReparos = _context.Atendimentos
                        .Count(a => a.TecnicoId == t.UsuarioId && a.Status == "Finalizado"),

                    // 3. Pegando as Especialidades Reais da sua tabela de ligação
                    Tags = _context.TecnicoEspecialidades
                        .Where(te => te.TecnicoPerfilId == t.Id)
                        .Select(te => te.Especialidade.Nome) // Nome vem da tabela ESPECIALIDADES
                        .ToList()
                }).ToList();

            var telaUsuario = new TelaUsuarioViewModel
            {
                Tecnico = tecnicos,
                Usuario = _context.Usuarios.Find(idUsuario)
            };

            return View(telaUsuario);
        }

        [HttpPost]
        public IActionResult CriarAtendimento(Atendimento atendimento)
        {
            TempData.Keep("Usu");
            // O Entity Framework preenche o objeto 'atendimento' com os nomes dos inputs do seu Modal

            // 1. Forçamos os dados que o usuário não preenche manualmente
            atendimento.DataAbertura = DateTime.Now;
            atendimento.Status = "Solicitado";

            // 2. Salva no banco
            _context.Atendimentos.Add(atendimento);
            _context.SaveChanges();

            // 3. Cria uma mensagem para exibir na volta
            TempData["MensagemSucesso"] = "Solicitação de reparo enviada com sucesso!";

            // 4. Redireciona de volta para a vitrine de técnicos
            return RedirectToAction("Index");
        }

        /*

        // GET: AreaCliente
        public async Task<IActionResult> Index()
        {
            var context = _context.TecnicoPerfis.Include(t => t.Usuario);
            return View(await context.ToListAsync());
        }

        // GET: AreaCliente/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tecnicoPerfil = await _context.TecnicoPerfis
                .Include(t => t.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tecnicoPerfil == null)
            {
                return NotFound();
            }

            return View(tecnicoPerfil);
        }

        // GET: AreaCliente/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id");
            return View();
        }

        // POST: AreaCliente/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UsuarioId,Descricao,FotoUrl,Disponivel")] TecnicoPerfil tecnicoPerfil)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tecnicoPerfil);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", tecnicoPerfil.UsuarioId);
            return View(tecnicoPerfil);
        }

        // GET: AreaCliente/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tecnicoPerfil = await _context.TecnicoPerfis.FindAsync(id);
            if (tecnicoPerfil == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", tecnicoPerfil.UsuarioId);
            return View(tecnicoPerfil);
        }

        // POST: AreaCliente/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UsuarioId,Descricao,FotoUrl,Disponivel")] TecnicoPerfil tecnicoPerfil)
        {
            if (id != tecnicoPerfil.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tecnicoPerfil);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TecnicoPerfilExists(tecnicoPerfil.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", tecnicoPerfil.UsuarioId);
            return View(tecnicoPerfil);
        }

        // GET: AreaCliente/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tecnicoPerfil = await _context.TecnicoPerfis
                .Include(t => t.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tecnicoPerfil == null)
            {
                return NotFound();
            }

            return View(tecnicoPerfil);
        }

        // POST: AreaCliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tecnicoPerfil = await _context.TecnicoPerfis.FindAsync(id);
            if (tecnicoPerfil != null)
            {
                _context.TecnicoPerfis.Remove(tecnicoPerfil);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TecnicoPerfilExists(int id)
        {
            return _context.TecnicoPerfis.Any(e => e.Id == id);
        }

        */
    }
}
