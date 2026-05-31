using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
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

        public async Task<IActionResult> Index()
        {
            var usuLogado = TempData["Usu"];
            if (usuLogado == null)
            {
                return RedirectToAction("Index", "Login");
            }
            TempData.Keep("Usu");

            int idUsuario = (int)usuLogado;


            var tecnicos = await _context.TecnicoPerfis
                .Include(t => t.Usuario)
                .Where(t => t.Disponivel)
                .OrderByDescending(a => _context.Avaliacoes
                    .Where(t => t.TecnicoId == a.UsuarioId)
                    .Average(a => (double?)a.Nota))
                .Select(t => new DashBoardViewModel
                {
                    Id = t.Id,
                    IdTecnico = t.UsuarioId,
                    Nome = t.Usuario.Nome,
                    FotoUrl = t.FotoUrl ?? "default-avatar.jpg",
                    Descricao = t.Descricao,

                    // 1. Média de Avaliações (Trata nulos se não houver notas)
                    Avaliacao = _context.Avaliacoes
                        .Where(a => a.TecnicoId == t.UsuarioId)
                        .Average(a => (double?)a.Nota) ?? 0.0,
                        

                    // 2. Contagem de Reparos (Retorna 0 se não houver correspondência)
                    QtdReparos = _context.Atendimentos
                        .Count(a =>  a.TecnicoId == t.UsuarioId && a.Status == StatusAtendimento.Finalizado),

                    // 3. Pegando as Especialidades Reais da sua tabela de ligação
                    Tags = _context.TecnicoEspecialidades
                        .Where(te => te.TecnicoPerfilId == t.Id)
                        .Select(te => te.Especialidade.Nome) // Nome vem da tabela ESPECIALIDADES
                        .ToList()
                }).ToListAsync();

            var telaUsuario = new TelaUsuarioViewModel
            {
                Tecnico = tecnicos,
                Usuario = _context.Usuarios.Find(idUsuario)
            };

            return View(telaUsuario);
        }

        [HttpPost]
        public async Task<IActionResult> CriarAtendimento(Atendimento atendimento)
        {
            TempData.Keep("Usu");
            // O Entity Framework preenche o objeto 'atendimento' com os nomes dos inputs do seu Modal

            // 1. Forçamos os dados que o usuário não preenche manualmente
            atendimento.DataAbertura = DateTime.Now;
            atendimento.Status = StatusAtendimento.Solicitado;

            // 2. Salva no banco
            await _context.Atendimentos.AddAsync(atendimento);
            await _context.SaveChangesAsync();

            // 3. Cria uma mensagem para exibir na volta
            TempData["MensagemSucesso"] = "Solicitação de reparo enviada com sucesso!";

            // 4. Redireciona de volta para a vitrine de técnicos
            return RedirectToAction("MeusAgendamentos", "AreaCliente");
        }





        // Action para carregar a página de listagem
        public async Task<IActionResult> MeusAgendamentos()
        {
            var usuLogado = TempData["Usu"];
            if (usuLogado == null)
            {
                return RedirectToAction("Index", "Login");
            }
            TempData.Keep("Usu"); // Mantém o usuário logado na sessão

            int idUsuario = (int)usuLogado;

            // Buscamos os atendimentos e transformamos para a ViewModel que criamos
            var agendamentos = await _context.Atendimentos
                .Include(a => a.Tecnico)// Traz os dados do usuário técnico
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
                                    .FirstOrDefault() ?? "default-avatar.jpg",

                    TecnicoEspecialidade = _context.TecnicoEspecialidades
                                            .Include(te => te.Especialidade)
                                            .Where(te => te.TecnicoPerfil.UsuarioId == a.TecnicoId)
                                            .Select(te => te.Especialidade.Nome)
                                            .FirstOrDefault() ?? "Técnico Especialista",
                    JaAvaliado = _context.Avaliacoes
                                    .Any(av => av.TecnicoId == a.TecnicoId),

                    Modelo = a.Aparelho,
                    ServicoDescricao = a.Descricao,
                    ValorOrcamento = a.ValorOrcamento,
                    ObservacaoTecnico = a.ObservacaoTecnico,
                    DataAbertura = a.DataAbertura,
                    Status = a.Status.ToString().ToUpper() // Usamos ToUpper para facilitar o switch no HTML
                })
                .ToListAsync();


            return View(agendamentos);
        }

        // Action para processar o cancelamento pelo cliente
        [HttpPost]
        public async Task<IActionResult> CancelarAtendimento(int id)
        {
            var atendimento = _context.Atendimentos.Find(id);
            if (atendimento != null)
            {
                atendimento.Status = StatusAtendimento.Cancelado;
                await _context.SaveChangesAsync();
                TempData["MensagemSucesso"] = "Agendamento cancelado com sucesso.";
            }
            return RedirectToAction("MeusAgendamentos");
        }

        [HttpPost]
        public async Task<IActionResult> AprovarAtendimento(int id)
        {
            var atendimento = _context.Atendimentos.Find(id);
            if (atendimento != null)
            {
                atendimento.Status = StatusAtendimento.Aprovado;
                await _context.SaveChangesAsync();
                TempData["MensagemSucesso"] = "Agendamento aprovado com sucesso.";
            }
            return RedirectToAction("MeusAgendamentos");
        }

        [HttpPost]
        public async Task<IActionResult> AvaliarTecnico(int AtendimentoId, int Nota, string Comentario)
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

                await _context.SaveChangesAsync();
                TempData["MensagemSucesso"] = "Obrigado por sua avaliação!";
            }
            else
            {
                TempData["Erro"] = "Atendimento não encontrado.";
            }

            return RedirectToAction("MeusAgendamentos");
        }

        /*
         #####################
         */

        [HttpGet]
        public async Task<IActionResult> Perfil()
        {
            var usuLogado = TempData["Usu"];

            if (usuLogado == null)
                return RedirectToAction("Index", "Login");

            TempData.Keep("Usu");

            int idUsu = (int)usuLogado;

            var perfil = await _context.Usuarios
                .FirstOrDefaultAsync(t => t.Id == idUsu);

            if (perfil == null)
                return NotFound();

            var vm = new Usuario
            {
                Nome = perfil.Nome,
                Email = perfil.Email
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarDados(
            Usuario model)
        {
            var usuLogado = TempData["Usu"];

            if (usuLogado == null)
                return RedirectToAction("Index", "Login");

            TempData.Keep("Usu");

            int idUsu = (int)usuLogado;

            var perfil = await _context.Usuarios
                .FirstOrDefaultAsync(t => t.Id == idUsu);

            if (perfil == null)
                return NotFound();

            perfil.Nome = model.Nome;
            perfil.Email = model.Email;


            await _context.SaveChangesAsync();

            TempData["Sucesso"] = "Dados atualizados com sucesso!";
            return RedirectToAction(nameof(Perfil));
        }

        

        [HttpPost]
        public async Task<IActionResult> AtualizarSenha(
                                            string SenhaAtual,
                                            string NovaSenha,
                                            string ConfirmarSenha)
        {
            var usuLogado = TempData["Usu"];

            if (usuLogado == null)
                return RedirectToAction("Index", "Login");

            TempData.Keep("Usu");

            int idUsu = (int)usuLogado;


            var perfil = await _context.Usuarios
                .FirstOrDefaultAsync(t => t.Id == idUsu);

            if (perfil == null)
                return NotFound();

            if (!BCrypt.Net.BCrypt.Verify(
                SenhaAtual,
                perfil.Senha))
            {
                TempData["Erro"] = "Senha incorreta!";
                return RedirectToAction(nameof(Perfil));
            }

            if (NovaSenha != ConfirmarSenha)
            {
                TempData["Erro"] = "As senhas não coincidem";
                return RedirectToAction(nameof(Perfil));
            }

            perfil.Senha =
                BCrypt.Net.BCrypt.HashPassword(NovaSenha);

            await _context.SaveChangesAsync();

            TempData["Sucesso"] = "Senha atualizada com sucesso!";
            return RedirectToAction(nameof(Perfil));
        }

        [HttpGet]
        public IActionResult Ajuda()
        {
            return View();
        }

    }
}