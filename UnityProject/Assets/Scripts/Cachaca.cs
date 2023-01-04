using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cachaca : MonoBehaviour{
    private Player player;

    void OnTriggerEnter2D(Collider2D collision){
        player = collision.gameObject.GetComponent<Player>();
        if(player != null){
          player.cachacaController.catchCachaca();
          Destroy(gameObject);
        } 
    }
}
