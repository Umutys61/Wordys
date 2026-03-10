using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class WordManager : MonoBehaviour
{
    [System.Serializable]
    public struct WordListEntry
    {
        public int length;
        public TextAsset file;
    }

    [Header("Word Sources")]
    [SerializeField] private WordListEntry[] wordLists;

    private HashSet<string> dictionary;
    private string targetWord;
    private int currentLength;

    private readonly CultureInfo turkishCulture = new CultureInfo("tr-TR");

    public string TargetWord => targetWord;

    public void LoadWords(int length)
    {
        currentLength = length;
        dictionary = new HashSet<string>();

        TextAsset file = null;

        foreach (var entry in wordLists)
        {
            if (entry.length == length)
            {
                file = entry.file;
                break;
            }
        }

        if (file == null)
        {
            Debug.LogError($"No word list found for length {length}");
            return;
        }

        var lines = file.text.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            string word = line.Trim().ToUpper(turkishCulture);
            word = word.Replace("\r", "").Trim();

            if (word.Length == length)
                dictionary.Add(word);
        }

        Debug.Log($"Loaded {dictionary.Count} words for length {length}");
    }

    public void SelectRandomWord()
    {
        if (dictionary == null || dictionary.Count == 0)
            return;

        int index = Random.Range(0, dictionary.Count);
        int i = 0;

        foreach (var word in dictionary)
        {
            if (i == index)
            {
                targetWord = word;
                break;
            }
            i++;
        }

        Debug.Log("TARGET WORD: " + targetWord);
    }

    public bool IsValidWord(string guess)
    {
        return dictionary.Contains(guess.ToUpper(turkishCulture));
    }

    public LetterState[] CheckGuess(string guess)
    {
        guess = guess.ToUpper(turkishCulture);

        int length = targetWord.Length;
        LetterState[] result = new LetterState[length];
        bool[] used = new bool[length];

        for (int i = 0; i < length; i++)
        {
            if (guess[i] == targetWord[i])
            {
                result[i] = LetterState.Correct;
                used[i] = true;
            }
            else
            {
                result[i] = LetterState.None;
            }
        }

        for (int i = 0; i < length; i++)
        {
            if (result[i] != LetterState.None)
                continue;

            for (int j = 0; j < length; j++)
            {
                if (used[j])
                    continue;

                if (guess[i] == targetWord[j])
                {
                    result[i] = LetterState.WrongPlace;
                    used[j] = true;
                    break;
                }
            }
        }

        for (int i = 0; i < length; i++)
        {
            if (result[i] == LetterState.None)
                result[i] = LetterState.NotInWord;
        }

        return result;
    }
}