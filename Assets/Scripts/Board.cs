using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    private static readonly KeyCode[] ALL_KEYS = new KeyCode[] {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H,
        KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P,
        KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.Y,
        KeyCode.Z, KeyCode.Y, KeyCode.Z, KeyCode.Quote, KeyCode.RightBracket, KeyCode.BackQuote

        // Quote corresponds to Æ. Semicolon corresponds to Ø & LeftBacket corresponds to Å
    };

    [SerializeField]
    private TextMeshProUGUI primaryWordUI;

    [SerializeField]
    GameObject primaryWordGameObject;
    // Managing contact to the GuessedWordList script.
    GuessedWordList guessedWordList;
    [SerializeField] GameObject guessedWordListGameObject;

    DifficultySelector difficultySelector;
    [SerializeField] GameObject difficultiesGameObject;

    // JSON file gets added in UI.
    public TextAsset textJSON;

    [SerializeField]
    [DisplayName("Word to be guessed")]
    private string wordToBeGuessed { get; set; }
    private string primaryWord;

    private string[] solutionWords;

    public SynonymResponse.Root synonymResponse;

    [SerializeField]
    public List<string> synonymList;

    [SerializeField]
    private int rowAmount;

    public List<string> correctWords = new List<string>();

    //public GuessedWordList guessedWordLists;
    public List<Row> rows { get; set; } = new List<Row>();
    public GameObject rowPrefab;

    private int rowIndex = 0;
    private int columnIndex;

    [Header("States")]
    public Tile.State emptyState;
    public Tile.State correctState;
    public Tile.State occupiedState;
    public Tile.State wrongPositionState;
    public Tile.State incorrectState;

    [Header("UI")]
    public Button tryAgainButton;
    public Button nextWordButton;
    public Button newGameButton;

    private string chosendifficulty;

    private void Awake()
    {
        DifficultySelector.OnDifficultyChanged += DifficultySelectorOnDifficultyChanged;

        guessedWordList = guessedWordListGameObject.GetComponent<GuessedWordList>();
        primaryWordUI = primaryWordGameObject.GetComponent<TextMeshProUGUI>();
        difficultySelector = difficultiesGameObject.GetComponent<DifficultySelector>();
    }

    private void OnDestroy()
    {
        DifficultySelector.OnDifficultyChanged -= DifficultySelectorOnDifficultyChanged;
        Debug.Log("UNSUBBED");
    }

    private void DifficultySelectorOnDifficultyChanged(string difficulty)
    {
        chosendifficulty = difficulty;
        NewGame();
    }

    void Start()
    {
        LoadData();
        NewGame();
    }

    public void TryAgain()
    {
        ClearBoardTryAgain();
        enabled = true;
    }

    public void NextWord()
    {
        tryAgainButton.interactable = true;

        if (synonymList.Count > 0)
        {
            string tempWord = synonymList[Random.Range(0, synonymList.Count)];

            wordToBeGuessed = tempWord.ToLower().Trim();

            ClearBoardNextWord();
            enabled = true;
        }
        else
        {
            Debug.Log("You guessed all the synonyms!");
        }
    }

    public void NewGame()
    {
        Debug.Log("The difficulty is: " + chosendifficulty);
        nextWordButton.interactable = true;
        tryAgainButton.interactable = true;

        SetRandomWord(chosendifficulty);
        ClearBoardNewGame();
        enabled = true;
    }

    private void LoadData()
    {
        synonymResponse = JsonUtility.FromJson<SynonymResponse.Root>(textJSON.text);

        // OLD CODE
        //TextAsset textFile = Resources.Load("solution_words.txt") as TextAsset;
        //solutionWords = textFile.text.Split("\n");
    }

    private void SetRandomWord(string difficulty)
    {
        List<string> tempWordList = new List<string>();

        int randomIndex = Random.Range(0, synonymResponse.words.Count);

        while (PopulateSynonymsFromDifficulty(difficulty, randomIndex).Count == 0)
        {
            randomIndex = Random.Range(0, synonymResponse.words.Count);
            //PopulateSynonymsFromDifficulty(difficulty, randomIndex);
        }

        primaryWord = synonymResponse.words[randomIndex].word;
        primaryWordUI.text = primaryWord;

        /*for (int i = 0; i < synonymResponse.words[randomIndex].synonyms.Count; i++)
        {
            synonymList.Add(synonymResponse.words[randomIndex].synonyms[i].ToLower().Trim());
        }
        */

        string tempWord = synonymList[Random.Range(0, synonymList.Count)];

        if (wordToBeGuessed != string.Empty)
        {
            while (tempWord == wordToBeGuessed)
            {
                tempWord = synonymList[Random.Range(0, synonymList.Count)];
                tempWord.ToLower().Trim();
            }
        }

        wordToBeGuessed = tempWord.ToLower().Trim();
    }

    private List<string> PopulateSynonymsFromDifficulty(string difficulty, int randomIndex)
    {
        synonymList = new List<string>();

            if (difficulty == "EASY")
            {
                for (int i = 0; i < synonymResponse.words[randomIndex].synonyms.Count; i++)
                {
                    if (synonymResponse.words[randomIndex].synonyms[i].Length <= 6)
                    {
                        synonymList.Add(synonymResponse.words[randomIndex].synonyms[i].ToLower().Trim());
                    }
                }
            }
            else if (difficulty == "MEDIUM")
            {
                for (int i = 0; i < synonymResponse.words[randomIndex].synonyms.Count; i++)
                {
                    if (synonymResponse.words[randomIndex].synonyms[i].Length <= 10)
                    {
                        synonymList.Add(synonymResponse.words[randomIndex].synonyms[i].ToLower().Trim());
                    }
                }
            }
            else
            {
                for (int i = 0; i < synonymResponse.words[randomIndex].synonyms.Count; i++)
                {
                    synonymList.Add(synonymResponse.words[randomIndex].synonyms[i].ToLower().Trim());
                }
            }

        return synonymList;
    }

    private void SetTileAmount()
    {
        for (int i = 0; i < rows.Count; i++)
        {
            rows[i].SetTilesAmount(wordToBeGuessed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        //THIS IS USED FOR FINDING THE CORRESPONDING KEYS FROM AN AZERTY LAYOUT TO QWERTY
         foreach (KeyCode keycode in KeyCode.GetValues (typeof (KeyCode)))
         {
             if (Input.GetKeyDown (keycode))
             {
                 Debug.Log ("Qwerty key is : " + keycode );
             }
         }
         */

        Row currentRow = rows[rowIndex];

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (columnIndex > 0)
            {
                columnIndex--;
                currentRow.tiles[columnIndex].SetLetter('\0');
                currentRow.tiles[columnIndex].SetState(emptyState);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (columnIndex == currentRow.tiles.Count)
            {
                OnSubmitRow(currentRow);
            }
            else
            {
                Debug.Log("You can't submit before the whole row is filled");
            }
        }

        for (int i = 0; i < ALL_KEYS.Length; i++)
        {
            if (Input.GetKeyDown(ALL_KEYS[i]))
            {
                if (columnIndex < currentRow.tiles.Count)
                {
                    //currentRow.tiles[columnIndex].SetLetter(ConvertToDanish(ALL_KEYS[i]));
                    currentRow.tiles[columnIndex].SetLetter(ConvertToDanish(ALL_KEYS[i]));
                    currentRow.tiles[columnIndex].SetState(occupiedState);
                    columnIndex++;

                    // Making sure, that if the player spams keys, it can't mess up the indexes
                    break;
                }
            }
        }
    }

    private void ClearBoardTryAgain()
    {
        for (int row = 0; row < rows.Count; row++)
        {
            for (int col = 0; col < rows[row].tiles.Count; col++)
            {
                rows[row].tiles[col].SetLetter('\0');
                rows[row].tiles[col].SetState(emptyState);
            }
        }

        rowIndex = 0;
        columnIndex = 0;
    }

    private void ClearBoardNextWord()
    {
        foreach (Row row in rows)
        {
            foreach (Tile tile in row.tiles)
            {
                Destroy(tile.gameObject);
            }

            row.tiles.Clear();

            Destroy(row.gameObject);
        }

        rows.Clear();

        rowIndex = 0;
        columnIndex = 0;

        SetRowAmount();
        SetTileAmount();
    }

    public void ClearBoardNewGame()
    {
        foreach (Row row in rows)
        {
            foreach (Tile tile in row.tiles)
            {
                Destroy(tile.gameObject);
            }

            row.tiles.Clear();

            Destroy(row.gameObject);
        }

        rows.Clear();

        rowIndex = 0;
        columnIndex = 0;

        SetRowAmount();
        SetTileAmount();

        foreach (CorrectWord item in guessedWordList.correctWords)
        {
            Destroy(item.gameObject);
        }

        guessedWordList.correctWords.Clear();
    }

    private char ConvertToDanish(KeyCode keyCode)
    {
        if (keyCode == KeyCode.Quote)
        {
            char temp = 'ø';
            return temp;
        }
        else if (keyCode == KeyCode.BackQuote)
        {
            char temp = 'æ';
            return temp;
        }
        else if (keyCode == KeyCode.RightBracket)
        {
            char temp = 'å';
            return temp;
        }
        else
        {
            return ((char)keyCode);
        }
    }

    private void OnSubmitRow(Row submitRow)
    {
        string remaining = wordToBeGuessed;

        for (int i = 0; i < submitRow.tiles.Count; i++)
        {
            Tile tile = submitRow.tiles[i];

            if (tile.letter == wordToBeGuessed[i])
            {
                tile.SetState(correctState);
                remaining = remaining.Remove(i, 1);
                remaining = remaining.Insert(i, " ");
            }
            else if (!wordToBeGuessed.Contains(tile.letter))
            {
                tile.SetState(incorrectState);
            }
        }

        for (int i = 0; i < submitRow.tiles.Count; i++)
        {
            Tile tile = submitRow.tiles[i];

            if (tile.state != correctState && tile.state != incorrectState)
            {
                if (remaining.Contains(tile.letter))
                {
                    tile.SetState(wrongPositionState);

                    int index = remaining.IndexOf(tile.letter);
                    remaining = remaining.Remove(index, 1);
                    remaining = remaining.Insert(index, " ");
                }
                else
                {
                    tile.SetState(incorrectState);
                }
            }
        }

        rowIndex++;
        columnIndex = 0;

        // Check if the player reached the last row without guessing the word. Disables the script.
        if (rowIndex >= rows.Count)
        {
            enabled = false;
        }

        // Check if the row submitted is the wordTobeGuessed
        if (HasWon(submitRow))
        {

            if (synonymList.Contains(wordToBeGuessed))
            {
                synonymList.Remove(wordToBeGuessed.ToLower().Trim());
            }

            if (LastSynonym())
            {
                nextWordButton.interactable = false;
            }

            tryAgainButton.interactable = false;
            SetGuessedWord(wordToBeGuessed);
            enabled = false;
            Debug.Log("YOU WIN");
        }
    }

    public void SetGuessedWord(string correctWord)
    {
        guessedWordList.SpawnCorrectWord(correctWord);
    }

    public bool LastSynonym()
    {
        if (synonymList.Count == 0)
        {
            return true;
        }

        return false;
    }

    private bool HasWon(Row row)
    {
        for (int i = 0; i < row.tiles.Count; i++)
        {
            if (row.tiles[i].state != correctState)
            {
                return false;
            }
        }

        return true;
    }

    public void SetRowAmount()
    {
        for (int i = 0; i < rowAmount; i++)
        {
            Row row = Instantiate(this.rowPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Row>();
            row.transform.SetParent(this.transform, false);

            rows.Add(row);
        }
    }

    private void OnEnable()
    {
        tryAgainButton.gameObject.SetActive(false);
        nextWordButton.gameObject.SetActive(false);
        newGameButton.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        tryAgainButton.gameObject.SetActive(true);
        nextWordButton.gameObject.SetActive(true);
        newGameButton.gameObject.SetActive(true);
    }
}
