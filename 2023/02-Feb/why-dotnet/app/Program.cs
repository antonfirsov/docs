Console.WriteLine("Code samples for post");
var file = "/Users/rich/files.txt";

try
{
    var lines = await File.ReadAllLinesAsync(file);
    Console.WriteLine($"The {file} has {lines.Length}");
}
catch (Exception e) when (e is FileNotFoundException or DirectoryNotFoundException)
{
    Console.WriteLine($"{file} doesn't exist.");
}

var someInfo = "important information";
var moreInfo = "more important information";
List<string> data = new()
{
    someInfo,
    moreInfo
};

foreach (Type type in typeof(Program).Assembly.DefinedTypes)
{
    if (type.IsAssignableTo(typeof(IStory)) &&
        !type.IsInterface)
    {
        IStory? story = (IStory?)Activator.CreateInstance(type);
        if (story is not null)
        {
            var text = story.TellMeAStory();
            Console.WriteLine(text);
        }

    }
}

interface IStory
{
    string TellMeAStory();
}

class BedTimeStory : IStory
{
    public string TellMeAStory() => "Once upon a time, there was an orphan who knew magic ...";
}

class HorrorStory : IStory
{
    public string TellMeAStory() => "I was alone, the lights went out, and I heard steps on the stairs behind me ...";
}
