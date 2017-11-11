using System;
using Newtonsoft.Json;

namespace Registry.Data.Services
{
  public class Tracer
  {
    public static void TraceEnter(string message,
      [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
    {
      Console.WriteLine($"{DateTime.Now.ToString("yyyy.MM.dd, hh:mm:ss")} | {memberName} called with parameter {message}");
    }

    public static void TraceInfo([System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
    {
      Console.WriteLine($"{DateTime.Now.ToString("yyyy.MM.dd, hh:mm:ss")} | {memberName} called");
    }

    public static void TraceExit([System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
    {
      Console.WriteLine($"{DateTime.Now.ToString("yyyy.MM.dd, hh:mm:ss")} | {memberName} exited");
    }

    public static T TraceReturn<T>(T obj, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
      where T : class
    {
      Console.WriteLine(
        $"{DateTime.Now.ToString("yyyy.MM.dd, hh:mm:ss")} | {memberName} returned = <{JsonConvert.SerializeObject(obj)}>");
      return obj;
    }
  }
}