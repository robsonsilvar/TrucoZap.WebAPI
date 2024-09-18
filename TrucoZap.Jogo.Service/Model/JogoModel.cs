using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrucoZap.Jogo.Entidade;
using TrucoZap.Jogo.Entidade.Enumeradores;

namespace TrucoZap.Jogo.Service.Model
{
    public class JogoModel
    {
        public JogoModel() { }
        public PlacarEntidade Placar { get; set; }
        public int NumSub { get; set; }
        public int PontosSubRobo { get; set; }
        public int PontosSubPlayer1 { get; set; }
        public MaoJogadorEntidade MaoPlayer1 { get; set; }
        public MaoJogadorEntidade MaoRobo { get; set; }
        public CartaEntidade Manilha { get; set; }
        public CartaEntidade JogadaPlayer1 { get; set; }
        public CartaEntidade JogadaRobo { get; set; }
        public TipoJogadorEnum VencedorSub { get; set; }
    }
}
