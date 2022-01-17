using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Strategy.Models
{
    public enum DatabaseType
    {
        SqlServer = 1,
        MongoDb = 2
    }

    public class Settings
    {
        public static readonly string DatabaseClaimType = "databaseType";

        public DatabaseType DatabaseType;
        public DatabaseType GetDefaultDatabaseType => DatabaseType.SqlServer;

    }
}
