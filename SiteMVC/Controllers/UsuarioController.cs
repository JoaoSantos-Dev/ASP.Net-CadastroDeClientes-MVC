using Microsoft.AspNetCore.Mvc;
using SiteMVC.Models;
using SiteMVC.Repositorio;

namespace SiteMVC.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        public UsuarioController(IUsuarioRepositorio usuarioRespositorio)
        {
            _usuarioRepositorio = usuarioRespositorio;
        }

        public IActionResult Index()
        {
            List<UsuarioModel> contatos = _usuarioRepositorio.BuscarTodos();

            return View(contatos);
        }

        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Criar(UsuarioModel usuario, IFormFile foto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Verifica se uma foto foi enviada
                    if (foto != null && foto.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            // Copia o conteúdo do arquivo para o MemoryStream
                            foto.CopyTo(memoryStream);

                            // Converte o conteúdo do MemoryStream em um array de bytes
                            usuario.Foto = memoryStream.ToArray();
                        }
                    }

                    // Adiciona o usuário ao repositório (com a foto, se enviada)
                    _usuarioRepositorio.Adicionar(usuario);
                    TempData["MensagemSucesso"] = "Cadastro realizado com sucesso!";
                    return RedirectToAction("Index");
                }

                return View(usuario);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Falha no Cadastro, tente novamente! detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
