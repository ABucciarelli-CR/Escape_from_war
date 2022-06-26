using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    /*
     *Layers rules:
     *layers(Hit, Duck, Jump) cannot interact with themselves
     * if player hit them a life will be removed
     * all life removed will be restored after level completion or death
     */
    
    private Touch theBadTouch;             //Oh...you touch my tra la la 
    private Vector2 touchStartPosition;    //Mmm...my ding ding dong
    private Vector2 touchEndPosition;
    private int totalPlayerHealth = 3;
    private int temporaryPlayerHealth;
    private bool canAct = true;

    public TMP_Text showLife;
    public float immunityTime = 1f;
    public int direction;    // 0= left/down         1= Right/up
    public GameObject player;
    
    void Start()
    {
        UpdatePlayerHealth(0);
    }
    
    void Update()
    {

        if (Input.touchCount > 0)
        {
            theBadTouch = Input.GetTouch(0);
            
            if (theBadTouch.phase == TouchPhase.Began)
            {
                //Debug.Log("Start Touch");
                touchStartPosition = theBadTouch.position;
            }
            else if (theBadTouch.phase == TouchPhase.Ended)
            {
                touchEndPosition = theBadTouch.position;
                float x = touchEndPosition.x - touchStartPosition.x;
                float y = touchEndPosition.y - touchStartPosition.y;

                //tap
                if (Mathf.Abs(x) == 0 && Mathf.Abs(y) == 0)
                {
                    LayerChange(1);
                    Debug.Log("Tap");
                }
                
                //left & right
                if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    direction = x > 0 ? 1 : 0;
                    //Going left
                    if (direction == 0)
                    {
                        if (player.transform.position.x == 0)
                        {
                            player.transform.position = new Vector3(-1, 1, player.transform.position.z);
                        }
                        else if(player.transform.position.x > 0)
                        {
                            player.transform.position = new Vector3(0, 1, player.transform.position.z);
                        }
                    }
                    //Going right
                    else if(direction == 1)
                    {
                        if (player.transform.position.x == 0)
                        {
                            player.transform.position = new Vector3(1,1, player.transform.position.z);
                        }
                        else if(player.transform.position.x < 0)
                        {
                            player.transform.position = new Vector3(0, 1, player.transform.position.z);
                        }
                    }
                    else
                    {
                        Debug.Log("Non sono nè destro nè sinistro ç.ç");
                    }
                }
                
                //up & down
                if (Mathf.Abs(x) < Mathf.Abs(y))
                {
                    direction = y > 0 ? 1 : 0;
                    //Going down
                    if (direction == 0)
                    {
                        LayerChange(2);
                        Debug.Log("Down");
                    }
                    //Going up
                    else if(direction == 1)
                    {
                        LayerChange(3);
                        Debug.Log("Up");
                    }
                    else
                    {
                        Debug.Log("Non sono nè Up nè Down ç.ç");
                    }
                }
            }
        }
    }

    void UpdatePlayerHealth(int healthCase)    //    0=Reset health    1=Subtract Health
    {
        switch (healthCase)
        {
            case 0:
                Debug.Log("resetted");
                temporaryPlayerHealth = totalPlayerHealth;
                UpdateShownHealth();
                break;
            case 1:
                temporaryPlayerHealth--;
                UpdateShownHealth();
                CheckPlayerHealth();
                break;
            default:
                Debug.Log("---------------Health case default-----------");
                break;
        }
    }

    void LayerChange(int layerChosen)
    {
        /*
         * Layer:
         * 0 = default
         * 1 = Hit
         * 2 = Duck
         * 3 = Jump
         */
        if (canAct)
        {
            switch (layerChosen)
            {
                //restore default layer
                case 0:
                    canAct = false;
                    player.layer = LayerMask.NameToLayer("Default");
                    break;
                //layer set as Hit
                case 1:
                    StartCoroutine(WaitForChange());
                    player.layer = LayerMask.NameToLayer("Hit");
                    break;
                //layer set as Duck
                case 2:
                    StartCoroutine(WaitForChange());
                    player.layer = LayerMask.NameToLayer("Duck");
                    break;
                //layer set as Jump
                case 3:
                    StartCoroutine(WaitForChange());
                    player.layer = LayerMask.NameToLayer("Jump");
                    break;
                default:
                    Debug.Log("----------Hit in default wtf----------");
                    break;
            }
        }
        /*else
        {
            player.layer = LayerMask.NameToLayer("Default");
        }*/
    }

    IEnumerator WaitForChange()
    {
        canAct = false;
        yield return new WaitForSeconds(immunityTime);
        player.layer = LayerMask.NameToLayer("Default");
        canAct = true;
    }
    /*
    IEnumerator ResetLayer()
    {
        yield return new WaitForSeconds(immunityTime);
        canAct = true;
        player.layer = LayerMask.NameToLayer("Default");
        //LayerChange(0);
    }*/

    void CheckPlayerHealth()
    {
        if (temporaryPlayerHealth <= 0)
        {
            Debug.Log("Player is Dead");
        }
    }

    void UpdateShownHealth()
    {
        showLife.text = temporaryPlayerHealth.ToString();
    }

    public void Hit()
    {
        UpdatePlayerHealth(1);
    }
}
