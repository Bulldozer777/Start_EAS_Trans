using Smart_explorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Start_EAS_Trans
{
    class DBForm1
    {
        public void DBWrite(string Action, string NumberOps, string StatusConnection, string IpConnection, DateTime DateTimeAction)
        {
            Smart_explorer.ApplicationContext db1 = new ApplicationContext();
            DBSmartExplorer DBSmartExplorers = new DBSmartExplorer();
            DBSmartExplorers.Action = Action;
            DBSmartExplorers.NumberOps = NumberOps;
            DBSmartExplorers.StatusConnection = StatusConnection;
            DBSmartExplorers.IpConnection = IpConnection;
            DBSmartExplorers.DateTimeAction = DateTimeAction;
            db1.DBSmartExplorers.Add(DBSmartExplorers);
            db1.SaveChanges();
        }
    }
}
