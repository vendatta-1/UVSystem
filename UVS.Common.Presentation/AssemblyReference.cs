using System.Reflection;

namespace UVS.Common.Presentation;

public  static class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}