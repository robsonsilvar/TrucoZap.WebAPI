using System;
using System.Collections.Generic;
using System.Text;
using TrucoZap.Jogo.Entidade;

namespace TrucoZap.Jogo.Service.Interface
{
    public interface IAnaliseRoboService
    {
        CartaEntidade EscolherCarta(MaoJogadorEntidade mao);
    }
}
