using System.Reflection;


namespace UVS.Modules.System.Application;

public static class ApplicationConfiguration
{
  public static readonly Assembly Assembly = typeof(ApplicationConfiguration).Assembly;
}
