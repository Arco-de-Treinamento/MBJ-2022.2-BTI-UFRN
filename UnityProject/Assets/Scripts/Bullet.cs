using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour{
    public int damage;
    public float distance;
    public LayerMask enemyLayer;
    public string enemyTag;

    // Update is called once per frame
    void Update(){
        RaycastHit2D bulletCast = Physics2D.Raycast(transform.position, transform.forward, distance, enemyLayer);

        if(bulletCast.collider != null){
            if(bulletCast.collider.CompareTag(enemyTag)){
                // Causa dano ao inimigo quando em contato com seu componente
                bulletCast.collider.GetComponent<Generic_Enemy>().TakeDamage(damage);
            }
            // Destroi projetil quando em contato com algum objeto da enemyLayer
            DestroyBullet();
        }
    }

    public void DestroyBullet(){
        Destroy(gameObject);
    }
}
