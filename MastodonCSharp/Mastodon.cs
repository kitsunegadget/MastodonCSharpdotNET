using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using Mastonet;
using Mastonet.Entities;

namespace MastodonCSharp
{
    public class Mastodon
    {
        //private const string base_uri = "mstdn-workers.com";
        private string client_cred { get; }
        private string user_cred { get; }
        protected string base_uri { get; }

        public Mastodon(string base_uri)
        {
            this.base_uri = base_uri;
            client_cred = base_uri + "_client_cred.txt";
            user_cred = base_uri + "_user_cred.txt";

            var authClient = new AuthenticationClient(base_uri);

            if (!File.Exists(client_cred))
            {
                //クライアント登録 1度のみでおｋ。
                RegistApp(authClient).Wait();
                Console.WriteLine("App Regist ok.");
                Console.ReadKey();
                Console.WriteLine();
            }

            if (!File.Exists(user_cred))
            {
                GetToken(authClient).Wait(); //トークン取得
                Console.WriteLine("Token ok.");
                Console.ReadKey();
                Console.WriteLine();
            }
        }

        //app登録
        private async Task RegistApp(AuthenticationClient authClient)
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; 
            var appRegistration = await authClient.CreateApp("Foxydon C#", Scope.Read | Scope.Write | Scope.Follow, "https://twitter.com/yuzsr_", "urn:ietf:wg:oauth:2.0:oob");
            
            var json = JsonConvert.SerializeObject(appRegistration);
            string filename = client_cred;
            File.WriteAllText(filename, json);
        }

        //トークン取得
        private async Task GetToken(AuthenticationClient authClient)
        {
            var appregist = GetClientId();
            authClient.AppRegistration = appregist;

            var url = authClient.OAuthUrl();
            System.Diagnostics.Process.Start(url);

            Console.WriteLine("Enter Athorization CODE.");
            string code = Console.ReadLine();

            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; 
            var auth = await authClient.ConnectWithCode(code);

            var json = JsonConvert.SerializeObject(auth);
            string filename = user_cred;
            File.WriteAllText(filename, json);
        }

        protected AppRegistration GetClientId()
        {
            var client_id = File.ReadAllText(client_cred);
            AppRegistration ap = JsonConvert.DeserializeObject<AppRegistration>(client_id);
            ap.Instance = base_uri;
            ap.Scope = Scope.Read | Scope.Write | Scope.Follow;

            return ap;
        }

        protected Auth GetUserId()
        {
            var user_id = File.ReadAllText(user_cred);
            Auth au = JsonConvert.DeserializeObject<Auth>(user_id);

            return au;
        }
    }
}
