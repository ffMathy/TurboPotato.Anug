using FluffySpoon.Kibana;
using System;
using System.IO;

namespace Kibana
{
    class Program
    {
        static void Main(string[] args)
        {
            var kibanaUrl = File.ReadAllText("KibanaUrl.txt");

            var parser = new KibanaParser();
            var elasticsearchQuery = parser.ConvertUrlToElasticsearchQueryString(kibanaUrl, "order_date");

            Console.WriteLine(elasticsearchQuery);

            Console.ReadLine();
        }
    }
}
