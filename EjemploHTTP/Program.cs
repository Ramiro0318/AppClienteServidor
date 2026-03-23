using System.Net;

string url = "http://*:8081/ejemplo";
HttpListener servidorhttp = new();

servidorhttp.Prefixes.Add(url);
servidorhttp.Start();

while (true)
{
    var contexto = servidorhttp.GetContext();
}

Console.WriteLine("Hello, World!");


