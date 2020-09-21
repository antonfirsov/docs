namespace BlogValidator
{
    internal class Diagnostic
    {
        public Diagnostic(bool isWarning, string id, string fileName, int line, int column, string message)
        {
            IsWarning = isWarning;
            Id = id;
            FileName = fileName;
            Line = line;
            Column = column;
            Message = message;
        }

        public bool IsWarning { get; }
        public string Id { get; }
        public string FileName { get; }
        public int Line { get; }
        public int Column { get; }
        public string Message { get; }
    }
}
