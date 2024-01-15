// See https://aka.ms/new-console-template for more information

using CS2GSI.TestApp;
using Microsoft.Extensions.Logging;

public class TestApp
{
    public static void Main(string[] args)
    {
        new CS2GSI.CS2GSI(new Logger(LogLevel.Information));
    }
}