using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PIM_TechTrust.Models;
using PIMIII_CLICKTECK.Data;

namespace PIMIII_CLICKTECK.Controllers
{
    public class AreaTecnicoController : Controller
    {
        private readonly Context _context;

        public AreaTecnicoController(Context context)
        {
            _context = context;
        }

        // GET: AreaTecnico
        public async Task<IActionResult> Index()
        {
            var context = _context.Atendimentos.Include(a => a.Cliente).Include(a => a.Tecnico);
            return View(await context.ToListAsync());
        }

        // GET: AreaTecnico/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var atendimento = await _context.Atendimentos
                .Include(a => a.Cliente)
                .Include(a => a.Tecnico)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (atendimento == null)
            {
                return NotFound();
            }

            return View(atendimento);
        }

        // GET: AreaTecnico/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Usuarios, "Id", "Id");
            ViewData["TecnicoId"] = new SelectList(_context.Usuarios, "Id", "Id");
            return View();
        }

        // POST: AreaTecnico/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClienteId,TecnicoId,ValorOrcamento,ObservacaoTecnico,Descricao,Status,DataAbertura,DataConclusao")] Atendimento atendimento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(atendimento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Usuarios, "Id", "Id", atendimento.ClienteId);
            ViewData["TecnicoId"] = new SelectList(_context.Usuarios, "Id", "Id", atendimento.TecnicoId);
            return View(atendimento);
        }

        // GET: AreaTecnico/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var atendimento = await _context.Atendimentos.FindAsync(id);
            if (atendimento == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Usuarios, "Id", "Id", atendimento.ClienteId);
            ViewData["TecnicoId"] = new SelectList(_context.Usuarios, "Id", "Id", atendimento.TecnicoId);
            return View(atendimento);
        }

        // POST: AreaTecnico/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClienteId,TecnicoId,ValorOrcamento,ObservacaoTecnico,Descricao,Status,DataAbertura,DataConclusao")] Atendimento atendimento)
        {
            if (id != atendimento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(atendimento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AtendimentoExists(atendimento.Id))
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
            ViewData["ClienteId"] = new SelectList(_context.Usuarios, "Id", "Id", atendimento.ClienteId);
            ViewData["TecnicoId"] = new SelectList(_context.Usuarios, "Id", "Id", atendimento.TecnicoId);
            return View(atendimento);
        }

        // GET: AreaTecnico/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var atendimento = await _context.Atendimentos
                .Include(a => a.Cliente)
                .Include(a => a.Tecnico)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (atendimento == null)
            {
                return NotFound();
            }

            return View(atendimento);
        }

        // POST: AreaTecnico/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var atendimento = await _context.Atendimentos.FindAsync(id);
            if (atendimento != null)
            {
                _context.Atendimentos.Remove(atendimento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AtendimentoExists(int id)
        {
            return _context.Atendimentos.Any(e => e.Id == id);
        }
    }
}
