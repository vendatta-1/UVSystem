using System.Reflection;

namespace UVS.Modules.Authentication.Application;

public static class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}