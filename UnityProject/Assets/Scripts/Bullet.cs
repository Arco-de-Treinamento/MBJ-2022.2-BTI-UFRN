using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour{
    public int damage;
    public float distance;
    public LayerMask enemyLayer;
    public string enemyTag;

    public float time;
    void Start() {
        Invoke("DestroyBullet", time);    
    }
    // Update is called once per frame
    void Update(){
        RaycastHit2D bulletCast = Physics2D.Raycast(transform.position, transform.forward, distance, enemyLayer);

        if(bulletCast.collider != null){
            if(bulletCast.collider.CompareTag(enemyTag)){
                try{
                    bulletCast.collider.GetComponent<CactoVerde>().TakeDamage(damage);
                }catch(Exception e){

                }
                try{
                    bulletCast.collider.GetComponent<Predador>().TakeDamage(damage);
                }catch(Exception e){
                    
                }
                try{
                    bulletCast.collider.GetComponent<Alien>().TakeDamage(damage);
                }catch(Exception e){
                    
                }
            }
            DestroyBullet();
        }
    }

    public void DestroyBullet(){
        Destroy(gameObject);
    }
}
