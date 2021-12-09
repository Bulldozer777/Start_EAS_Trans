
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_explorer
{
  public class ApplicationContext : DbContext
    {
        public ApplicationContext()
            : base("DBExplorer")
        { }

        public DbSet<DBSmartExplorer> DBSmartExplorers { get; set; }
    }
}
