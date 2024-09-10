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

        public IActionResult Editar(int id)
        {
            UsuarioModel usuario = _usuarioRepositorio.ListarPorId(id);
            return View(usuario);
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

        public IActionResult ApagarConfirmacao(int id)
        {
            UsuarioModel usuario = _usuarioRepositorio.ListarPorId(id);
            return View(usuario);
        }

        public IActionResult Apagar(int id)
        {
            try
            {
                bool apagado = _usuarioRepositorio.Apagar(id);
                if (apagado)
                {
                    TempData["MensagemSucesso"] = "Deleção realizada com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = $"Falha na deleção, tente novamente!";
                }
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Falha na deleção, tente novamente! detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Editar(UsuarioSemSenhaModel usuarioSemSenhaModel, IFormFile? foto)
        {
            try
            {
                UsuarioModel usuario = null;
                // Verifica se o modelo é válido
                if (ModelState.IsValid)
                {
                    var usuarioExistente = _usuarioRepositorio.ListarPorId(usuarioSemSenhaModel.Id);

                    usuario = new UsuarioModel()
                    {
                        Id = usuarioSemSenhaModel.Id,
                        Nome = usuarioSemSenhaModel.Nome,
                        Email = usuarioSemSenhaModel.Email,
                        Login = usuarioSemSenhaModel.Login,
                        Perfil = usuarioSemSenhaModel.Perfil,
                        Foto = usuarioSemSenhaModel.Foto
                    };
                    // Se uma nova foto foi enviada, atualiza o campo Foto
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
                    else
                    {
                        // Se nenhuma nova foto foi enviada, mantém a foto existente
                        usuario.Foto = usuarioExistente.Foto;
                    }

                    // Atualiza o usuário no repositório
                    _usuarioRepositorio.Atualizar(usuario);

                    TempData["MensagemSucesso"] = "Atualização realizada com sucesso!";
                    return RedirectToAction("Index");
                }

                // Se o modelo não for válido, retorna a mesma visão com erros de validação
                return View(usuario);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Falha na Atualização, tente novamente! Detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }


        }

        public IActionResult GetImage(int id)
        {
            UsuarioModel usuario = _usuarioRepositorio.ListarPorId(id);
            if (usuario?.Foto != null)
            {
                return File(usuario.Foto, "image/jpeg"); // Ajuste o tipo MIME se necessário
            }
            return NotFound();
        }
    }
}
