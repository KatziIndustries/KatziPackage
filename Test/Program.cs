using Katzi.Package;

internal static class Program
{
    public static void Main(string[] args)
    {
        KatziPackage katziPackage = new("/home/katzi/KatziPackage.kpg");

        Console.WriteLine($"Name: {katziPackage.Metadata.Name}");
        Console.WriteLine($"Description: {katziPackage.Metadata.Dependencies}");
        Console.WriteLine($"Version: {katziPackage.Metadata.Version}");
        Console.WriteLine($"Type: {katziPackage.Metadata.Type}");
        Console.WriteLine($"Url: {katziPackage.Metadata.Url}");

        if (katziPackage.Metadata.Dependencies.Length > 0)
        {
            Console.WriteLine("Dependencies:");
            foreach (string dependency in katziPackage.Metadata.Dependencies)
                Console.WriteLine($"- {dependency}");
        }
        else Console.WriteLine("Package has no dependencies");

        Console.WriteLine("Files:");
        foreach (var file in katziPackage.Files.EnumerateFileSystemInfos())
        {
            Console.WriteLine($"- {file.FullName}");
        }

        Console.ReadLine();

        katziPackage.Dispose();
    }
}
