using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TM.Helper;
using Dapper;
using NETCore.DapperKit.Extensions;

namespace TMShopsCore.Common
{
    public class Settings
    {
        public const string SessionSettings = "Settings";
        public static List<Models.Settings> Get()
        {
            TM.Connection.SQLServer SQLServer = new TM.Connection.SQLServer();
            try
            {
                if (TMAppContext.Http.Session.Get<string>(SessionSettings) == null)
                {
                    var settings = SQLServer.Connection.GetAll<Models.Settings>();
                    TMAppContext.Http.Session.Set(SessionSettings, JsonConvert.SerializeObject(settings));
                }
                return JsonConvert.DeserializeObject<List<Models.Settings>>(TMAppContext.Http.Session.Get<string>(SessionSettings));
            }
            catch (Exception) { throw; }
            finally
            {
                SQLServer.Close();
            }
        }
        public static void Set()
        {
            TM.Connection.SQLServer SQLServer = new TM.Connection.SQLServer();
            try
            {
                var settings = SQLServer.Connection.GetAll<Models.Settings>();
                TMAppContext.Http.Session.Set(SessionSettings, JsonConvert.SerializeObject(settings));
            }
            catch (Exception) { throw; }
            finally
            {
                SQLServer.Close();
            }
        }
    }
}
