using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TrucoZap.Jogo.Entidade;
using TrucoZap.Jogo.Entidade.Enumeradores;
using TrucoZap.Jogo.Service.Interface;
using TrucoZap.Jogo.Service.Model;


namespace TrucoZap.Jogo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JogoController : ControllerBase
    {
        private readonly ILogger<JogoController> _logger;
        private IJogoService _jogoService;

        public JogoController(ILogger<JogoController> logger, IJogoService jogoService)
        {
            _logger = logger;
            _jogoService = jogoService;
        }

        [HttpGet("jogar-player1")]
        public IActionResult JogarPlayer1(int cartaValor)
        {
            JogoModel jogoModel = JsonSerializer.Deserialize<JogoModel>(HttpContext.Session.GetString("jogoModel"));

            if (_jogoService.IniciarProximaSubRodada())
            {
                CartaEntidade cartaEscolhidaJogador = jogoModel.MaoPlayer1.CartasTotais.Where(c => c.Valor == cartaValor).FirstOrDefault();
                jogoModel.JogadaPlayer1 = _jogoService.JogarPlayer1(cartaEscolhidaJogador.Valor);
                jogoModel.MaoPlayer1 = _jogoService.MaoPlayer1;

                jogoModel.JogadaRobo = _jogoService.JogarRobo();
                jogoModel.MaoRobo = _jogoService.MaoRobo;

                jogoModel.VencedorSub = _jogoService.ProcessarSubRodada();
                jogoModel.NumSub = _jogoService.NumSub;
                jogoModel.PontosSubPlayer1 = _jogoService.PontosSubPlayer1;
                jogoModel.PontosSubRobo = _jogoService.PontosSubRobo;
            }
            
            //fim de rodada
            if (!_jogoService.IniciarProximaSubRodada())
            {
                _jogoService.IniciarProximaRodada();
                jogoModel.PontosSubPlayer1 = _jogoService.PontosSubPlayer1;
                jogoModel.PontosSubRobo = _jogoService.PontosSubRobo;
                jogoModel.MaoPlayer1 = _jogoService.MaoPlayer1;
                jogoModel.MaoRobo = _jogoService.MaoRobo;
                jogoModel.Placar = _jogoService.Placar;
                jogoModel.Manilha = _jogoService.CartavaViradaPraManilha;
                jogoModel.NumSub = _jogoService.NumSub;                
            }

            HttpContext.Session.SetString("jogoModel", JsonSerializer.Serialize(jogoModel));
            return Ok(jogoModel);
        }

        [HttpGet("iniciar")]
        public IActionResult Iniciar()
        {
            JogoModel jogoModel = new JogoModel();

            if (HttpContext.Session.GetString("jogoModel") == null)
            {
                _jogoService.IniciarProximaRodada();
                jogoModel.MaoPlayer1 = _jogoService.MaoPlayer1;
                jogoModel.MaoRobo = _jogoService.MaoRobo;               
                jogoModel.Manilha = _jogoService.CartavaViradaPraManilha;
                jogoModel.NumSub = _jogoService.NumSub;
                jogoModel.Placar = _jogoService.Placar;
                HttpContext.Session.SetString("jogoModel", JsonSerializer.Serialize(jogoModel));

            }
            return Ok(jogoModel);
        }
    }
}
