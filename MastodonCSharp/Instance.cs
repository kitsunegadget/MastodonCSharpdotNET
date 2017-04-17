using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Mastonet;

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
            client_id.Instance = base_uri;
            user_id = base.GetUserId();
            client = new MastodonClient(client_id, user_id.AccessToken);
        }

        public async Task Toot(string status, Visibility visibility = Visibility.Public)
        {
            /*
            var http = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://" + base_uri + "/api/v1/statuses");
            request.Headers.Add("Authorization", "Bearer " + user_id.AccessToken);
            request.Content = new StringContent("status=" + status + "&visibility=" + visibility, Encoding.UTF8, "application/x-www-form-urlencoded");

            
            var values = new Dictionary<string, string>
                {
                    { "status", status },
                    { "visibility", visibility }
                };

            var content = new FormUrlEncodedContent(values);
            
            HttpResponseMessage response = await http.SendAsync(request);
            */
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var response = await client.PostStatus(status, visibility);
            Console.WriteLine(response.Id);
        }
    }
}
