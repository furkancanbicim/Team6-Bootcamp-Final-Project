using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GameData", menuName = "Data/Game Data")]
public class GameData : ScriptableObject
{
    public int money;
    public int currentLevel;
    public int levelCount;
    public List<Character> characters=new List<Character>();
    public List<GameObject> enemies=new List<GameObject>();
    public List<GameObject> players = new List<GameObject>();
}
