using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuessedWordList : MonoBehaviour
{
    public List<CorrectWord> correctWords { get; set; } = new List<CorrectWord>();
    [SerializeField] GameObject correctWord;

    private void Start() 
    {

    }

    public void SpawnCorrectWord(string guessedWord)
    {

        CorrectWord correctWord = Instantiate(this.correctWord, new Vector3(0,0,0), Quaternion.identity).GetComponent<CorrectWord>();
        correctWord.transform.SetParent(this.transform, false);
        correctWord.SetWord(guessedWord);

        if(!correctWords.Contains(correctWord))
        {
            correctWords.Add(correctWord);
        }
    }
}
