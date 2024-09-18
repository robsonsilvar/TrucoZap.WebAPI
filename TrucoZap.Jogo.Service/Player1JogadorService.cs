using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrucoZap.Jogo.Entidade;
using TrucoZap.Jogo.Service.Base;

namespace TrucoZap.Jogo.Service
{
    public class Player1JogadorService : BaseJogadorService
    {

        public Player1JogadorService(MaoJogadorEntidade maoJogador) : base(maoJogador)
        {

        }

        public CartaEntidade Jogar(int valorCarta)
        {
            CartaEntidade cartaSacada = null;
            cartaSacada = MaoJogador.CartasRestantes.Where(c => c.Valor == valorCarta).FirstOrDefault();
            SacarCarta(cartaSacada);
            return cartaSacada;
        }


    }
}
