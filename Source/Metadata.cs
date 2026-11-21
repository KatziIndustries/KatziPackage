namespace Katzi.Package;

public class Metadata
{
#nullable disable
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Version { get; private set; }
    public KatziPackageType Type { get; private set; }
    public string Url { get; private set; }
    public string[] Dependencies { get; private set; } = [];
#nullable enable

    public Metadata(string metadata)
    {
        ParseMetadata(metadata);
    }

    private void ParseMetadata(string metadata)
    {
        string[] lines = metadata.Split("\n");
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];

            if (line == string.Empty)
                continue;

            int firstColon = line.IndexOf(':');

            if (firstColon == -1)
                throw new InvalidDataException($"Metadata wasn't in the right format; No colon (:) found [Line {i + 1}]");

            string value = line[(firstColon + 1)..line.Length].Trim();

            switch (line[0..firstColon].Trim())
            {
                case "Name": Name = value; break;
                case "Description": Description = value; break;
                case "Version": Version = value; break;
                case "Type": Type = Enum.Parse<KatziPackageType>(value); break;
                case "Url": Url = value; break;
                case "Depends": Dependencies = ParseDependencies(value); break;
            }
        }
    }

    private string[] ParseDependencies(string dependenciesString)
    {
        return [.. dependenciesString.Split(',').Select(s => s.Trim())];
    }
}
