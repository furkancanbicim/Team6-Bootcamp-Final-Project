using System;
using UnityEngine;

public static class EventManager
{
    #region GameManagerEvents
    public static Action startGame;
    public static Action winGame;
    public static Action loseGame;
    #endregion

    #region LevelManagerEvents
    public static Action loadNextScene;
    public static Action loadOpeningScene;
    public static Action loadSameScene;
    #endregion

    #region UserInterfaceEvents
    public static Action showWinPanel;
    public static Action showFailPanel;
    #endregion

    #region InGameEvents
    public static Action<GameObject> setGrid;
    public static Action goFightScene;
    public static Action<CharacterType, CharacterLevel, Vector3,GameObject> doMerge;
    public static Action<CharacterType, GameObject> spawnCharacter;
    public static Func<CharacterType,bool> buyCharacter;
    public static Action<bool> isGridFull;
    public static Action<GameObject, int> attackByPlayer;
    public static Action<GameObject, int> attackByEnemy;
    public static Action characterDeath;
    #endregion

}