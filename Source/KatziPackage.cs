using System.IO.Compression;
using System.Text;
using StbImageSharp;

namespace Katzi.Package;

public class KatziPackage
{
#nullable disable
    public Metadata Metadata { get; private set; }
    public byte[] Logo { get; private set; }
#nullable enable

    public KatziPackage(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"KatziPackage at '{filePath}' could not be found");

        Parse(filePath);
    }

    private void Parse(string filePath)
    {
        ZipArchive archive = ZipFile.OpenRead(filePath);

        foreach (var entry in archive.Entries)
        {
            byte[] content = ExtractZipEntryContents(entry);
            string contentString = Encoding.UTF8.GetString(content);

            if (entry.FullName.Equals("metadata"))
                Metadata = new(contentString);

            if (entry.FullName.StartsWith("logo"))
                Logo = content;
        }
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
