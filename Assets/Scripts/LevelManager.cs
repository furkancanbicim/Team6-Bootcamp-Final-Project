using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameData gameData;

    private void OnEnable()
    {
        EventManager.goFightScene += NextLevel;
    }
    private void OnDisable()
    {
        EventManager.goFightScene -= NextLevel;
    }
    void NextLevel()
    {
        if(gameData.currentLevel>gameData.levelCount)
        {
            gameData.currentLevel = 1;
            gameData.characters.Clear();
            gameData.money = 200;
            gameData.enemies.Clear();
            gameData.players.Clear();
        }
        Invoke("LoadScene", 1f);
      
    }
    void LoadScene()
    {
        SceneManager.LoadScene("Level " + gameData.currentLevel);
    }
}
