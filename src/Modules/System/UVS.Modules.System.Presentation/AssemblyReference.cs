using System.Reflection;

namespace UVS.Modules.System.Presentation;

public sealed class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}