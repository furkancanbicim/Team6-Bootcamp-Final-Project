using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DragScript : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;


    private void OnMouseDown()
    {
        if(SceneManager.GetActiveScene().name=="Level 1")
        {
            mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            mOffset = gameObject.transform.position - GetMouseWorldPos();
            GetComponent<CapsuleCollider>().isTrigger = true;
        }
      
    }

    public Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;

        mousePos.z = mZCoord+(mousePos.y/100);

        return Camera.main.ScreenToWorldPoint(mousePos);
    }
    private void OnMouseDrag()
    {
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            transform.position = GetMouseWorldPos() + mOffset;
            transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
        }
       
    }
    

}
