using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Net.Security;
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

            if (!File.Exists(client_cred))
            {
                //クライアント登録 1度のみでおｋ。
                RegistApp(base_uri).Wait();
                Console.WriteLine("App Regist ok.");
                //Console.ReadKey();
            }

            if (!File.Exists(user_cred))
            {
                GetToken(); //トークン取得
                Console.WriteLine("Token ok.");
                //Console.ReadKey();
            }
        }

        //app登録
        private async Task RegistApp(string base_uri)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var appRegistration = await MastodonClient.CreateApp(base_uri, "Foxydon C#", Scope.Read | Scope.Write | Scope.Follow, "https://twitter.com/yuzsr_", "urn:ietf:wg:oauth:2.0:oob");
            
            var json = JsonConvert.SerializeObject(appRegistration);
            string filename = client_cred;
            File.WriteAllText(filename, json);
        }

        //トークン取得
        private void GetToken()
        {
            var appregist = GetClientId();
            appregist.Instance = base_uri;
            appregist.Scope = Scope.Read | Scope.Write | Scope.Follow;

            var client = new MastodonClient(appregist);
            var url = client.OAuthUrl();
            System.Diagnostics.Process.Start(url);

            Console.WriteLine("Enter Athorization CODE.");
            string code = Console.ReadLine();

            GetTokenSync(appregist, code).Wait();
        }
        //なぜ分けた…
        private async Task GetTokenSync(AppRegistration appregist, string code)
        {
            var client = new MastodonClient(appregist);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var auth = await client.ConnectWithCode(code);

            var json = JsonConvert.SerializeObject(auth);
            string filename = user_cred;
            File.WriteAllText(filename, json);
        }

        protected AppRegistration GetClientId()
        {
            var client_id = File.ReadAllText(client_cred);
            AppRegistration ap = JsonConvert.DeserializeObject<AppRegistration>(client_id);

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
