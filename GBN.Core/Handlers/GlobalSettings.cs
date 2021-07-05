using System;
using System.Collections;
using System.Collections.Generic;
using UnivIntel.Common;
using UnivIntel.GBN.Core.DataAccess.Entities;

namespace UnivIntel.GBN.Core.Handlers
{
    public class GlobalSettings
    {

      public static string BaseConfig { get; set; } = @"C:\Configs\gbn.";

#if DEBUGPROD
        public static Hashtable Config = ConfigReader.ReadConfig($"{BaseConfig}system.production.config");
#elif DEBUG
        public static Hashtable Config = ConfigReader.ReadConfig($"{BaseConfig}development.config");
#else
        public static Hashtable Config = ConfigReader.ReadConfig($"{BaseConfig}system.production.config");
#endif

        public static string Db_Server = Config.Get("db_server");
        public static string Db_Name = Config.Get("db_name");
        public static string Db_UserId = Config.Get("db_userId");
        public static string Db_Password = Config.Get("db_password");
        public static string ConnStr { get; set; } = $"Host={Db_Server};Database={Db_Name};Username={Db_UserId};Password={Db_Password}";

        public static IDictionary<string, AccountRank> AccountRanks = new Dictionary<string, AccountRank>
        {
            ["free"] = new AccountRank { Id = "free", Companies = 1, Name = "Free" },
            ["new"] = new AccountRank { Id = "new", Companies = 0, Name = "New" },
            ["premium"] = new AccountRank { Id = "premium", Companies = 9999, Name = "Premium" },
            ["paid"] = new AccountRank { Id = "paid", Companies = 10, Name = "Paid" },
        };

        public static string GetAppName(Guid input)
        {
            return $"accId:{input}";
        }
    }
}
