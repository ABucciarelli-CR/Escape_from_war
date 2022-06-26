using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject gameMaster;
    
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("yo!");
        other.enabled = false;
        other.GameObject().SetActive(false);
        gameMaster.GetComponent<EventHandler>().Hit();
    }
}
