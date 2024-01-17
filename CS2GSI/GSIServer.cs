using System.Net;
using System.Text;
using Microsoft.Extensions.Logging;

// ReSharper disable LocalizableElement

namespace CS2GSI;

internal class GSIServer
{
    private HttpListener HttpListener { get; init; }
    internal delegate void OnMessageEventHandler(string content);
    internal event OnMessageEventHandler? OnMessage;
    private bool _keepRunning = true;
    internal bool IsRunning { get; private set; }
    private ILogger? logger;

    internal GSIServer(int port, ILogger? logger = null)
    {
        this.logger = logger;
        string prefix = $"http://127.0.0.1:{port}/";
        this.HttpListener = new HttpListener();
        this.HttpListener.Prefixes.Add(prefix);
        this.HttpListener.Start();
        this.logger?.Log(LogLevel.Information, $"Listening on {prefix}");

        Thread connectionListener = new (HandleConnection);
        connectionListener.Start();

        this.IsRunning = true;
    }

    private async void HandleConnection()
    {
        while (this._keepRunning)
        {
            HttpListenerContext context = await HttpListener.GetContextAsync();
            HttpListenerRequest request = context.Request;
            
            this.logger?.Log(LogLevel.Information, $"[{request.HttpMethod}] {request.Url} - {request.UserAgent}");

            HttpResponseMessage responseMessage = new (HttpStatusCode.Accepted);
            context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(responseMessage.ToString()));

            StreamReader reader = new (request.InputStream, request.ContentEncoding);
            string content = await reader.ReadToEndAsync();
            this.logger?.Log(LogLevel.Debug, $"Message Content:\n{content}");
            OnMessage?.Invoke(content);
        }
        this.HttpListener.Close();
        this.IsRunning = false;
        this.logger?.Log(LogLevel.Information, "Stopped GSIServer.");
    }

    internal void Dispose()
    {
        this.logger?.Log(LogLevel.Information, "Stopping GSIServer.");
        this._keepRunning = false;
    }

}