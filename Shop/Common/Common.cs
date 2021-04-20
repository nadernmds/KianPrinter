using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages;

namespace Shop.Common
{

    public static class Commons
    {
        public static string ConnectionString = @"Integrated Security=False;Data Source=192.168.1.213;Initial Catalog=Shop_db;Password=rzk123!@#;User ID=sa";
        //new Models.OmranDBEntities().Database.Connection.ConnectionString;
        
    }
}