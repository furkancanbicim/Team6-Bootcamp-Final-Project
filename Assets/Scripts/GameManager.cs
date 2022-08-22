using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameData data;
    [SerializeField] private List<GameObject> meleeCaharacters;
    [SerializeField] private List<GameObject> rangedCaharacters;
    private List<GameObject> spawnedPlayers = new List<GameObject>();

    private void Start()
    {
        for(int i=0;i<data.characters.Count;i++)
        {
            SpawnForFight(data.characters[i].type, data.characters[i].level, data.characters[i].position);
        }
        if(SceneManager.GetActiveScene().name == "Level 1")
        data.characters.Clear();
    }

    private void OnEnable()
    {
        EventManager.doMerge += Merge;
        EventManager.spawnCharacter += SpawnNewCharacter;
        EventManager.goFightScene += ClearCharactesInData;
        EventManager.characterDeath += CharacterDeaths;
    }
    private void OnDisable()
    {
        EventManager.doMerge -= Merge;
        EventManager.spawnCharacter -= SpawnNewCharacter;
        EventManager.goFightScene -= ClearCharactesInData;
        EventManager.characterDeath -= CharacterDeaths;
    }
    void SpawnForFight(CharacterType type, CharacterLevel level, Vector3 position)
    {
        if (type == CharacterType.Melee)
        {
            GameObject obj = Instantiate(meleeCaharacters[(int)level]);
            obj.transform.position = position;
            
        }
        else
        {

            GameObject obj = Instantiate(rangedCaharacters[(int)level]);
            obj.transform.position = position;
        }
    }
    void CharacterDeaths()
    {
        if (data.players.Count <= 0)
            EventManager.loseGame?.Invoke();
        else if (data.enemies.Count <= 0)
            EventManager.winGame?.Invoke();

    }

    void SpawnNewCharacter(CharacterType type,GameObject grid)
    {
        Vector3 pos = new Vector3(grid.transform.position.x,1f, grid.transform.position.z);
        SpawnNewCharacter(type, CharacterLevel.Level1, pos, grid);
    }
    private void SpawnNewCharacter(CharacterType type, CharacterLevel level, Vector3 position, GameObject grid)
    {
        if(type == CharacterType.Melee)
        {
            GameObject obj = Instantiate(meleeCaharacters[(int)level]);
            SetGrid(obj, grid, position);
        }
        else
        {

            GameObject obj = Instantiate(rangedCaharacters[(int)level]);
            SetGrid(obj, grid, position);
        }
    }
    private void Merge(CharacterType type,CharacterLevel level,Vector3 position, GameObject grid)
    {
        if(type==CharacterType.Melee && (int)level < 2)
        {
            GameObject obj = Instantiate(meleeCaharacters[(int)level + 1]);
            SetGrid(obj, grid, position);
        }
        else if(type == CharacterType.Ranged&& (int)level <2)
        {
            GameObject obj = Instantiate(rangedCaharacters[(int)level + 1]);
            SetGrid(obj, grid, position);
        }

        
    }
    void SetGrid(GameObject obj,GameObject grid,Vector3 position)
    {
        spawnedPlayers.Add(obj);
        obj.transform.position = position;
        obj.GetComponent<PlayerScript>().lastGrid = grid;
        grid.GetComponent<GridScript>().stayingCharacter = obj;
    }

    void ClearCharactesInData()
    {
        
        data.enemies.Clear();
        data.players.Clear();
    }

    private void OnApplicationQuit()
    {
        ClearCharactesInData();
    }
}
