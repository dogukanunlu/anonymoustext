namespace Backend.Services;

using System.Collections.Concurrent;
using System.IO;

using System.Collections.Concurrent;

public class BadWordsService
{
    private readonly string _filePath = "badwords.txt";
    private readonly ConcurrentDictionary<string, byte> _badWords = new();
    private FileSystemWatcher _watcher;

    public BadWordsService()
    {
        LoadBadWords();
        SetupFileWatcher();
    }

    public bool ContainsBadWord(string content)
    {
        return _badWords.Keys.Any(word => content.Contains(word, StringComparison.OrdinalIgnoreCase));
    }

    private void LoadBadWords()
    {
        if (!File.Exists(_filePath)) return;

        var badWords = File.ReadLines(_filePath)
            .Select(line => line.Trim())
            .Where(line => !string.IsNullOrEmpty(line));

        foreach (var word in badWords)
        {
            _badWords.TryAdd(word, 0); // Add the word to the dictionary
        }
    }

    private void SetupFileWatcher()
    {
        var directory = Path.GetDirectoryName(_filePath);
        var fileName = Path.GetFileName(_filePath);

        _watcher = new FileSystemWatcher(directory, fileName)
        {
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
            EnableRaisingEvents = true
        };

        _watcher.Changed += (sender, args) => ReloadBadWords();
    }

    private void ReloadBadWords()
    {
        _badWords.Clear();
        LoadBadWords();
    }
}
