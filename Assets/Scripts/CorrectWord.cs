using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CorrectWord : MonoBehaviour
{
    private TextMeshProUGUI text;
    public string correctWord { get; private set; }

    private void Awake() 
    {
        text = GetComponentInChildren<TextMeshProUGUI>(); 
    }
    private void Start() 
    {
    }

    public void SetWord(string correctWord)
    {
        this.correctWord = correctWord;
        text.text = correctWord;
    }
}
