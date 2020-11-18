using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ramais.Models
{
    public class Auditoria
    {
        public int Id { get; set; }
        public String NomeAnterior { get; set; }
        public String NomeNovo { get; set; }
        public String NumeroAnterior { get; set; }
        public String NumeroNovo { get; set; }
        public String SetorAnterior { get; set; }
        public String SetorNovo { get; set; }
        public String MatriculaAntiga { get; set; }
        public String MatriculaNova { get; set; }
        public String Usuario { get; set; }
        public String Acao { get; set; }
        public DateTime Data { get; set; }
        public Ramal Ramal { get; set; }
        public int RamalId { get; set; }
    }
}