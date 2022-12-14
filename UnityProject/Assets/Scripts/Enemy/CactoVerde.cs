using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactoVerde : MonoBehaviour{
    
    [SerializeField]
    private Rigidbody2D rigidbody2D;

    [SerializeField]
    private float movementVelocity;

    [SerializeField]
    private SpriteRenderer sprite;
    private BoxCollider2D triggerDespertar;
    private bool paraEsquerda = true;
    private float distanceGround = 5;
    private bool isSleeping;
    public Transform groundCheck; 
    public int currentHealth;
    private Transform player;
    private Vector3 playerDistance;
    public int maxHealth = 5;
    public int touchingDamage;
    private Player playerCollision;
    private Bullet bulletCollision;
    private Animator animator;

    void Start(){
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigidbody2D = GetComponent<Rigidbody2D>();

        this.currentHealth = maxHealth;
        this.touchingDamage = 35;
        
        isSleeping = true;
    }

    void Update(){
        isAlive();
        atualizar();
        playerDistance = player.transform.position - transform.position;
    }

    private void atualizar(){
        if(isSleeping == false){
            follow();
        }
    }

    private void isAlive(){
        if(currentHealth <= 0){
            isSleeping = true;
            animator.SetTrigger("isDead");
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            GetComponent<BoxCollider2D>().enabled = false;
            
            Invoke("destroyBody", .5f);
        }
    }

    public void TakeDamage(int damage){ 
        currentHealth -= damage;
    }

    private void destroyBody(){
        Destroy(gameObject);
    }

    public void follow(){
        RaycastHit2D ground = Physics2D.Raycast(groundCheck.position, Vector2.down, distanceGround);
        if(ground.collider != false){

            if(playerDistance.x > 0){
                if(paraEsquerda) sprite.flipX = true;
                paraEsquerda = false;
                this.rigidbody2D.velocity = new Vector3(this.movementVelocity,0f,0f);
            }
            else{
                if(!paraEsquerda) sprite.flipX = false;
                paraEsquerda = true;
                this.rigidbody2D.velocity = new Vector3(-this.movementVelocity,0f,0f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        playerCollision = collision.gameObject.GetComponent<Player>();

        if(playerCollision != null){
            StartCoroutine(acordar());
        } 
    }

    IEnumerator acordar(){
        animator.SetBool("Acordou",true);
        animator.SetBool("Levantou",true);
        yield return new WaitForSeconds(1);
        this.isSleeping = false;
    }

    //causa dano no player e joga ele para trás
    private void OnCollisionEnter2D(Collision2D collision){
        playerCollision = collision.gameObject.GetComponent<Player>();
        bulletCollision = collision.gameObject.GetComponent<Bullet>();

        if( playerCollision != null){
            //causa dano de encostar no player
            playerCollision.takeDamage(this.touchingDamage);

            //Joga o player para trás
            playerCollision.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 8 * (playerDistance.x / Mathf.Abs(playerDistance.x)),ForceMode2D.Impulse);
        }

        if(bulletCollision != null){
            TakeDamage(4);
        }
    }
}