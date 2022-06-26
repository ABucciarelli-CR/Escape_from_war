using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    public Transform forwardTileObj;
    public Transform leftTileObj;
    public Transform rightTileObj;
    public Transform endLevelTileSpawn;
    public GameObject mainCamera;
    public Transform[] obstacles;    // 0 = Hit    1 = Dodge    2 = Duck    3 = Jump
    public int tilesForLevel = 0;
    public float velocityMultiplier = 0.3f;
    public float[] obstacleSpawnRange;

    private Vector3 nextTileSpawn;
    
    void Start()
    {
        forwardTileObj.position = new Vector3(forwardTileObj.position.x,0.25f, forwardTileObj.position.z);
        //Debug.Log(mainCamera.GetComponent<CamMove>().gameVelocity * velocityMultiplier);
        for(int i=0; i<5; i++)
        {
            Instantiate(forwardTileObj, nextTileSpawn, forwardTileObj.rotation);
            nextTileSpawn.z += 3;
            tilesForLevel--;
        }
        
        StartCoroutine(SpawnTile());
        StartCoroutine(SpawnObstacle());
    }
    
    void Update()
    {
    }

    IEnumerator SpawnTile()
    {
        yield return new WaitForSeconds(mainCamera.GetComponent<CamMove>().gameVelocity * velocityMultiplier);

        switch (Random.Range(0,3))
        {
            case 0:
                Instantiate(forwardTileObj, nextTileSpawn, forwardTileObj.rotation);
                break;
            
            case 1:
                //nextTileSpawn.x -= 3;
                Instantiate(leftTileObj, nextTileSpawn, leftTileObj.rotation);
                break;
            
            case 2:
                //nextTileSpawn.x += 3;
                Instantiate(rightTileObj, nextTileSpawn, rightTileObj.rotation); 
                break;
            default:
                Debug.Log("Perchè sò qua? Coddio!");
                break;
        }
        //Instantiate(tileObj, nextTileSpawn, tileObj.rotation);
        nextTileSpawn.z += 3;
        tilesForLevel--;
        if(tilesForLevel > 0)
        {
            StartCoroutine(SpawnTile());
        }
        else
        {
            Instantiate(endLevelTileSpawn, nextTileSpawn, rightTileObj.rotation); 
        }
    }

    IEnumerator SpawnObstacle()
    {
        yield return new  WaitForSeconds(Random.Range(obstacleSpawnRange[0], obstacleSpawnRange[1]));
        Instantiate(obstacles[Random.Range(0,obstacles.Length)], new Vector3(Random.Range(-1, 2), nextTileSpawn.y + 1, nextTileSpawn.z), forwardTileObj.rotation);
        StartCoroutine(SpawnObstacle());
    }
}
