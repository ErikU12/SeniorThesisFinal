using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour


{
 

    void Start()
    {
     
    }

    
    void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow))
        {
         transform.position += new Vector3(0.1f,0,0);
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
         transform.position += new Vector3(-0.1f,0,0);
        }
        if(Input.GetKey(KeyCode.UpArrow))
        {
         transform.position += new Vector3(0,0.1f,0);
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
         transform.position += new Vector3(0,-0.1f,0);
        }
    }

}
