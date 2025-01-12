using Backend.Models;
using System.Text.Json;
using Ganss.Xss;

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
            if (File.Exists(_badWordsFile))
            {
                _badWords.AddRange(File.ReadAllLines(_badWordsFile).Select(word => word.Trim())); // Trim bad words
            }

            var watcher = new FileSystemWatcher(Path.GetDirectoryName(_badWordsFile)!, Path.GetFileName(_badWordsFile))
            {
                NotifyFilter = NotifyFilters.LastWrite
            };
            watcher.Changed += (_, __) => LoadBadWords();
            watcher.EnableRaisingEvents = true;
        }

        private void LoadBadWords()
        {
            lock (_badWords)
            {
                _badWords.Clear();
                if (File.Exists(_badWordsFile))
                {
                    _badWords.AddRange(File
                        .ReadAllLines(_badWordsFile)
                        .Select(word => word.Trim())
                        .Where(word => !string.IsNullOrEmpty(word)));
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
            var sanitizer = new HtmlSanitizer();
            submission.Content = sanitizer.Sanitize(submission.Content);

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

                var reversedLines = lines.Reverse(); 

                return reversedLines
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
