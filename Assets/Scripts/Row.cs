using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Row : MonoBehaviour
{
    private float tileScale = 0.8f;

    private HorizontalLayoutGroup transformLayoutGroup;
    private Vector3 localSpawnScale = new Vector3(1,1,1);
    public GameObject tile;
    public List<Tile> tiles { get; set; } = new List<Tile>();

    void Awake()
    {
        transformLayoutGroup = GetComponent<HorizontalLayoutGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetTilesAmount(string word)
    {
        //transformLayoutGroup.spacing = transformLayoutGroup.spacing * spacingScale;

        for (int i = 0; i < word.Length; i++)
        {
            Tile tile = Instantiate(this.tile, new Vector3(0,0,0), Quaternion.identity).GetComponent<Tile>();
            tile.transform.SetParent(this.transform, false);

            if(word.Length > 10 && word.Length <= 14)
            {
                tile.transform.localScale = new Vector3(tileScale,tileScale,tileScale);
            }
            else if(word.Length > 14)
            {
                tile.transform.localScale = new Vector3(0.6f,0.6f,0.6f);
            }
            else{
                tile.transform.localScale = new Vector3(1f,1f,1f);
            }

            tiles.Add(tile);
        }

        if(word.Length > 10){
            transformLayoutGroup.spacing = 8f;
        }
    }
}
