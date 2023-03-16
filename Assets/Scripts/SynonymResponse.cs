using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynonymResponse
{
    [Serializable]
    public class Root
    {
        public List<Word> words { get; set; }
    }

    [Serializable]
    public class Word
    {
        public string word { get; set; }
        public List<string> synonyms { get; set; }
    }
}
