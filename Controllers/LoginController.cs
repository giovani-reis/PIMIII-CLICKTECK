using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PIM_TechTrust.Models;
using PIM_TechTrust.Models.Enums;
using PIMIII_CLICKTECK.Data;

namespace PIMIII_CLICKTECK.Controllers
{
    public class LoginController : Controller
    {
        private readonly Context _context;

        public LoginController(Context context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string email, string senha)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (usuario == null)
            {
                ViewBag.Erro = "Usuário não encontrado";
                return View();
            }

            bool senhaCorreta = BCrypt.Net.BCrypt.Verify(senha, usuario.Senha);
            if (senhaCorreta)
            {
                switch (usuario.Role)
                {
                    case Role.Cliente:
                        TempData["Usu"] = usuario.Id;
                        return RedirectToAction("Index", "AreaCliente");
                    case Role.Tecnico:
                        TempData["Tecnico"] = usuario.Id;
                        return RedirectToAction("Index", "AreaTecnico");
                    case Role.Admin:
                        return RedirectToAction("Index", "AreaAdmin");


                }
            }
            ViewBag.Erro = "Usuario ou Senha inválidos";
            ViewBag.Email = email;
            ViewBag.Senha = senha;
            return View();
         }
     }
}
