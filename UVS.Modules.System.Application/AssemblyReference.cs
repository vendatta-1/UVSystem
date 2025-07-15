using System.Reflection;


namespace UVS.Modules.System.Application;

public static class AssemblyReference
{
  public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
