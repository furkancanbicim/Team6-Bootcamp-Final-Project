using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Card", menuName = "CartData/Card")]
public class CardScriptableObjects : ScriptableObject
{
    public string name;
    public int health;
    public int attack;
}
