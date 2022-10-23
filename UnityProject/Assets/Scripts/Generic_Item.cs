using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Item : MonoBehaviour
{
    // Detector de colisao de triggers generico
    void OnTriggerEnter2D(Collider2D collider2D){
        // Destroi o elemento
        if(collider2D.gameObject.tag == "Player") Destroy(gameObject);
    }
}
