using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VillainNames.IO
{
    internal class SqlReader : IReader
    {
        public SqlReader(string filename)
        {
            this.FileName = filename;
        }
        public string FileName { get; }
        public string Read()
        {
            string currDirectoryPath = Directory.GetCurrentDirectory();
            string fullPath = Path.Combine(currDirectoryPath, $"../Queries/{this.FileName}.sql");

            string query = File.ReadAllText(fullPath);
            return query;
        }
    }
}
