using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrucoZap.Jogo.Entidade;
using TrucoZap.Jogo.Entidade.Enumeradores;

namespace TrucoZap.Jogo.Service.Interface
{
    public interface IJogoService
    {
        #region Props
        public MaoJogadorEntidade MaoRobo { get; }
        public MaoJogadorEntidade MaoPlayer1 { get; }
        public CartaEntidade CartavaViradaPraManilha { get; }
        public PlacarEntidade Placar { get; }
        public int NumSub { get; }
        public int PontosSubRobo { get; }
        public int PontosSubPlayer1 { get; }

        #endregion
        bool IniciarProximaRodada();
        bool IniciarProximaSubRodada();
        CartaEntidade JogarPlayer1(int valorCarta);
        CartaEntidade JogarRobo();
        TipoJogadorEnum? ObterVencedorGeral();
        TipoJogadorEnum? ObterVencedorRodadaAnterior();
        TipoJogadorEnum? ObterVencedorRodadaAtual();
        void SomarSubPontos(TipoJogadorEnum tipoJogador, int pontos);
        void SomarPontos(TipoJogadorEnum tipoJogador, int pontos);
        TipoJogadorEnum ProcessarSubRodada();
        bool FimSub();


    }
}
