using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynonymResponse
{
    [Serializable]
    public class Root
    {
        public List<Word> words;
    }

    [Serializable]
    public class Word
    {
        public string word;
        public List<string> synonyms;
    }
}
