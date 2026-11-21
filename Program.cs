using Katzi.Package;
using SDL3;
using SmashFramework;

internal static class Program
{
    public static void Main(string[] args)
    {
        KatziPackage katziPackage = new("KatziPackage.kpg");

        Console.WriteLine($"Name: {katziPackage.Metadata.Name}");
        Console.WriteLine($"Description: {katziPackage.Metadata.Description}");
        Console.WriteLine($"Version: {katziPackage.Metadata.Version}");
        Console.WriteLine($"Type: {katziPackage.Metadata.Type}");
        Console.WriteLine($"Url: {katziPackage.Metadata.Url}");

        if (katziPackage.Metadata.Dependencies.Length > 0)
        {
            Console.WriteLine("Dependencies:");

            foreach (string dependency in katziPackage.Metadata.Dependencies)
                Console.WriteLine($"- {dependency}");
        }
        else
            Console.WriteLine("Package has no dependencies");
    }
}
