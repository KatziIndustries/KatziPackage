using System.IO.Compression;
using System.Text;

namespace Katzi.Package;

public class KatziPackage
{
    public Metadata Metadata { get; private set; } = null!;
    public byte[] Logo { get; private set; } = null!;
    public string InstallScript { get; private set; } = null!;

    public string? UninstallScript { get; private set; }
    public string? BuildScript { get; private set; }
    public string? VersionScript { get; private set; }

    public KatziPackage(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"KatziPackage at '{filePath}' could not be found");

        Parse(filePath);
        Verify();
    }

    private void Parse(string filePath)
    {
        ZipArchive archive = ZipFile.OpenRead(filePath);

        foreach (var entry in archive.Entries)
        {
            byte[] content = ExtractZipEntryContents(entry);
            string contentString = Encoding.UTF8.GetString(content);

            switch (entry.FullName)
            {
                case "metadata": Metadata = new(contentString); break;
                case var name when name.StartsWith("logo"): Logo = content; break;
                case "install.sh": InstallScript = contentString; break;
                case "uninstall.sh": UninstallScript = contentString; break;
                case "build.sh": BuildScript = contentString; break;
                case "version.sh": VersionScript = contentString; break;
            }
        }
    }

    private void Verify()
    {
        bool error = false;

        if (Metadata == null) { error = true; Console.WriteLine("Missing metadata"); }
        if (Logo == null) { error = true; Console.WriteLine("Missing logo"); }
        if (InstallScript == null) { error = true; Console.WriteLine("Missing install.sh"); }

        if (error)
            throw new Exception("Files are missing");
    }

    private byte[] ExtractZipEntryContents(ZipArchiveEntry entry)
    {
        Stream stream = entry.Open();

        byte[] buffer = new byte[entry.Length];
        stream.ReadExactly(buffer);

        stream.Dispose();

        return buffer;
    }
}
