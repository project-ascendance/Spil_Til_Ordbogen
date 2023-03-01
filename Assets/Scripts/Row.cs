using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    public Tile[] tiles { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        tiles = GetComponentsInChildren<Tile>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
