using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }

    public Row[] Rows;
    public Tile[,] Tiles { get; private set; }

    public int Width
    {
       get { return Tiles.GetLength(dimension: 0); }
    }
    public int Height
    {
        get { return Tiles.GetLength(dimension: 1); }
    }


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Tiles = new Tile[Rows.Max(selector: r=> r.Tiles1.Length),Rows.Length];

        for (var y = 0; y < Height; y++)
        {

            for (var x = 0; x < Width; x++)
            {
                var tile = Rows[y].Tiles1[x];
                tile.x = x;
                tile.y = y;
                tile.Item = ItemDatabase.Items[Random.Range(0,ItemDatabase.Items.Length-1)];
                Tiles[y, x] = tile;
            }
        }
    }
}
