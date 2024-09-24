using System.Text;

using CsvHelper;
using CsvHelper.Configuration;
using static System.Console;

namespace csvtocsv;

class Program
{
    static void Main(string[] args)
    {
        if (args == null || args.Length < 1)
        {
            WriteLine("Bad input.");
            return;
        }

        var inputCSV = args[0];

        if (!File.Exists(inputCSV))
        {
            WriteLine("File does not exist.");
            return;
        }

        using (var stream = new StreamReader(inputCSV))
        using (var csv = new CsvReader(stream, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)))
        {
            while (csv.Read())
            {
                var index = 0;
                var buffer = new StringBuilder();
                while (csv.TryGetField<string>(index, out var field))
                {
                    if (index > 0)
                    {
                        buffer.Append(",");
                    }
                    var newfield = field?.Replace('\n', ' ') ?? string.Empty;
                    buffer.Append(newfield);
                    index++;
                }
                WriteLine(buffer.ToString());
            }
        }
    }
}
