using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynonymResponse
{
    [System.Serializable]
    public class Root
    {
        public List<Word> Words;
    }

    [System.Serializable]
    public class Synonym
    {
        public string Name;
    }

    [System.Serializable]
    public class Word
    {
        public string PrimaryWord;
        public List<Synonym> Synonyms;
    }
}
