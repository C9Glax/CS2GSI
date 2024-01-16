// See https://aka.ms/new-console-template for more information

using CS2GSI.TestApp;
using Microsoft.Extensions.Logging;

public class TestApp
{
    public static void Main(string[] args)
    {
        CS2GSI.CS2GSI gsi = new (new Logger(LogLevel.Debug));
        while(gsi.IsRunning)
            Thread.Sleep(10);
    }
}