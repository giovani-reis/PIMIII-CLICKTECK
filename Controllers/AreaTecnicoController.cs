using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PIM_TechTrust.Models;
using PIM_TechTrust.Models.Enums;
using PIMIII_CLICKTECK.Data;
using PIMIII_CLICKTECK.Models.ViewModels;

namespace PIMIII_CLICKTECK.Controllers
{
    public class AreaTecnicoController : Controller
    {
        private readonly Context _context;

        public AreaTecnicoController(Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var usuLogado = TempData["Tecnico"];
            if (usuLogado == null)
            {
                return RedirectToAction("Index", "Login");
            }
            TempData.Keep("Tecnico");

            int idUsuario = (int)usuLogado;

            // 1. Busca o técnico logado
            var tecnicoLogado = _context.Usuarios
                .Include(u => u.TecnicoPerfil)
                .FirstOrDefault(u => u.Id == idUsuario);

            // 2. Cálculos para o Card 1 (Ganhos e Reparos Concluídos)
            var ganhosTotais = _context.Atendimentos
                .Where(a => a.TecnicoId == idUsuario && a.Status == StatusAtendimento.Finalizado)
                .Sum(a => a.ValorOrcamento) ?? 0;

            var totalReparos = _context.Atendimentos
                .Count(a => a.TecnicoId == idUsuario && a.Status == StatusAtendimento.Finalizado);

            // 3. Cálculos para o Card 3 (Média de Avaliações)
            // OBS: Ajuste '_context.Avaliacoes' para o nome da sua tabela/entidade de notas (se houver)
            var avaliacaoMedia = _context.Avaliacoes
                .Where(av => av.TecnicoId == idUsuario)
                .Average(av => (decimal?)av.Nota) ?? 0;

            var totalAvaliacoes = _context.Avaliacoes
                .Count(av => av.TecnicoId == idUsuario);

            // 4. Monta a ViewModel com todos os dados
            var model = new DashboardTecnicoViewModel
            {
                Tecnico = tecnicoLogado,
                AtendimentosPendentes = _context.Atendimentos
                    .Include(a => a.Cliente)
                    .Where(a => a.Status == StatusAtendimento.Solicitado && a.TecnicoId == idUsuario)
                    .ToList(),

                GanhosTotais = ganhosTotais,
                TotalReparosConcluidos = totalReparos,
                AvaliacaoMedia = avaliacaoMedia,
                TotalAvaliacoes = totalAvaliacoes
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EnviarOrcamento(int AtendimentoId, decimal Valor, string Observacao)
        {
            var atendimento = _context.Atendimentos.Find(AtendimentoId);
            if (atendimento != null)
            {
                atendimento.ValorOrcamento = Valor;
                atendimento.ObservacaoTecnico = Observacao;
                atendimento.Status = StatusAtendimento.PendenteAprovacao;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}
