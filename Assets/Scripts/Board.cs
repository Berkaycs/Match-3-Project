using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using System;

public class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }


    public Row[] Rows;

    //iki boyutlu dizi. sütun ve satýrý ifade eder.
    public Tile[,] Tiles { get; private set; }

    //Width sütun sayýsý, Height satýr sayýsý.
    public int Width
    {
       get { return Tiles.GetLength(dimension: 0); }
    }
    public int Height
    {
        get { return Tiles.GetLength(dimension: 1); }
    }

    private readonly List<Tile> _selection = new List<Tile>();

    private const float TweenDuration = 0.25f;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //linQ ile pratik yoldan yazým.
        Tiles = new Tile[Rows.Max(selector: r=> r.Tiles1.Length),Rows.Length];

        for (int a = 0; a < Height; a++)
        {

            for (int b = 0; b < Width; b++)
            {
                Tile tile = Rows[a].Tiles1[b];
                tile.x = b;
                tile.y = a;
                tile.Item = ItemDatabase.Items[UnityEngine.Random.Range(0,ItemDatabase.Items.Length-1)];
                Tiles[b, a] = tile;
            }
        }
        Popx();
        Popy();
    }

   
    public async void Select(Tile tile)
    {
        var control = 0;
        
        if (!_selection.Contains(tile))
        {
            _selection.Add(tile);
        }
        if(_selection.Count <2)
        {
            return;
        }
        Debug.Log($"Selected tiles at ({_selection[0].x}),({_selection[0].y}) and ({ _selection[1].x}),({ _selection[1].y})");
        await Swap(_selection[0], _selection[1]);
        if ((_selection[0].x == _selection[1].x-1 && _selection[0].y == _selection[1].y) || (_selection[0].x == _selection[1].x + 1 && _selection[0].y == _selection[1].y) || (_selection[0].y == _selection[1].y + 1 && _selection[0].x == _selection[1].x) || (_selection[0].y == _selection[1].y - 1 && _selection[0].x == _selection[1].x))
        {
            if (CanPopx())
            {
                Popx();
                control = 1;
            }
            if (CanPopy())
            {
                Popy();
                control = 1;
            }
            if (control == 0)
            {
                await Swap(_selection[0], _selection[1]);
            }

            _selection.Clear();
            control = 0;           
        }
        else
        {
            await Swap(_selection[0], _selection[1]);
            _selection.Clear();
        }
        
    }


    public async Task Swap(Tile tile1,Tile tile2)
    {
        var icon1 = tile1.Icon;
        var icon2 = tile2.Icon;

        var icon1Transform = icon1.transform;
        var icon2Transform = icon2.transform;

        var sequence = DOTween.Sequence();
        sequence.Join(icon1Transform.DOMove(icon2Transform.position, TweenDuration)).Join(icon2Transform.DOMove(icon1Transform.position, TweenDuration));

        await sequence.Play().AsyncWaitForCompletion();

        icon1Transform.SetParent(tile2.transform);
        icon2Transform.SetParent(tile1.transform);

        tile1.Icon = icon2;
        tile2.Icon = icon1;

        //önceki bilgiyi tutar.
        var tile1Item = tile1.Item;

        tile1.Item = tile2.Item;
        tile2.Item = tile1Item;

    }

    private bool CanPopx()
    {
        for (var y=0; y < Height; y++)        
            for (var x=0; x < Width; x++)            
                if (Tiles[x, y].GetConnectedTilesx().Skip(1).Count() >= 2)
                {
                    return true;
                }
                
        return false;
    }
    private bool CanPopy()
    {
        for (var y = 0; y < Height; y++)
            for (var x = 0; x < Width; x++)
                if (Tiles[x, y].GetConnectedTilesy().Skip(1).Count() >= 2)
                {
                    return true;
                }

        return false;
    }
    private async void Popx()
    {
        for (var y=0; y < Height; y++)
        {
            for (var x=0; x < Width; x++)
            {
                var tile = Tiles[x, y];
                var connectedTiles = tile.GetConnectedTilesx();
                if (connectedTiles.Skip(1).Count() < 2)
                { 
                    continue;
                }
                var deflateSequence = DOTween.Sequence();

                foreach (var connectedTile in connectedTiles)
                { 
                    deflateSequence.Join(connectedTile.Icon.transform.DOScale(Vector3.zero,TweenDuration));
                }
                await deflateSequence.Play().AsyncWaitForCompletion();

                var inflateSequence = DOTween.Sequence();

                foreach(var connectedTile in connectedTiles)
                {
                    connectedTile.Item = ItemDatabase.Items[UnityEngine.Random.Range(0,ItemDatabase.Items.Length)];
                    inflateSequence.Join(connectedTile.Icon.transform.DOScale(Vector3.one,TweenDuration));
                }
                await inflateSequence.Play().AsyncWaitForCompletion();
                if (CanPopx())
                {
                    Popx();
                }
                if (CanPopy())
                {
                    Popy();
                }
            }
        }
    }
    private async void Popy()
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var tile = Tiles[x, y];
                var connectedTiles = tile.GetConnectedTilesy();
                if (connectedTiles.Skip(1).Count() < 2)
                {
                    continue;
                }
                var deflateSequence = DOTween.Sequence();

                foreach (var connectedTile in connectedTiles)
                {
                    deflateSequence.Join(connectedTile.Icon.transform.DOScale(Vector3.zero, TweenDuration));
                }
                await deflateSequence.Play().AsyncWaitForCompletion();

                var inflateSequence = DOTween.Sequence();

                foreach (var connectedTile in connectedTiles)
                {
                    connectedTile.Item = ItemDatabase.Items[UnityEngine.Random.Range(0, ItemDatabase.Items.Length)];
                    inflateSequence.Join(connectedTile.Icon.transform.DOScale(Vector3.one, TweenDuration));
                }
                await inflateSequence.Play().AsyncWaitForCompletion();
                if (CanPopx())
                {
                    Popx();
                }
                if (CanPopy())
                {
                    Popy();
                }
            }
        }
    }
}
