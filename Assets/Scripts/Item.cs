using UnityEngine;

[CreateAssetMenu(menuName = "Match3/Item")]
public sealed class Item : ScriptableObject
{
    //hangi icon ka� puan kazand�r�r.
    public int Value;

    public Sprite Sprite;
}
