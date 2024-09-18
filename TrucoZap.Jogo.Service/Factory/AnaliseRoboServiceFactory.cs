using System;
using System.Collections.Generic;
using System.Text;
using TrucoZap.Jogo.Entidade;
using TrucoZap.Jogo.Service.Interface;

namespace TrucoZap.Jogo.Service.Factory
{
    public class AnaliseRoboServiceFactory
    {
        public IAnaliseRoboService CriarAnaliseRoboService(string tipo) {
            if (tipo == "simples")
                return new AnaliseRoboServiceSimples();
            else
                return new AnaliseRoboServiceEvoluido();

            throw new Exception("Argumento TipoAnaliseRobo faltando");
        }     
      
    }
}
