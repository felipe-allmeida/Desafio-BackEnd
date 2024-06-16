using BikeRental.Application.Extensions;
using System.Text.RegularExpressions;

namespace BikeRental.Application.Services
{
    public interface IExcelService
    {
        List<Dictionary<string, string>> Get(string path);
        List<T> Get<T>(string path, Func<string[], string[], T> rowCallback);
        void Write(List<Dictionary<string, string>> data, string path);
    }
    public class ExcelService : IExcelService
    {
        private readonly char _delimiter = ',';

        public ExcelService(char delimiter = ',')
        {
            _delimiter = delimiter;
        }

        public List<Dictionary<string, string>> Get(string path)
        {
            string[] headers = ValidateHeaders(path);

            return File.ReadAllLines(path)
                .Skip(1)
                .Select(row => Regex.Split(row, _delimiter + "(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                .SelectTry(collumn => GetRow(headers, collumn))
                .OnCaughtException(ex => { Console.WriteLine(ex.Message); return null; })
                .Where(x => x != null).ToList();
        }

        public List<T> Get<T>(string path, Func<string[], string[], T> rowCallback)
        {
            string[] headers = ValidateHeaders(path);

            return File.ReadAllLines(path)
                .Skip(1)
                .Select(row => Regex.Split(row, _delimiter + "(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"))
                .SelectTry(collumn => rowCallback(headers, collumn))
                .OnCaughtException(ex => { Console.WriteLine(ex.Message); return default; })
                .Where(x => x != null).ToList();
        }

        public void Write(List<Dictionary<string, string>> data, string path)
        {
            using StreamWriter sw = new StreamWriter(path);

            var headers = string.Join(_delimiter, data.First().Keys);

            sw.WriteLine(headers);

            foreach (var row in data)
            {
                var line = string.Join(_delimiter, row.Values);
                sw.WriteLine(line);
            }
        }

        private string[] ValidateHeaders(string csvFile)
        {
            return File.ReadLines(csvFile).First().Split(_delimiter);
        }

        private Dictionary<string, string> GetRow(string[] headers, string[] collumn)
        {
            if (collumn.Length != headers.Length)
            {
                throw new Exception($"column count '{collumn.Length}' not the same as headers count'{headers.Length}'");
            }

            var row = new Dictionary<string, string>();

            foreach (var header in headers)
            {
                row.Add(header, collumn[Array.IndexOf(headers, header)].Trim('"').Trim());
            }

            return row;
        }
    }
}
