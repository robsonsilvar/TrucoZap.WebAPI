using System;
using System.Collections.Generic;
using System.Text;

namespace TrucoZap.Jogo.Entidade
{
    [Serializable]
    public class MaoJogadorEntidade
    {
        public List<CartaEntidade> CartasTotais { get; set; }      
        public List<CartaEntidade> CartasRestantes { get; set; }           
        public List<CartaEntidade> CartasJogadas { get; set; }

        public MaoJogadorEntidade() { }

        public MaoJogadorEntidade(List<CartaEntidade> cartas)
        {
            CartasTotais = new List<CartaEntidade>();         
            CartasJogadas = new List<CartaEntidade>();

            this.CartasRestantes = cartas;

            foreach (CartaEntidade carta in cartas)
            {
                CartasTotais.Add(carta);
            }
        }
    


    }
}
