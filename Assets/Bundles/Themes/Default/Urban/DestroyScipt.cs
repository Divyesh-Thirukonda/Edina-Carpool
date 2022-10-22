using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScipt : MonoBehaviour
{
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "destroyBuilding")
        {
            Destroy(gameObject);
        }
    }
}
