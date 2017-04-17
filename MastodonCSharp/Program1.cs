using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MastodonCSharp
{
    class Program1
    {
        public static void Main(string[] args)
        {
            //接続するインスタンス。
            string base_uri = "mstdn-workers.com";
            //インスタンスを指定して生成。
            //インスタンスごとにインスタンスを作成して複数垢の管理ができる？
            var mastodon = new Instance(base_uri);

            Console.WriteLine("Input Text...");
            string text = Console.ReadLine();
            mastodon.Toot(text, Mastonet.Visibility.Unlisted).Wait();

            Console.WriteLine("Toot ok. Please any key...");
            Console.ReadKey();
        }
    }
}
