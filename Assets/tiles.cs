using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class tiles : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile[] tilesToPlace;
    public int radius = 6;

    // Start is called before the first frame update
    void Start()
    {
        float scale = 1f/10f;
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                Vector3Int p = new Vector3Int(x,y,0);
                float noiseX = (x+10000f)*scale;
                float noiseY = (y+10000f)*scale;
                float noiseValue = Mathf.PerlinNoise(noiseX,noiseY);
                int tileIndex = (int)(Mathf.Round(noiseValue));
                // print(x+" , "+y+" / "+noiseX+" , "+noiseY+": "+Mathf.PerlinNoise(noiseX,noiseY)+" -> "+tileIndex);
                tilemap.SetTile(p, tilesToPlace[tileIndex]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
