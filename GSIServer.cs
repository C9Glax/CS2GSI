using System.Net;
using System.Text;
// ReSharper disable LocalizableElement

namespace CS2GSI;

internal class GSIServer
{
    private HttpListener HttpListener { get; init; }
    internal delegate void OnMessageEventHandler(string content);
    internal event OnMessageEventHandler? OnMessage;
    private bool _keepRunning = true;
    internal bool IsRunning { get; private set; }

    internal GSIServer(int port)
    {
        HttpListener = new HttpListener();
        HttpListener.Prefixes.Add($"http://127.0.0.1:{port}/");
        HttpListener.Start();

        Thread connectionListener = new (HandleConnection);
        connectionListener.Start();

        IsRunning = true;
    }

    private async void HandleConnection()
    {
        while (_keepRunning)
        {
            HttpListenerContext context = await HttpListener.GetContextAsync();
            HttpListenerRequest request = context.Request;
            
            Console.WriteLine($"[{request.HttpMethod}] {request.Url} - {request.UserAgent}");

            HttpResponseMessage responseMessage = new (HttpStatusCode.Accepted);
            context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(responseMessage.ToString()));

            StreamReader reader = new (request.InputStream, request.ContentEncoding);
            string content = await reader.ReadToEndAsync();
            OnMessage?.Invoke(content);
        }
        HttpListener.Close();
        IsRunning = false;
    }

    internal void Dispose()
    {
        _keepRunning = false;
    }

}