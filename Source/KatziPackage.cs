using System.IO.Compression;
using System.Text;

namespace Katzi.Package;

public class KatziPackage : IDisposable
{
    public readonly Metadata Metadata;
    public readonly byte[] Logo;
    public DirectoryInfo Files;

    public readonly string? PreInstallScript;
    public readonly string? PostInstallScript;

    public readonly string TemporaryFilePath;

    /// <summary>
    /// Creating an instance of this class will extract the contents of the .kpg into /tmp 
    /// and provide access to it as long as the instance is alive
    /// </summary>
    public KatziPackage(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"KatziPackage at '{filePath}' could not be found");

        TemporaryFilePath = Path.Combine("/tmp", "kpm-temp-" + Path.GetFileNameWithoutExtension(filePath));
        if (Directory.Exists(TemporaryFilePath))
        {
            throw new Exception($"There's already a directory at {TemporaryFilePath}; exiting");
        }

        ZipArchive archive = new(new FileStream(filePath, FileMode.Open));
        archive.ExtractToDirectory(TemporaryFilePath);

        Logo = GetLogo();
        Metadata = new(File.ReadAllText(Path.Combine(TemporaryFilePath, "metadata")));
        Files = new(Path.Combine(TemporaryFilePath, "files"));

        string preinstallPath = Path.Combine(TemporaryFilePath, "preinstall.sh");
        if (File.Exists(preinstallPath))
            PreInstallScript = File.ReadAllText(preinstallPath);

        string postinstallPath = Path.Combine(TemporaryFilePath, "postinstall.sh");
        if (File.Exists(postinstallPath))
            PostInstallScript = File.ReadAllText(postinstallPath);
    }

    private byte[] GetLogo()
    {
        string logoPath = "";
        foreach (string file in Directory.GetFiles(TemporaryFilePath))
        {
            if (Path.GetFileName(file).StartsWith("logo"))
            {
                logoPath = file;
                break;
            }
        }

        if (logoPath == string.Empty)
        {
            throw new FileNotFoundException("Failed to find logo in .kpg");
        }

        return File.ReadAllBytes(logoPath);
    }

    //private void Parse(string filePath)
    //{
    //    ZipArchive archive = ZipFile.OpenRead(filePath);

    //    foreach (var entry in archive.Entries)
    //    {
    //        byte[] content = ExtractZipEntryContents(entry);
    //        string contentString = Encoding.UTF8.GetString(content);

    //        switch (entry.FullName)
    //        {
    //            case "metadata": Metadata = new(contentString); break;
    //            case var name when name.StartsWith("logo"): Logo = content; break;
    //            case "files/": Files = new(); break;
    //            case "install.sh": InstallScript = contentString; break;
    //            case "uninstall.sh": UninstallScript = contentString; break;
    //            case "build.sh": BuildScript = contentString; break;
    //            case "version.sh": VersionScript = contentString; break;
    //        }
    //    }
    //}

    //private void Verify()
    //{
    //    bool error = false;

    //    if (Metadata == null) { error = true; Console.WriteLine("Missing metadata"); }
    //    if (Logo == null) { error = true; Console.WriteLine("Missing logo"); }
    //    if (InstallScript == null) { error = true; Console.WriteLine("Missing install.sh"); }

    //    if (error)
    //        throw new Exception("Files are missing");
    //}

    //private byte[] ExtractZipEntryContents(ZipArchiveEntry entry)
    //{
    //    Stream stream = entry.Open();

    //    byte[] buffer = new byte[entry.Length];
    //    stream.ReadExactly(buffer);

    //    stream.Dispose();

    //    return buffer;
    //}

    ~KatziPackage()
    {
        Dispose();
    }

    public void Dispose()
    {
        Directory.Delete(TemporaryFilePath, true);
    }
}
