using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class tiles : MonoBehaviour
{
    public Grid grid;
    public Tilemap tilemap;
    public Tile[] tilesToPlace;
    public int radius = 6;

    void Update()
    {
        var centerCell = grid.WorldToCell(Vector3.zero);
        float scale = 1f/10f;
        for (int x = centerCell.x-radius; x <= centerCell.x+radius; x++)
        {
            for (int y = centerCell.y-radius; y <= centerCell.y+radius; y++)
            {
                Vector3Int p = new Vector3Int(x,y,0);
                if (!tilemap.GetTile(p)) {
                    float noiseX = (x+10000f)*scale;
                    float noiseY = (y+10000f)*scale;
                    float noiseValue = Mathf.PerlinNoise(noiseX,noiseY);
                    int tileIndex = (int)(Mathf.Round(noiseValue));
                    tilemap.SetTile(p, tilesToPlace[tileIndex]);
                }
            }
        }
    }
}
