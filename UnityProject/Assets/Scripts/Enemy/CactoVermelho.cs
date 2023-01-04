using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactoVermelho : MonoBehaviour{
    
    [SerializeField]
    private Rigidbody2D rigidbody;

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
    public Animator animator;

    void Start(){
        sprite = GetComponent<SpriteRenderer>();
        isSleeping = true;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        this.currentHealth = maxHealth;
        this.touchingDamage = 35;
        animator = GetComponent<Animator>();
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
            animator.SetBool("Morreu",true);
            for(int i=0;i<3;i++){
                //espera por 3 frames
            }
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage){ 
        currentHealth -= damage;
    }

    public void follow(){
        RaycastHit2D ground = Physics2D.Raycast(groundCheck.position, Vector2.down, distanceGround);
        if(ground.collider != false){
            if(playerDistance.x > 0){
                if(paraEsquerda) sprite.flipX = true;
                paraEsquerda = false;
                this.rigidbody.velocity = new Vector3(this.movementVelocity,0f,0f);
            }
            else{
                if(!paraEsquerda) sprite.flipX = false;
                paraEsquerda = true;
                this.rigidbody.velocity = new Vector3(-this.movementVelocity,0f,0f);
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
        if( playerCollision != null){
            //causa dano de encostar no player
            playerCollision.takeDamage(this.touchingDamage);

            //Joga o player para trás
            playerCollision.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 8 * (playerDistance.x / Mathf.Abs(playerDistance.x)),ForceMode2D.Impulse);
        }
    }

}