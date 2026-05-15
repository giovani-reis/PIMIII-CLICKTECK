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
            // Busca o usuário, o perfil dele e os serviços pendentes
            var tecnicoLogado = _context.Usuarios
                .Include(u => u.TecnicoPerfil)
                .FirstOrDefault(u => u.Id == idUsuario); // ID do técnico logado

            var model = new DashboardTecnicoViewModel
            {
                Tecnico = tecnicoLogado,
                AtendimentosPendentes = _context.Atendimentos
                    .Include(a => a.Cliente)
                    .Where(a => a.Status == StatusAtendimento.Solicitado)
                    .ToList()
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
