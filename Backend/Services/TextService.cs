using System.Diagnostics;
using Backend.Models;
using System.Text.Json;
using System.IO;

namespace Backend.Services
{
    public class TextService
    {
        private readonly string _submissionsFile = "Data/submissions.txt";
        private readonly string _badWordsFile = "Data/badwords.txt";
        private readonly List<string> _badWords = new();
        private readonly Mutex _fileMutex = new();

        public TextService()
        {
            // Load bad words at startup
            if (File.Exists(_badWordsFile))
                _badWords.AddRange(File.ReadAllLines(_badWordsFile));
            System.Diagnostics.Debug.WriteLine("Bad words loaded:");
            
            
            
            

            // Monitor the bad words file for updates
            var watcher = new FileSystemWatcher(Path.GetDirectoryName(_badWordsFile)!, Path.GetFileName(_badWordsFile))
            {
                NotifyFilter = NotifyFilters.LastWrite
            };
            watcher.Changed += (_, __) => LoadBadWords();
            watcher.EnableRaisingEvents = true;
            foreach (var word in _badWords)
            {
                System.Diagnostics.Debug.WriteLine(word);
            }
        }

        private void LoadBadWords()
        {
            lock (_badWords)
            {
                _badWords.Clear();
                _badWords.AddRange(File.ReadAllLines(_badWordsFile));
                foreach (var word in _badWords)
                {
                    System.Diagnostics.Debug.WriteLine(word);
                }
            }
        }

        public bool ContainsBadWords(string content)
        {
            lock (_badWords)
            {
                return _badWords.Any(badWord => content.Contains(badWord, StringComparison.OrdinalIgnoreCase));
            }
        }

        public void AddSubmission(Submission submission)
        {
            _fileMutex.WaitOne();
            try
            {
                var json = JsonSerializer.Serialize(submission) + Environment.NewLine;
                File.AppendAllText(_submissionsFile, json);
            }
            finally
            {
                _fileMutex.ReleaseMutex();
            }
        }

        public List<Submission> GetSubmissions(int start, int count)
        {
            _fileMutex.WaitOne();
            try
            {
                var lines = File.Exists(_submissionsFile) ? File.ReadAllLines(_submissionsFile) : Array.Empty<string>();
                return lines
                    .Skip(start)
                    .Take(count)
                    .Select(line => JsonSerializer.Deserialize<Submission>(line)!)
                    .ToList();
            }
            finally
            {
                _fileMutex.ReleaseMutex();
            }
        }
    }
}
