using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DifficultySelector : MonoBehaviour
{
    [SerializeField]
    Button easyButton;
    [SerializeField]
    Button mediumButton;
    [SerializeField]
    Button hardButton;

    public static DifficultySelector Instance;
    public static readonly string easy = "EASY";
    public static readonly string medium = "MEDIUM";
    public static readonly string hard = "HARD";

    public string difficulty { get; set; }

    public static event Action<string> OnDifficultyChanged;

    private void Awake() 
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        mediumButton.interactable = false;
        UpdateDifficulty(medium);
    }

    public void SetDifficultyEasy()
    {
        easyButton.interactable = false;
        mediumButton.interactable = true;
        hardButton.interactable = true;

        UpdateDifficulty(easy);
    }

    public void SetDifficultyMedium()
    {
        easyButton.interactable = true;
        mediumButton.interactable = false;
        hardButton.interactable = true;

        UpdateDifficulty(medium);
    }

    public void SetDifficultyHard()
    {
        easyButton.interactable = true;
        mediumButton.interactable = true;
        hardButton.interactable = false;

        UpdateDifficulty(hard);
    }

    public void UpdateDifficulty(string newDifficulty)
    {
        difficulty = newDifficulty;
        OnDifficultyChanged?.Invoke(newDifficulty);
    }
}
