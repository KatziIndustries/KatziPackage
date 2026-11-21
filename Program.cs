using System.Diagnostics;
using Katzi.Package;

internal static class Program
{
    public static void Main(string[] args)
    {
        string[]? missingRequired = KatziPackager.GetMissingRequiredFiles("/home/katzi/KatziPackage");
        string[]? missingOptional = KatziPackager.GetMissingOptionalFiles("/home/katzi/KatziPackage");
        string[]? unknown = KatziPackager.GetUnknownFiles("/home/katzi/KatziPackage");

        if (missingRequired != null)
        {
            Console.WriteLine("Missing required files:");
            foreach (string missing in missingRequired) Console.WriteLine($"- {missing}");
        }
        else Console.WriteLine("All required files found");

        if (missingOptional != null)
        {
            Console.WriteLine("Missing optional files:");
            foreach (string missing in missingOptional) Console.WriteLine($"- {missing}");
        }
        else Console.WriteLine("All optional files found");

        if (unknown != null)
        {
            Console.WriteLine("Unknown files:");
            foreach (string missing in unknown) Console.WriteLine($"- {missing}");
        }
    }
}
