using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoStationManagerApplication.Core
{
    public class test
    {
        var dbHelper = new DbHelper("Server=localhost;Database=EcoStationManager;Uid=root;Pwd=;");
        dbHelper.TestConnection();

    }
}
