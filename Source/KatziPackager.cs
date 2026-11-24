using System.IO.Compression;
using Microsoft.Win32.SafeHandles;

namespace Katzi.Package;

public static class KatziPackager
{
    public static readonly string[] RequiredFiles = [
        "metadata",
        "logo",
        "install.sh",
    ];

    public static readonly string[] OptionalFiles = [
        "uninstall.sh",
        "build.sh",
        "version.sh",
    ];

    public static KatziPackage Parse(string filePath)
    {
        return new(filePath);
    }

    /// <summary>
    /// Tries to create a .kpg from a directory.
    /// This fails if any unknown files are present or any required files are missing.
    /// Returns the path where the resulting .kpg got put or null on failure.
    /// </summary>
    public static string? Create(string directoryPath, string? outputPath = null)
    {
        if (!Directory.Exists(directoryPath))
            throw new DirectoryNotFoundException($"Couldn't find directory at '{directoryPath}'");

        if (GetUnknownFiles(directoryPath) != null ||
            GetMissingRequiredFiles(directoryPath) != null)
            return null;

        string destination = Path.Combine(outputPath ?? Directory.GetParent(directoryPath)!.FullName, GetDirectoryName(directoryPath) + ".kpg");
        ZipFile.CreateFromDirectory(directoryPath, destination);

        return destination;
    }

    /// <summary>
    /// Returns all unknown files or null if none are present
    /// </summary>
    public static string[]? GetUnknownFiles(string directoryPath)
    {
        string[] entries = FormatEntries(directoryPath);

        string[] unknownFiles = [.. entries.Except(RequiredFiles.Concat(OptionalFiles))];

        if (unknownFiles.Length > 0)
            return unknownFiles;

        return null;
    }

    /// <summary>
    /// Returns all missing required files or null if all are present.
    /// </summary>
    public static string[]? GetMissingRequiredFiles(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
            throw new DirectoryNotFoundException($"Couldn't find directory at '{directoryPath}'");

        string[] entries = FormatEntries(directoryPath);

        string[] missingRequiredFiles = [.. RequiredFiles.Except(entries)];

        if (missingRequiredFiles.Length > 0)
            return missingRequiredFiles;

        return null;
    }

    /// <summary>
    /// Returns all missing optional files or null if all are present.
    /// </summary>
    public static string[]? GetMissingOptionalFiles(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
            throw new DirectoryNotFoundException($"Couldn't find directory at '{directoryPath}'");

        string[] entries = FormatEntries(directoryPath);

        string[] missingOptionalFiles = [.. OptionalFiles.Except(entries)];

        if (missingOptionalFiles.Length > 0)
            return missingOptionalFiles;

        return null;
    }

    private static string[] FormatEntries(string directoryPath)
    {
        return [.. Directory.GetFileSystemEntries(directoryPath).Select((s) =>
            {
                if (Directory.Exists(s))
                    return GetDirectoryName(s) + "/";

                string fileName = Path.GetFileName(s);
                if (fileName.StartsWith("logo"))
                    return "logo";

                return fileName;
            })];
    }

    private static string GetDirectoryName(string directoryPath)
    {
        return directoryPath.TrimEnd('/')[(directoryPath.LastIndexOf("/") + 1)..directoryPath.Length];
    }
}
