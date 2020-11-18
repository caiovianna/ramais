using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ramais.Models
{
    public class Ramal
    {
        public int Id { get; set; }
        public String Nome { get; set; }
        public String Numero { get; set; }
        public String Setor { get; set; }
        public String Matricula { get; set; }
        public String UsuarioCriacao { get; set; }
        public String UsuarioAlteracao { get; set; }
        public Boolean Excluido { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAlteracao { get; set; }
    }
}