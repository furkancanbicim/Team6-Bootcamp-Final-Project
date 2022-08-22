using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CharacterData", menuName = "Data/Character Data")]
public class CharacterData : ScriptableObject
{
    public CharacterType type=CharacterType.Melee;
    public CharacterLevel level=CharacterLevel.Level1;
    public int health;
    public int attack;
}