using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Board : MonoBehaviour
{
        private static readonly KeyCode[] ALL_KEYS = new KeyCode[] {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H,
        KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P,
        KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.Y,
        KeyCode.Z, KeyCode.Y, KeyCode.Z, KeyCode.Quote, KeyCode.Semicolon, KeyCode.LeftBracket

        // Quote corresponds to Æ. Semicolon corresponds to Ø & LeftBacket corresponds to Å
    };

    [SerializeField]
    private string wordToBeGuessed { get; set; }  

    private string[] solutionWords;
    

    private Row[] rows;
    private int rowIndex;
    private int columnIndex;

    // Start is called before the first frame update
    void Start()
    {
        foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            //if ((int)vKey == 145 || (int)vKey == 146)
                print($"{((char)vKey)} + {((int)vKey)}");
        }


        rows = GetComponentsInChildren<Row>();
        LoadData();
        SetRandomWord();
    }

    private void LoadData()
    {
        TextAsset textFile = Resources.Load("solution_words") as TextAsset;
        solutionWords = textFile.text.Split("\n");
    }

    private void SetRandomWord()
    {
        wordToBeGuessed = solutionWords[Random.Range(0, solutionWords.Length)];
        wordToBeGuessed.ToLower().Trim();
    }

    // Update is called once per frame
    void Update()
    {
        Row currentRow = rows[rowIndex];

        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            if(columnIndex > 0)
            {
                columnIndex--;
                rows[rowIndex].tiles[columnIndex].SetLetter('\0');
            }
        }
        else if(Input.GetKeyDown(KeyCode.Return))
        {
            OnSubmitRow(currentRow);
        }

        for (int i = 0; i < ALL_KEYS.Length; i++)
        {
            if(Input.GetKeyDown(ALL_KEYS[i]))
            {
                if(columnIndex < rows[rowIndex].tiles.Length)
                {

                    rows[rowIndex].tiles[columnIndex].SetLetter(ConvertToDanish(ALL_KEYS[i]));
                    
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
            char temp = 'æ';
            return temp;
        }
        else if(keyCode == KeyCode.Semicolon)
        {
            char temp = 'ø';
            return temp;
        }
        else if(keyCode == KeyCode.LeftBracket)
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
        rowIndex++;
        columnIndex = 0;
    }

/*    private char ConvertToDanish(char letter)
    {
        if(letter == (char)KeyCode.Quote)
        {
            char temp = 'æ';
            letter = temp;
        }
        else if(letter == (char)KeyCode.Semicolon)
        {
            char temp = 'ø';
            letter = temp;
        }
        else if(letter == (char)KeyCode.LeftBracket)
        {
            char temp = 'å';
            letter = temp;
        }

        return letter;
    }*/
}
