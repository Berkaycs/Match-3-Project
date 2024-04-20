using UnityEngine;
using UnityEngine.UI;

public sealed class Tile : MonoBehaviour
{
    public int x;
    public int y;

    private Item _item;
    public Item Item
    {
        get { return _item; }
        set 
        { 
            if(_item == value) return;
            _item = value;
            Icon.sprite = _item.Sprite;
        }
    }
    public Image Icon;
    public Button Button;

    private void Start()
    {
        Button.onClick.AddListener(call: () => Board.Instance.Select(this));
    }
}
