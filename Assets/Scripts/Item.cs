using UnityEngine;

[CreateAssetMenu(menuName = "Match3/Item")]
public sealed class Item : ScriptableObject
{
    //hangi icon kaç puan kazandýrýr.
    public int Value;

    public Sprite Sprite;
}
