using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrucoZap.Jogo.Entidade;
using TrucoZap.Jogo.Service.Base;
using TrucoZap.Jogo.Service.Factory;
using TrucoZap.Jogo.Service.Interface;

namespace TrucoZap.Jogo.Service
{
    public class RoboJogadorService : BaseJogadorService
    {
        private IAnaliseRoboService _analise = null;
        public RoboJogadorService(MaoJogadorEntidade maoJogador, IConfiguration configuration) :base(maoJogador)
        {            
            _analise = new AnaliseRoboServiceFactory().CriarAnaliseRoboService(configuration["TipoAnaliseRobo"]);
        }
      
        public CartaEntidade Jogar()
        {           
            CartaEntidade cartaEscolhida = _analise.EscolherCarta(MaoJogador);
            SacarCarta(cartaEscolhida);        
            return cartaEscolhida;
        }

      
    }
}
