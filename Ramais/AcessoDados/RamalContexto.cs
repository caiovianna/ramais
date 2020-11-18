using Ramais.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Ramais.AcessoDados
{
    public class RamalContexto : DbContext
    {
        public DbSet<Ramal> Ramais { get; set; }
        public DbSet<Auditoria> Auditoria { get; set; }

    }
}