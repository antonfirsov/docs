using System.Text;

using CsvHelper;

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
        }

        using (var stream = new StreamReader(inputCSV))
        {
            var reader = new CsvReader(stream);

            while (reader.Read())
            {
                var index = 0;
                var buffer = new StringBuilder();
                while (reader.TryGetField<string>(index, out var field))
                {
                    if (index > 0)
                    {
                        buffer.Append(",");
                    }
                    var newfield = field.Replace('\n', ' ');
                    buffer.Append(newfield);
                    index++;
                }
                WriteLine(buffer.ToString());
            }
        }
    }
}
