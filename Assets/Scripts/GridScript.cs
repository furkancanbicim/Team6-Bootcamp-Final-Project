using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    public CharacterType type;
    public CharacterLevel level;
    public bool isFull;
    public GameObject greenSquare;
    public GameObject stayingCharacter;
    private void Start()
    {
        isFull = false;
        greenSquare.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!isFull && collision.transform.GetComponent<PlayerScript>())
        {
            isFull = true;
            greenSquare.SetActive(false);
            stayingCharacter = collision.gameObject;
            type = collision.transform.GetComponent<PlayerScript>().type;
            level = collision.transform.GetComponent<PlayerScript>().level;
        }
    }
   
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.gameObject == stayingCharacter)
            isFull = false;
    }
    
}
