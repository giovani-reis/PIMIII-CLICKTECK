using Microsoft.AspNetCore.Mvc;
using PIM_TechTrust.Models;
using PIMIII_CLICKTECK.Data;
using PIMIII_CLICKTECK.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using PIMIII_CLICKTECK.Models.DTOs;


namespace PIMIII_CLICKTECK.Controllers
{
    public class ChatController : Controller
    {
        private readonly Context _context;

        public ChatController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int atendimentoId)
        {
            // TEMPORÁRIO
            // depois você troca pela autenticação real
            string tipo;
            if (TempData["Usu"] != null)
            {
                tipo = "Usu";
            }else if (TempData["Tecnico"] != null)
            {
                tipo = "Tecnico";
            }
            else
            {
                return RedirectToAction("Index", "Login");

            }
            var usuLogado = TempData[tipo];
            TempData.Keep(tipo); // Mantém o usuário logado na sessão

            int idUsuario = (int)usuLogado;

            var atendimento = await _context.Atendimentos
                .Include(a => a.Cliente)
                .Include(a => a.Tecnico)
                .FirstOrDefaultAsync(a => a.Id == atendimentoId);

            if (atendimento == null)
                return NotFound();

            int outroUsuarioId;
            string nomeOutroUsuario;

            if (idUsuario == atendimento.ClienteId)
            {
                outroUsuarioId = atendimento.TecnicoId;
                nomeOutroUsuario = atendimento.Tecnico.Nome;
            }
            else
            {
                outroUsuarioId = atendimento.ClienteId;
                nomeOutroUsuario = atendimento.Cliente.Nome;
            }

            var mensagens = await _context.Mensagens
                .Where(m => m.AtendimentoId == atendimentoId)
                .OrderBy(m => m.DataEnvio)
                .ToListAsync();

            var vm = new ChatViewModel
            {
                AtendimentoId = atendimentoId,
                UsuarioLogadoId = idUsuario,
                OutroUsuarioId = outroUsuarioId,
                NomeOutroUsuario = nomeOutroUsuario,
                Mensagens = mensagens
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EnviarMensagem([FromBody] EnviarMensagemDTO dto)
        {
            // TEMPORÁRIO
            // depois você troca pela autenticação real

            string tipo;
            if (TempData["Usu"] != null)
            {
                tipo = "Usu";
            }
            else if (TempData["Tecnico"] != null)
            {
                tipo = "Tecnico";
            }
            else
            {
                return RedirectToAction("Index", "Login");

            }
            var usuLogado = TempData[tipo];
            TempData.Keep(tipo); // Mantém o usuário logado na sessão

            int idUsuario = (int)usuLogado;

            var atendimento = await _context.Atendimentos
                .FirstOrDefaultAsync(a => a.Id == dto.AtendimentoId);

            if (atendimento == null)
                return BadRequest();

            int destinatarioId;

            if (idUsuario == atendimento.ClienteId)
                destinatarioId = atendimento.TecnicoId;
            else
                destinatarioId = atendimento.ClienteId;

            var mensagem = new Mensagem
            {
                Conteudo = dto.Conteudo,
                AtendimentoId = dto.AtendimentoId,
                RemetenteId = idUsuario,
                DestinatarioId = destinatarioId,
                DataEnvio = DateTime.Now
            };

            _context.Mensagens.Add(mensagem);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                sucesso = true
            });
        }
    }
}

