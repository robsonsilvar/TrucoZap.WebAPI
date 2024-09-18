using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrucoZap.Jogo.Entidade;
using TrucoZap.Jogo.Service.Base;
using TrucoZap.Jogo.Entidade.Enumeradores;
using TrucoZap.Jogo.Service.Interface;
using System.Threading.Tasks;
using TrucoZap.Jogo.Service.Model;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace TrucoZap.Jogo.Service
{
    public class JogoService : IJogoService
    {
        private BaralhoService _baralhoService;
        private Player1JogadorService _player1Service;
        private RoboJogadorService _roboService;
        private CartaService _cartaService = new CartaService();
        private PlacarEntidade _placar;
        private int _numSub = 1;
        private int _pontosSubRobo = 0;
        private int _pontosSubPlayer1 = 0;
        public int PontosMaximoVencer = 4;
        public int NumRodada = 0;
        private CartaEntidade _cartvaViraPraManilha;
        private JogoModel _jogoContexto;

        public CartaEntidade CartavaViradaPraManilha
        {
            get
            {
                return _cartvaViraPraManilha;
            }
        }

        public PlacarEntidade Placar
        {
            get { return _placar; }
        }

        public MaoJogadorEntidade MaoRobo
        {
            get
            {
                return _roboService.MaoJogador;
            }

        }

        public MaoJogadorEntidade MaoPlayer1
        {
            get
            {
                return _player1Service.MaoJogador;
            }
        }

        public int NumSub
        {
            get
            {
                return _numSub;
            }
        }      

        public int PontosSubRobo {
            get { return _pontosSubRobo; }
        }

        public int PontosSubPlayer1
        {
            get { return _pontosSubPlayer1; }
        }

        IConfiguration _config = null;

        public JogoService()
        {
           
        }

        public JogoService(IHttpContextAccessor accessor, IConfiguration config)
        {
            _config = config;
            if (accessor.HttpContext.Session.GetString("jogoModel") != null)
            {
                _jogoContexto = JsonSerializer.Deserialize<JogoModel>(accessor.HttpContext.Session.GetString("jogoModel"));
                _player1Service = new Player1JogadorService(_jogoContexto.MaoPlayer1);
                _roboService = new RoboJogadorService(_jogoContexto.MaoRobo, _config);
                _cartvaViraPraManilha = _jogoContexto.Manilha;
                _numSub = _jogoContexto.NumSub;
                _placar = _jogoContexto.Placar;
                _pontosSubPlayer1 = _jogoContexto.PontosSubPlayer1;
                _pontosSubRobo = _jogoContexto.PontosSubRobo;
            }
        }

        public bool IniciarProximaRodada()
        {
            _baralhoService = new BaralhoService(new CartaService());
            //primeira rodada
            if (_placar == null)
            {
                ConfigurarProxRodada();
                _placar = new PlacarEntidade();
                return true;
            }
            else
            {
                PontuarRodadaAtual();

                if (ObterVencedorGeral() == null)
                {
                    ConfigurarProxRodada();
                    return true;
                }
            }
            return false;
        }

        private void ConfigurarProxRodada()
        {
            NumRodada++;
            _numSub = 1;
            _player1Service = new Player1JogadorService(_baralhoService.DarCartas());
            _roboService = new RoboJogadorService(_baralhoService.DarCartas(), _config);
            _cartvaViraPraManilha = _baralhoService.VirarCartaPraManilha();
        }

        private void PontuarRodadaAtual()
        {
            TipoJogadorEnum jogador = (TipoJogadorEnum)ObterVencedorRodadaAtual();
            if (jogador == TipoJogadorEnum.Player1)
                _placar.PontosPlayer1++;
            else
                _placar.PontosRobo++;

            _pontosSubPlayer1 = 0;
            _pontosSubRobo = 0;
        }

        public bool IniciarProximaSubRodada()
        {
            if (_numSub == 4)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public CartaEntidade JogarPlayer1(int valorCarta)
        {
            return _player1Service.Jogar(valorCarta);
        }

        public CartaEntidade JogarRobo()
        {
            return _roboService.Jogar();
        }

        public TipoJogadorEnum? ObterVencedorGeral()
        {
            //fim de jogo
            if (_placar.PontosRobo == PontosMaximoVencer || _placar.PontosPlayer1 == PontosMaximoVencer)
            {
                if (_placar.PontosRobo == PontosMaximoVencer)
                    return TipoJogadorEnum.Robo;
                else
                    return TipoJogadorEnum.Player1;
            }
            return null;
        }

        public TipoJogadorEnum? ObterVencedorRodadaAnterior()
        {
            if (_pontosSubRobo > _pontosSubPlayer1)
                return TipoJogadorEnum.Robo;
            else
                return TipoJogadorEnum.Player1;
        }

        public TipoJogadorEnum? ObterVencedorRodadaAtual()
        {
            if (_numSub >= 2)
            {
                if (_pontosSubRobo > _pontosSubPlayer1)
                    return TipoJogadorEnum.Robo;
                else if (_pontosSubRobo < _pontosSubPlayer1)
                    return TipoJogadorEnum.Player1;
                else//igual
                    return null;
            }
            return null;

        }

        public void SomarSubPontos(TipoJogadorEnum tipoJogador, int pontos)
        {
            if (tipoJogador == TipoJogadorEnum.Player1)
            {
                _pontosSubPlayer1 += pontos;
            }
            else
            {
                _pontosSubRobo += pontos;
            }
        }

        public void SomarPontos(TipoJogadorEnum tipoJogador, int pontos)
        {
            if (tipoJogador == TipoJogadorEnum.Player1)
            {
                _placar.PontosPlayer1 += pontos;
            }
            else
            {
                _placar.PontosRobo += pontos;
            }
        }

        private void DarAsCartas()
        {
            if (ObterVencedorRodadaAnterior() == null)
            {
                _roboService.ReceberCartas(_baralhoService.DarCartas());
                _player1Service.ReceberCartas(_baralhoService.DarCartas());
                _pontosSubPlayer1 = _pontosSubRobo = 0;
            }

        }

        public TipoJogadorEnum ProcessarSubRodada()
        {
            CartaEntidade cartaPlayer1 = _player1Service.MaoJogador.CartasJogadas.LastOrDefault();
            CartaEntidade cartaRobo = _roboService.MaoJogador.CartasJogadas.LastOrDefault();
            _cartaService.DefinirNome(cartaPlayer1);
            _cartaService.DefinirNome(cartaRobo);
            TipoJogadorEnum? jogadorVenceuSub = null;
            CalcularManilha(cartaPlayer1, cartaRobo);
            if (cartaPlayer1.Valor > cartaRobo.Valor)
            {
                SomarSubPontos(TipoJogadorEnum.Player1, 1);
                jogadorVenceuSub = TipoJogadorEnum.Player1;
            }
            else
            {
                SomarSubPontos(TipoJogadorEnum.Robo, 1);
                jogadorVenceuSub = TipoJogadorEnum.Robo;
            }

            if (ObterVencedorRodadaAtual() != null)
                _numSub = 4; //fim de rodada
            else
                //proxima sub
                _numSub++;

            return (TipoJogadorEnum)jogadorVenceuSub;
        }

        private void CalcularManilha(CartaEntidade cartaPlayer1, CartaEntidade cartaRobo)
        {
            if (_cartaService.IsManilha(cartaPlayer1, CartavaViradaPraManilha))
            {
                cartaPlayer1.Valor += 100;
            }
            if (_cartaService.IsManilha(cartaRobo, CartavaViradaPraManilha))
            {
                cartaRobo.Valor += 100;
            }

        }

        public bool FimSub()
        {
            return _numSub == 3;
        }

        //private int CalcularCarta(CartaEntidade cartaJogada)
        //{ 
        //IsManilha()
        //}


    }
}
