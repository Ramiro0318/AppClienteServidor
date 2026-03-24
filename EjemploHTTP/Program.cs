using System.Diagnostics.Contracts;
using System.Net;
using System.Text;

string url = "http://localhost:8081/ejemplo/";
//string url = "http://*/ejemplo/";
HttpListener servidorhttp = new();

servidorhttp.Prefixes.Add(url);
servidorhttp.Start();

var index = File.ReadAllText("index.html");
byte[] buffer = Encoding.UTF8.GetBytes(index);

while (true)
{
    var contexto = servidorhttp.GetContext();  //El contexto es la combinacion entre una petición y una respuesta
                                               //Bloquea el hilo
    if (contexto.Request.HttpMethod == "GET")
    {
        EnviarIndex(buffer, contexto);
    }
    else if (contexto.Request.HttpMethod == "POST")
    {
        StreamReader streamReader = new StreamReader
            (contexto.Request.InputStream);
        var texto = streamReader.ReadToEnd();

        Console.WriteLine(texto);
        Console.WriteLine("Hello, World!");
        EnviarIndex(buffer, contexto);
    }

}

static void EnviarIndex(byte[] buffer, HttpListenerContext contexto)
{
    contexto.Response.StatusCode = 200;
    contexto.Response.ContentLength64 = buffer.Length;
    contexto.Response.ContentType = "text/html";
    contexto.Response.OutputStream.Write(buffer, 0, buffer.Length);
    contexto.Response.Close();
}