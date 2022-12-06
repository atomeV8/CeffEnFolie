using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PencilCollider : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        print("collide");
        if (col.gameObject.name == "End")
        {
            print("good collide");                                                                                      //Gagné
        }
    }
}
