using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Mastonet;
using Mastonet.Entities;

namespace MastodonCSharp
{
    class Instance : Mastodon
    {
        private AppRegistration client_id { get; }
        private Auth user_id { get; }
        private MastodonClient client { get; }

        public Instance(string base_uri) : base(base_uri)
        {
            client_id = base.GetClientId();
            user_id = base.GetUserId();
            client = new MastodonClient(client_id, user_id);
        }

        public async Task Toot(string status, Visibility visibility = Visibility.Public)
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var response = await client.PostStatus(status, visibility);
            Console.WriteLine(response.Id);
        }
    }
}
