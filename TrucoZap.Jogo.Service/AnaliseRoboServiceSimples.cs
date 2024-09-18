using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrucoZap.Jogo.Entidade;
using TrucoZap.Jogo.Service.Interface;

namespace TrucoZap.Jogo.Service
{
    public class AnaliseRoboServiceSimples : IAnaliseRoboService
    {       
        public AnaliseRoboServiceSimples() {
           
        }
        public CartaEntidade EscolherCarta(MaoJogadorEntidade mao)
        {
            Random rand = new Random();
            int tentativaValorCarta = rand.Next(40 + 1);
            CartaEntidade cartaEscolhida = null;
            while (cartaEscolhida == null)
            {
                cartaEscolhida = mao.CartasRestantes.Where(c => c.Valor == tentativaValorCarta).FirstOrDefault();
                tentativaValorCarta = rand.Next(40 + 1);
            }           
            return cartaEscolhida;
        }
      
    }
}
