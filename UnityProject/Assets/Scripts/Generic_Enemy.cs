using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Enemy : MonoBehaviour{
    public int lifeEnemy;

    // Update is called once per frame
    void Update(){
        // Destroy o inimigo quando a vida chegar ao fim
        if(lifeEnemy <= 0) Destroy(gameObject);
    }

    // Funcao publica que diminui a vida do inimigo
    public void TakeDamage(int damage){ 
        lifeEnemy -= damage;
    }
}
