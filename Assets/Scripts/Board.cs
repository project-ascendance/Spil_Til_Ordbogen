using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
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
    [DisplayName("Word to be guessed")]
    private string wordToBeGuessed { get; set; }  

    private string[] solutionWords;
    
    private Row[] rows;
    private int rowIndex;
    private int columnIndex;

    [Header("States")]
    public Tile.State emptyState;
    public Tile.State correctState;
    public Tile.State occupiedState;
    public Tile.State wrongPositionState;
    public Tile.State incorrectState;

    private void Awake() {
        rows = GetComponentsInChildren<Row>();
    }
    void Start()
    {
        LoadData();
        SetRandomWord();
        SetTileAmount();
    }

    private void LoadData()
    {
        TextAsset textFile = Resources.Load("solution_words") as TextAsset;
        solutionWords = textFile.text.Split("\n");
    }

    private void SetRandomWord()
    {
        string tempWord = solutionWords[Random.Range(0, solutionWords.Length)];
        wordToBeGuessed = tempWord.ToLower().Trim();
    }

    private void SetTileAmount()
    {
        for (int i = 0; i < rows.Length; i++)
        {
            rows[i].SetTilesAmount(wordToBeGuessed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        THIS IS USED FOR FINDING THE CORRESPONDING KEYS FROM AN AZERTY LAYOUT TO QWERTY
         foreach (KeyCode keycode in KeyCode.GetValues (typeof (KeyCode)))
         {
             if (Input.GetKeyDown (keycode))
             {
                 Debug.Log ("Qwerty key is : " + keycode );
             }
         }
         */

        Row currentRow = rows[rowIndex];

        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            if(columnIndex > 0)
            {
                columnIndex--;
                currentRow.tiles[columnIndex].SetLetter('\0');
                currentRow.tiles[columnIndex].SetState(emptyState);
            }
        }
        else if(Input.GetKeyDown(KeyCode.Return))
        {
            if(columnIndex == currentRow.tiles.Count)
            {
                OnSubmitRow(currentRow);
            }
            else{
                Debug.Log("You can't submit before the whole row is filled");
            }
        }

        for (int i = 0; i < ALL_KEYS.Length; i++)
        {
            if(Input.GetKeyDown(ALL_KEYS[i]))
            {
                if(columnIndex < currentRow.tiles.Count)
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

    private char ConvertToDanish(KeyCode keyCode)
    {
        if(keyCode == KeyCode.Quote)
        {
            char temp = 'ø';
            return temp;
        }
        else if(keyCode == KeyCode.BackQuote)
        {
            char temp = 'æ';
            return temp;
        }
        else if(keyCode == KeyCode.RightBracket)
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

            if(tile.letter == wordToBeGuessed[i])
            {
                tile.SetState(correctState);
                remaining = remaining.Remove(i, 1);
                remaining = remaining.Insert(i, " ");
            }
            else if(!wordToBeGuessed.Contains(tile.letter))
            {
                tile.SetState(incorrectState); 
            }
        }

        for (int i = 0; i < submitRow.tiles.Count; i++)
        {
            Tile tile = submitRow.tiles[i];

            if(tile.state != correctState && tile.state != incorrectState)
            {
                if(remaining.Contains(tile.letter))
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
        if(rowIndex >= rows.Length){
            enabled = false;
        }

        // Check if the row submitted is the wordTobeGuessed
        if(HasWon(submitRow))
        {
            enabled = false;
            Debug.Log("YOU WIN");
        }
    }

    private bool HasWon(Row row)
    {
        for (int i = 0; i < row.tiles.Count; i++)
        {
            if(row.tiles[i].state != correctState)
            {
                return false;
            }   
        }

        return true;
    }
}
