using System;
using System.IO;
// Should probably implemented using ILoggers but I didn't bother yet.
// using Microsoft.Extensions.Logging;

namespace RayTracer;

interface ILogger
{
    public void WriteLine(string s);
}

class Logger : ILogger, IDisposable
{
    public Logger()
    {
        long _time = DateTime.Now.ToFileTime();
        //Allows for timdependent logs if necessary
        // path = "logs_" + _time + ".txt";
        path = "logs.txt";
        _file = File.CreateText(path);
        _file.WriteLine("Starting logging at" + _time);
    }
    public string path;
    private readonly StreamWriter _file;
    public void WriteLine(string line)
    {
        _file.WriteLine(line);
    }

    public void Dispose()
    {
        _file.Dispose();
    }
}

class EmptyLogger : ILogger, IDisposable
{
    public void WriteLine(string line) { }

    public void Dispose() { }
}
