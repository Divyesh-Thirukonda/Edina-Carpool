using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    // This script basically stores a bunch of variables and also spawns the buildings
    
    public GameObject[] blocksToSpawn; // blocks to place (essentially static)

    private GameObject nextBlock;

    System.Random random = new System.Random();

    public static float timeForNextBlock = 0;

    public GameObject transformOfLastPlacementReference;

    public static float playerSpeed = 1000f;
    public static float numOfBlocksSpawned;
    public static float points = 0;
    public static float scoreMultiplier = 1;

    // Update is called once per frame
    void Update()
    {
        timeForNextBlock += 1 * Time.deltaTime;
        points += 10 * Time.deltaTime * scoreMultiplier;
        if (timeForNextBlock >= .8) {
            // code to do when its time to place a new block
            nextBlock = blocksToSpawn[random.Next(0,6)]; //random number 0, 1, 2... 12, 13
            nextBlock = Instantiate(nextBlock, transformOfLastPlacementReference.transform);
            numOfBlocksSpawned++;
            nextBlock.transform.parent = null;
            
            timeForNextBlock = 0;
        }
        nextBlock.GetComponent<Rigidbody>().AddForce(0, 0, -playerSpeed*Time.deltaTime);
        
    }
}
