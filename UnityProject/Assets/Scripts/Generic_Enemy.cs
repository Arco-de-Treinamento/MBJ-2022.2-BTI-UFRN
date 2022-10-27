using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Enemy : MonoBehaviour{
    private int currentHealth;
    private Transform player;
    private Vector3 playerDistance;
    public int maxHealth = 100;
    public int touchingDamage = 20;

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player").transform;
        this.currentHealth = maxHealth;
    }

    void Update(){
        playerDistance = player.transform.position - transform.position;
    }

    private void death(){
        if(currentHealth <= 0){
            //Adicionar animação de morte aqui
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage){ 
        currentHealth -= damage;
    }

    private void OnCollisionEnter2D(Collision2D collision){
        Player player = collision.gameObject.GetComponent<Player>();
        if( player != null){
            player.takeDamage(this.touchingDamage);
            player.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 8 * (playerDistance.x / Mathf.Abs(playerDistance.x)),ForceMode2D.Impulse);
        }
    }

}
