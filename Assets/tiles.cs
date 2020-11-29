using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class tiles : MonoBehaviour
{
    public Grid grid;
    public Tilemap tilemap;
    public int spawnRadius = 8;

    List<Tile> tileList = new List<Tile>();
    List<CornersTile> tileCornerList = new List<CornersTile>();
    enum Material {
        Unidentified, Transparent,
        //from bottom to top (first one will be rendered below the later ones)
        Water, Grass
    }

    class CornersTile {
        public Material topRight    { set; get; }
        public Material bottomRight { set; get; }
        public Material bottomLeft  { set; get; }
        public Material topLeft     { set; get; }
        public Tile     tile        { set; get; }
    }

    Material materialByColor(Color color) {
        if (color.a <= 0.1f) return Material.Transparent;
        if (rgbAbout(color, 0.302f, 0.573f, 0.384f)) return Material.Grass;
        if (rgbAbout(color, 0.388f, 0.608f, 1.000f)) return Material.Water;
        return Material.Unidentified;
    }

    bool about(float a, float b) {
        float margin = 0.001f;
        return a >= b-margin && a <= b+margin;
    }

    bool rgbAbout(Color color, float r, float g, float b) {
        return about(color.r, r) && about(color.g, g) && about(color.b, b);
    }

    string htmlColor(Color color) {
        int r = (int)Mathf.Round(color.r*255);
        int g = (int)Mathf.Round(color.g*255);
        int b = (int)Mathf.Round(color.b*255);
        return "#"+r.ToString("X")+g.ToString("X")+b.ToString("X");
    }

    Tile tileByCornerMats(Material tr, Material br, Material bl, Material tl) {
        var validTiles = tileCornerList.Where( t =>
            t.topRight==tr && t.bottomRight==br && t.bottomLeft==bl && t.topLeft==tl
        );
        print(validTiles.Count()+" tiles found for: ⌝"+tr+" ⌟"+br+" ⌞"+bl+" ⌜"+tl);
        if (validTiles.Count() > 0) {
            return validTiles.OrderBy(qu => System.Guid.NewGuid()).First().tile;
        }
        return tileCornerList.OrderBy(qu => System.Guid.NewGuid()).First().tile;
    }

    void Start() {
        var size = 32;
        Texture2D tileset = Resources.Load<Texture2D>("RPG Nature Tileset") as Texture2D;
        for (int x = 0; x+size <= tileset.width; x+=size)
        {
            for (int y = 0; y+size <= tileset.height; y+=size)
            {
                // if (x!=64 || y!=128) continue;
                Tile tile = ScriptableObject.CreateInstance<Tile>();
                tile.sprite = Sprite.Create(
                    tileset,
                    new Rect(x, y, size, size), // section of texture to use
                    new Vector2(0.5f, 0.5f), // pivot in centre
                    size, // pixels per unity tile grid unit
                    1,
                    SpriteMeshType.Tight,
                    Vector4.zero
                );
                tileList.Add(tile);
                CornersTile cornersTile =
                    new CornersTile {
                        topRight    = materialByColor(tileset.GetPixel(x+size-1,y))
                    ,   bottomRight = materialByColor(tileset.GetPixel(x+size-1,y+size-1))
                    ,   bottomLeft  = materialByColor(tileset.GetPixel(x,y+size-1))
                    ,   topLeft     = materialByColor(tileset.GetPixel(x,y))
                    ,   tile        = tile
                    };
                tileCornerList.Add(cornersTile);
            }
        }
    }

    void Update()
    {
        var centerCell = grid.WorldToCell(Vector3.zero);
        for (int x = centerCell.x-spawnRadius; x <= centerCell.x+spawnRadius; x++)
        {
            for (int y = centerCell.y-spawnRadius; y <= centerCell.y+spawnRadius; y++)
            {
                Vector3Int coords = new Vector3Int(x,y,0);
                if (!tilemap.GetTile(coords)) {
                    tilemap.SetTile(coords, getTileFor(x,y));
                }
            }
        }
    }

    Tile getTileFor(int x, int y)
    {
        // think of the tile actually being in the intersection of 4 coordinates
        return tileByCornerMats(
          getMaterialFor(x+1,y  ) // ⌝ corner
        , getMaterialFor(x+1,y+1) // ⌟ corner
        , getMaterialFor(x  ,y+1) // ⌞ corner
        , getMaterialFor(x  ,y  ) // ⌜ corner
        );
    }

    Material getMaterialFor(int x, int y)
    {
        if (isWater(x,y)) {
            return Material.Water;
        } else {
            return Material.Grass;
        }
    }

    bool isWater(int x, int y) {
        float noiseValue = 0;
        noiseValue += getValue(x, y, 09000f, 10761f, 845.4099116050787f)*2; //larger patterns, larger weight
        noiseValue += getValue(x, y, 10720f, 09719f, 54.1202221496704f);    //finer details, smaller weight
        return noiseValue <= 1.6f;
    }

    float getValue(int x, int y, float offsetX, float offsetY, float scale) {
        //use unique scales & offsets for different properties to avoid patterns
        float noiseX = (x+offsetX)/scale;
        float noiseY = (y+offsetY)/scale;
        return Mathf.PerlinNoise(noiseX,noiseY);
    }
}
