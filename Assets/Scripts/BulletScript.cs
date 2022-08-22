using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    string type;
    int damage;
    GameObject target;
    public void SetTarget(string gettedType, int gettedDamage, GameObject obj)
    {
        type = gettedType;
        damage = gettedDamage;
        target = obj;
        StartCoroutine(Move());
    }
    private void OnEnable()
    {
        EventManager.winGame += DestroyThis;
        EventManager.loseGame += DestroyThis;
    }
    private void OnDisable()
    {
        EventManager.winGame -= DestroyThis;
        EventManager.loseGame -= DestroyThis;
    }

    void DestroyThis()
    {
        StopAllCoroutines();
        Destroy(this.gameObject);
    }
    IEnumerator Move()
    {
        while(true)
        {
            if(target)
          transform.position=  Vector3.MoveTowards(transform.position,target.transform.position,10f*Time.deltaTime);
            else
            {
                DestroyThis();
            }
                
            yield return new WaitForEndOfFrame();
        }
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Enemy"&& type=="Enemy")
        {
            EventManager.attackByPlayer?.Invoke(other.gameObject,damage);
            DestroyThis();
        }
        else if(other.tag == "Player" && type == "Player")
        {
            EventManager.attackByEnemy?.Invoke(other.gameObject, damage);
            DestroyThis();
        }
            
    }
}
