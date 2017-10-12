using System;
using TM.Helper;

namespace TMShopsCore.Common
{
    public class Auth
    {
        private const string SessionAuth = "Auth";
        private const string SessionRoles = "Roles";
        private const string SessionAllowRoles = "AllowRoles";
        public const string API = "API";
        //private static Models.Users AuthAccount;
        public static Models.Users SetAuth(Models.Users Users, System.Collections.Generic.List<Models.RolesAcess> Roles = null, System.Collections.Generic.List<Models.RolesAcess> AllowRoles = null)
        {
            try
            {
                TMAppContext.Http.Session.Set(SessionAuth, Newtonsoft.Json.JsonConvert.SerializeObject(Users));
                TMAppContext.Http.Session.Set(SessionRoles, Newtonsoft.Json.JsonConvert.SerializeObject(Roles));
                TMAppContext.Http.Session.Set(SessionAllowRoles, Newtonsoft.Json.JsonConvert.SerializeObject(AllowRoles));
                var ss = TM.Helper.TMAppContext.Http.Session.Get<string>(SessionAuth);
                //AppHttpContext.Current.Session.Set(SessionAuth, Newtonsoft.Json.JsonConvert.SerializeObject(Users));
                //var ss = AppHttpContext.Current.Session.Get<string>(SessionAuth);
                //AuthAccount = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Users>(ss);
                // return AuthAccount;
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Users>(ss);
            }
            catch (Exception) { throw; }
        }
        public static Models.Users AuthUser
        {
            get { return Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Users>(TM.Helper.TMAppContext.Http.Session.Get<string>(SessionAuth)); }
        }
        public static System.Collections.Generic.List<Models.RolesAcess> AuthRoles
        {
            get { return Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<Models.RolesAcess>>(TM.Helper.TMAppContext.Http.Session.Get<string>(SessionRoles)); }
        }
        public static System.Collections.Generic.List<Models.RolesAcess> AuthAllowRoles
        {
            get { return Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<Models.RolesAcess>>(TM.Helper.TMAppContext.Http.Session.Get<string>(SessionAllowRoles)); }
        }
        public static bool isAuth
        {
            get
            {
                if (TM.Helper.TMAppContext.Http.Session.Get<string>(SessionAuth) != null)
                    return true;
                return false;
            }
        }
        //public static Models.Users GetAuth()
        //{
        //    return Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Users>(AppHttpContext.Current.Session.Get<string>(SessionAuth));
        //    //return AuthAccount;
        //}
        public static bool Logout()
        {
            try
            {
                TM.Helper.TMAppContext.Http.Session.Remove(SessionAuth);
                TM.Helper.TMAppContext.Http.Session.Remove(SessionRoles);
                TM.Helper.TMAppContext.Http.Session.Remove(SessionAllowRoles);
                return true;
            }
            catch (Exception) { throw; }
        }
        private static System.Collections.Generic.List<Models.Users> ListAuthStatic()
        {
            var List = new System.Collections.Generic.List<Models.Users>();
            var User = new Models.Users();
            //tuanmjnh
            User.Id = Guid.Parse("f4191f70-2c4a-442e-a62d-b4b6833b33f4");
            User.Username = "tuanmjnh";
            User.Password = "aa2de065c899d53d7031b0975c56062f";//"Tuanmjnh@tm"; //"fc44d0279133a2f46178134ce9bf2167";//tuanmjnh@123
            User.Salt = "1c114c58-69d9-41e6-bd3e-363906e04e50";
            User.FullName = "Bơm Bơm";
            User.FullName = "tuanmjnh";
            User.Mobile = "0123456789";
            User.Email = "tuanmjnh@SuperAdmin.com";
            User.Address = "SuperAdmin";
            User.Roles = Common.Roles.superadmin;
            User.Orders = 0;
            User.CreatedBy = "f4191f70-2c4a-442e-a62d-b4b6833b33f4";
            User.CreatedAt = DateTime.Now;
            User.UpdatedBy = "f4191f70-2c4a-442e-a62d-b4b6833b33f4";
            User.UpdatedAt = DateTime.Now;
            User.LastLogin = DateTime.Now;
            User.Flag = 1;
            User.Extras = null;
            //Add User to list
            List.Add(User);
            return List;
        }
        public static Models.Users isAuthStatic(string username, string password)
        {
            foreach (var item in ListAuthStatic())
            {
                if (item.Username == username && item.Password == TM.Encrypt.MD5.CryptoMD5TM(password + item.Salt))
                {
                    return item;
                }
            }
            return null;
        }
        public static string getUserAction()
        {
            return "," + AuthUser.Id + "," + AuthUser.Username + "," + AuthUser.FullName + ",";
        }
    }
    public class Roles
    {
        public enum RoleMethod
        {
            GET = 1,
            POST = 2,
            PUT = 3,
            DELETE = 4,
        }
        public enum RoleAction
        {
            SELECT = 1,
            INSERT = 2,
            UPDATE = 3,
            DELETE = 4,
            VERIFY = 5,
            REMOVE = 6
        }

        public const string Allow = "ALLOW";

        public const string superadmin = "187eb627-0a7b-44a8-83c4-ceb4829709a3";
        public const string admin = "ee82e7f1-592c-4f5c-95fa-7cad30b14a2d";
        public const string mod = "238391cd-990d-4f3b-8d0c-0300416f9263";
        public const string director = "121ab8e5-1ad2-4b78-8ff2-4d953c9b71a8";
        public const string leader = "d0443498-09c4-4267-a7c9-2a20dde8e925";
        public const string staff = "dc67601d-ad74-4813-8293-8d4a07db1d31";
        public const string guest = "1abb3ce3-9da2-42d8-9e3c-59e839fd821a";
    }
}