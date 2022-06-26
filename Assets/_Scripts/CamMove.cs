using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    public int gameVelocity;
    public GameObject thePlayer;
    
    void Start()
    {
        ChangeVelocity();
    }
    
    void Update()
    {
        
    }

    void ChangeVelocity()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0,0,gameVelocity);
        thePlayer.GetComponent<Rigidbody>().velocity = new Vector3(0,0,gameVelocity);
    }
}
