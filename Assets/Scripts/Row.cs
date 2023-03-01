using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    private Vector3 localSpawnScale = new Vector3(1,1,1);
    public GameObject tile;
    public List<Tile> tiles { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        tiles = new List<Tile>();
    }

    public void SetTilesAmount(string word)
    {
        for (int i = 0; i < word.Length; i++)
        {
            Tile tile = Instantiate(this.tile, new Vector3(0,0,0), Quaternion.identity).GetComponent<Tile>();
            tile.transform.SetParent(this.transform, false);
            tiles.Add(tile);
        }
    }
}
