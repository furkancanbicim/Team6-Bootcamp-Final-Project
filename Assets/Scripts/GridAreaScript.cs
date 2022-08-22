using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAreaScript : MonoBehaviour
{

    [SerializeField] List<GridScript> grids;

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        EventManager.setGrid += SetActiveSquare;
        EventManager.doMerge += SetDeActiveSquare;
        EventManager.buyCharacter += CheckIsAreaEmpty;

    }
    private void OnDisable()
    {
        EventManager.setGrid -= SetActiveSquare;
        EventManager.doMerge -= SetDeActiveSquare;
        EventManager.buyCharacter -= CheckIsAreaEmpty;

    }

    bool CheckIsAreaEmpty(CharacterType type)
    {
        for(int i =grids.Count-1;i>=0;i--)
        {
            if(!grids[i].GetComponent<GridScript>().isFull)
            {
                EventManager.isGridFull?.Invoke(i==0);
                EventManager.spawnCharacter?.Invoke(type, grids[i].gameObject);
                return true;
            }
            else if(i==0)
            {
                EventManager.isGridFull?.Invoke(true);
                return false;
            }
        }
        return false;
    }    


    void SetActiveSquare(GameObject obj)
    {
        for (int i = 0; i < grids.Count; i++)
        {
            if (grids[i].name != obj.name)
                grids[i].greenSquare.SetActive(false);
            else
                grids[i].greenSquare.SetActive(true);
        }
    }
    void SetDeActiveSquare(CharacterType type, CharacterLevel level, Vector3 pos,GameObject grid)
    {
        
            grid.GetComponent<GridScript>().greenSquare.SetActive(false);
    }

}
