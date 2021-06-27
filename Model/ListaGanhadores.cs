using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 

namespace Pontos_de_Domino_UWP.Model
{
    public class ListaGanhadores
    {
        public double pontosA1 { get; set; }
        public double pontosB2 { get; set; }
        public double pontosA { get; set; }
        public double pontosB { get; set; }
        public string data { get; set; }
        public int ID { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }
        public string nomeA { get; set; }
        public string nome { get; set; }
        public string nomeB { get; set; }
        public string Result { get; set; }
        public bool Is20M { get; set; }
        public bool Is2X { get; set; }
    }
}