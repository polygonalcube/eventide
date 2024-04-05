using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Object/Item")]
public class Item : ScriptableObject
{
    public bool Stackable = true;
    public Sprite Image;
}
