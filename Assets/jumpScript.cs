using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpScript : MonoBehaviour
{
    void OnTriggerEnter(Collider collisionInfo) {
        if(collisionInfo.gameObject.tag == "Player"){
            int i = 0;
            //For every item in the jumpBoostCollection, if item empty, make the iterationCount of the jumpBoostCollection to the game object.
            foreach(GameObject item in GameObject.Find("Player").GetComponent<PlayerSwipe>().jumpBoostCollection) {
                if(item == null) {
                    GameObject.Find("Player").GetComponent<PlayerSwipe>().jumpBoostCollection[i] = gameObject;
                    i=0;
                    GetComponent<BoxCollider>().enabled = false;
                    transform.parent = null;
                    gameObject.SetActive(false);
                    return;
                }
                i++;
            }
        }
    }
}
