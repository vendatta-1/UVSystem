using System.Reflection;

namespace UVS.Modules.Authentication.Presentation;

public static class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}