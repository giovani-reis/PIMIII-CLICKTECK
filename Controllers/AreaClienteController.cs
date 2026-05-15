using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PIM_TechTrust.Models;
using PIM_TechTrust.Models.Enums;
using PIMIII_CLICKTECK.Data;
using PIMIII_CLICKTECK.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                        .Where(a => a.TecnicoId == t.UsuarioId)
                        .Average(a => (double?)a.Nota) ?? 0.0,

                    // 2. Contagem de Reparos (Retorna 0 se não houver correspondência)
                    QtdReparos = _context.Atendimentos
                        .Count(a => a.TecnicoId == t.UsuarioId && a.Status == StatusAtendimento.Finalizado),

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
            atendimento.Status = StatusAtendimento.Solicitado;

            // 2. Salva no banco
            _context.Atendimentos.Add(atendimento);
            _context.SaveChanges();

            // 3. Cria uma mensagem para exibir na volta
            TempData["MensagemSucesso"] = "Solicitação de reparo enviada com sucesso!";

            // 4. Redireciona de volta para a vitrine de técnicos
            return RedirectToAction("MeusAgendamentos", "AreaCliente");
        }

        // Action para carregar a página de listagem
        public IActionResult MeusAgendamentos()
        {
            var usuLogado = TempData["Usu"];
            if (usuLogado == null)
            {
                return RedirectToAction("Index", "Login");
            }
            TempData.Keep("Usu"); // Mantém o usuário logado na sessão

            int idUsuario = (int)usuLogado;

            // Buscamos os atendimentos e transformamos para a ViewModel que criamos
            var agendamentos = _context.Atendimentos
                .Include(a => a.Tecnico) // Traz os dados do usuário técnico
                .Where(a => a.ClienteId == idUsuario)
                .OrderByDescending(a => a.DataAbertura)
                .Select(a => new MeusAgendamentosViewModel
                {
                    AtendimentoId = a.Id,
                    TecnicoNome = a.Tecnico.Nome,
                    // Buscamos a foto e especialidade através do Perfil do Técnico
                    TecnicoFoto = _context.TecnicoPerfis
                                    .Where(p => p.UsuarioId == a.TecnicoId)
                                    .Select(p => p.FotoUrl)
                                    .FirstOrDefault() ?? "/img/default-avatar.jpg",

                    TecnicoEspecialidade = _context.TecnicoEspecialidades
                                            .Include(te => te.Especialidade)
                                            .Where(te => te.TecnicoPerfil.UsuarioId == a.TecnicoId)
                                            .Select(te => te.Especialidade.Nome)
                                            .FirstOrDefault() ?? "Técnico Especialista",

                    Modelo = a.Aparelho,
                    ServicoDescricao = a.Descricao,
                    DataAbertura = a.DataAbertura,
                    Status = a.Status.ToString().ToUpper() // Usamos ToUpper para facilitar o switch no HTML
                })
                .ToList();

            return View(agendamentos);
        }

        // Action para processar o cancelamento pelo cliente
        [HttpPost]
        public IActionResult CancelarAtendimento(int id)
        {
            var atendimento = _context.Atendimentos.Find(id);
            if (atendimento != null && atendimento.Status == StatusAtendimento.Solicitado)
            {
                atendimento.Status = StatusAtendimento.Cancelado;
                _context.SaveChanges();
                TempData["MensagemSucesso"] = "Agendamento cancelado com sucesso.";
            }
            return RedirectToAction("MeusAgendamentos");
        }

        [HttpPost]
        public IActionResult AprovarAtendimento(int id)
        {
            var atendimento = _context.Atendimentos.Find(id);
            if (atendimento != null)
            {
                atendimento.Status = StatusAtendimento.Aprovado;
                _context.SaveChanges();
                TempData["MensagemSucesso"] = "Agendamento aprovado com sucesso.";
            }
            return RedirectToAction("MeusAgendamentos");
        }

        [HttpPost]
        public IActionResult AvaliarTecnico(int AtendimentoId, int Nota, string Comentario)
        {
            var atendimento = _context.Atendimentos.Find(AtendimentoId);
            if (atendimento != null)
            {
                // 2. Cria o objeto de avaliação baseado no modelo que você enviou
                var novaAvaliacao = new Avaliacao
                {
                    ClienteId = atendimento.ClienteId,
                    TecnicoId = atendimento.TecnicoId,
                    Nota = Nota,
                    Comentario = Comentario
                };

                _context.Avaliacoes.Add(novaAvaliacao);

                // 3. Opcional: Marcar que este atendimento já foi avaliado
                // atendimento.Avaliado = true; 

                _context.SaveChanges();
                TempData["MensagemSucesso"] = "Obrigado por sua avaliação!";
            }
            else
            {
                TempData["Erro"] = "Atendimento não encontrado.";
            }

            return RedirectToAction("MeusAgendamentos");
        }
    }
}