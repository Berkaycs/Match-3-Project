using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class Tile : MonoBehaviour
{
    public int X;
    public int Y;

    private Item _item;
    public Item Item
    {
        get { return _item; }
        set 
        { 
            if(_item == value) return;
            //seçilen meyveyi tutar
            _item = value;
   
            Icon.sprite = _item.Sprite;
        }
    }
    public Image Icon;
    public Button Button;
    public Tile Right
    {
        get
        {
            if (X < Board.Instance.Width-1)
            {
                return Board.Instance.Tiles[X + 1, Y];
            }
            else
            {
                return null;
            }
        }
    }
    public Tile Left
    {
        get
        {
            if (X > 0)
            {
                return Board.Instance.Tiles[X - 1, Y];
            }
            else
            {
                return null;
            }
        }
    }
    public Tile Top
    {
        get
        {
            if (Y > 0)
            {
                return Board.Instance.Tiles[X, Y-1];
            }
            else
            {
                return null;
            }
        }
    }
    public Tile Bottom
    {
        get
        {
            if (Y < Board.Instance.Height-1)
            {
                return Board.Instance.Tiles[X, Y+1];
            }
            else
            {
                return null;
            }
        }
    }

    public Tile[] Neighboursx
    {
        get
        {
            return new Tile[]
            {
            Left,
            Right
            };
        }
    }
    public Tile[] Neighboursy
    {
        get
        {
            return new Tile[]
            {
            Top,
            Bottom
            };
        }
    }
    private void Start()
    {
        Button.onClick.AddListener(call: () => Board.Instance.Select(this));
    }

    public List<Tile> GetConnectedTilesx(List<Tile> exclude = null)
    {
        var result = new List<Tile>()
        {
            this,
        };
        if(exclude == null)
        {
            exclude = new List<Tile>
            {
                this,
            };
        }
        else
        {
            exclude.Add(this);
        }

        foreach(var neighbour in Neighboursx)
        {
            if(neighbour == null || exclude.Contains(neighbour) || neighbour.Item != Item)
            {
                continue;
            }
            result.AddRange(neighbour.GetConnectedTilesx(exclude));
        }
        return result;
    }
    public List<Tile> GetConnectedTilesy(List<Tile> exclude = null)
    {
        var result = new List<Tile>()
        {
            this,
        };
        if (exclude == null)
        {
            exclude = new List<Tile>
            {
                this,
            };
        }
        else
        {
            exclude.Add(this);
        }

        foreach (var neighbour in Neighboursy)
        {
            if (neighbour == null || exclude.Contains(neighbour) || neighbour.Item != Item)
            {
                continue;
            }
            result.AddRange(neighbour.GetConnectedTilesy(exclude));
        }
        return result;
    }
}
