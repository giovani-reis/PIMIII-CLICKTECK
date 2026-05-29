using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PIM_TechTrust.Models;
using PIM_TechTrust.Models.Enums;
using PIMIII_CLICKTECK.Data;
using PIMIII_CLICKTECK.Models.ViewModels;
using System.Globalization;

namespace PIMIII_CLICKTECK.Controllers
{
    public class AreaTecnicoController : Controller
    {
        private readonly Context _context;

        public AreaTecnicoController(Context context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var usuLogado = TempData["Tecnico"];
            if (usuLogado == null)
            {
                return RedirectToAction("Index", "Login");
            }
            TempData.Keep("Tecnico");

            int idUsuario = (int)usuLogado;

            // 1. Busca o técnico logado
            var tecnicoLogado = await _context.Usuarios
                .Include(u => u.TecnicoPerfil)
                .FirstOrDefaultAsync(u => u.Id == idUsuario);

            // 2. Cálculos para o Card 1 (Ganhos e Reparos Concluídos)
            var ganhosTotais = await _context.Atendimentos
                .Where(a => a.TecnicoId == idUsuario && a.Status == StatusAtendimento.Finalizado)
                .SumAsync(a => a.ValorOrcamento) ?? 0;

            var totalReparos = await _context.Atendimentos
                .CountAsync(a => a.TecnicoId == idUsuario && a.Status == StatusAtendimento.Finalizado);

            // 3. Cálculos para o Card 3 (Média de Avaliações)
            // OBS: Ajuste '_context.Avaliacoes' para o nome da sua tabela/entidade de notas (se houver)
            var avaliacaoMedia = await _context.Avaliacoes
                .Where(av => av.TecnicoId == idUsuario)
                .AverageAsync(av => (decimal?)av.Nota) ?? 0;

            var totalAvaliacoes = await _context.Avaliacoes
                .CountAsync(av => av.TecnicoId == idUsuario);

            // 4. Monta a ViewModel com todos os dados
            var model = new DashboardTecnicoViewModel
            {
                Tecnico = tecnicoLogado,
                AtendimentosPendentes = await _context.Atendimentos
                    .Include(a => a.Cliente)
                    .Where(a => a.Status == StatusAtendimento.Solicitado && a.TecnicoId == idUsuario)
                    .ToListAsync(),

                GanhosTotais = ganhosTotais,
                TotalReparosConcluidos = totalReparos,
                AvaliacaoMedia = avaliacaoMedia,
                TotalAvaliacoes = totalAvaliacoes
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EnviarOrcamento(int AtendimentoId, decimal Valor, string Observacao)
        {
            var atendimento = await _context.Atendimentos.FindAsync(AtendimentoId);
            if (atendimento != null)
            {
                atendimento.ValorOrcamento = Valor;
                atendimento.ObservacaoTecnico = Observacao;
                atendimento.Status = StatusAtendimento.PendenteAprovacao;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("MeusServicos");
        }

        [HttpPost]
        public async Task<IActionResult> CancelarAtendimento(int id)
        {
            var atendimento = await _context.Atendimentos.FindAsync(id);
            if (atendimento != null)
            {
                atendimento.Status = StatusAtendimento.Cancelado;
                await _context.SaveChangesAsync();
                TempData["MensagemSucesso"] = "Agendamento cancelado com sucesso.";
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }



        [HttpGet]
        public async Task<IActionResult> MeusServicos()
        {
            var usuLogado = TempData["Tecnico"];
            if (usuLogado == null)
            {
                return RedirectToAction("Index", "Login");
            }
            TempData.Keep("Tecnico"); // Mantém a sessão ativa

            int idUsuario = (int)usuLogado;

            // Busca apenas os serviços que já passaram da fase de "Solicitado" ou "Cancelado"
            var servicos = await _context.Atendimentos
                .Include(a => a.Cliente)
                .Where(a => a.TecnicoId == idUsuario &&
                            (a.Status == StatusAtendimento.PendenteAprovacao ||
                             a.Status == StatusAtendimento.Aprovado ||
                             a.Status == StatusAtendimento.Finalizado ||
                             a.Status == StatusAtendimento.Cancelado))
                .OrderByDescending(a => a.DataAbertura)
                .Select(a => new MeusAgendamentosTecnicoViewModel
                {   
                    AtendimentoId = a.Id,
                    ClienteNome = a.Cliente.Nome,
                    Aparelho = a.Aparelho,
                    Descricao = a.Descricao,
                    ValorOrcamento = a.ValorOrcamento,
                    DataAbertura = a.DataAbertura,
                    DataConclusao = a.DataConclusao,
                    Status = a.Status.ToString().ToUpper()
                })
                .ToListAsync();

            return View(servicos);
        }

        // Action para processar a finalização do serviço pelo Técnico
        [HttpPost]
        public async Task<IActionResult> FinalizarServico(int atendimentoId)
        {
            var atendimento = await _context.Atendimentos.FindAsync(atendimentoId);
            if (atendimento != null && atendimento.Status == StatusAtendimento.Aprovado)
            {
                atendimento.Status = StatusAtendimento.Finalizado;
                atendimento.DataConclusao = DateTime.Now;
                await _context.SaveChangesAsync();
                TempData["MensagemSucesso"] = "Reparo finalizado com sucesso!";
            }

            return RedirectToAction("MeusServicos");
        }



        [HttpGet]
        public IActionResult Avaliacoes()
        {
            var usuLogado = TempData["Tecnico"];
            if (usuLogado == null)
            {
                return RedirectToAction("Index", "Login");
            }
            TempData.Keep("Tecnico"); // Mantém a sessão ativa

            int tecnicoId = (int)usuLogado;


            var avaliacoes = _context.Avaliacoes
                .Include(a => a.Cliente)
                .Where(a => a.TecnicoId == tecnicoId)
                .OrderByDescending(a => a.Nota)
                .ToList();

            var viewModel = new AvaliacoesViewModel
            {
                Avaliacoes = avaliacoes,
                TotalAvaliacoes = avaliacoes.Count,

                MediaNotas = avaliacoes.Count > 0
                    ? avaliacoes.Average(a => a.Nota)
                    : 0,

                Quantidade5 = avaliacoes.Count(a => a.Nota == 5),
                Quantidade4 = avaliacoes.Count(a => a.Nota == 4),
                Quantidade3 = avaliacoes.Count(a => a.Nota == 3),
                Quantidade2 = avaliacoes.Count(a => a.Nota == 2),
                Quantidade1 = avaliacoes.Count(a => a.Nota == 1)
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Perfil()
        {
            // ID do técnico logado
            var usuLogado = TempData["Tecnico"];
            if (usuLogado == null)
            {
                return RedirectToAction("Index", "Login");
            }
            TempData.Keep("Tecnico"); // Mantém a sessão ativa

            int idTecnico = (int)usuLogado;

            // Busca técnico no banco
            var tecnico = await _context.Usuarios
                .Include(a => a.TecnicoPerfil)
                .FirstOrDefaultAsync(t => t.Id == idTecnico);


            var viewModel = new PerfilTecnicoViewModel
            {
                Nome = tecnico.Nome,
                Email = tecnico.Email,
                Bio = tecnico.TecnicoPerfil.Descricao,
                FotoPerfil = tecnico.TecnicoPerfil.FotoUrl,

                Especialidades = await _context.TecnicoEspecialidades
                                                .Include(es => es.Especialidade)
                                                .Where(e => e.TecnicoPerfilId == idTecnico)
                                                .Select(es => es.Especialidade.Nome)
                                                .ToListAsync()
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Ajuda()
        {
            return View();
        }
    }
}
