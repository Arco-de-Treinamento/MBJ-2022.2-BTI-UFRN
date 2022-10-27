using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour{
    private const int maxHealth = 100;
    public int currentHealth;
    public Image lifebar;
    public Image damageBar;

    public float speed;
    public float jumpForce;
    private bool isJumping;
    private bool isFacingRight = true;
    private new Rigidbody2D rigidbody2D;
    public Transform bulletPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    private float timeLastBullet;
    private Animator animator;


    // Start is called before the first frame update
    void Start(){
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update(){
        Move();
        Jump();
        Fire();
    }

    // Movimentacao lateral do personagem
    void Move(){
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movement * Time.deltaTime * speed;

        // Rotacionando personagem para a direita
        if(Input.GetAxis("Horizontal") > 0f){
            isFacingRight = true;
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }

        // Rotacionando personagem para a esquerda
        if(Input.GetAxis("Horizontal") < 0f){
            isFacingRight = false;
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    // Pulo simples do personagem
    void Jump(){
        if (Input.GetButtonDown("Jump") && !isJumping){
            rigidbody2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            this.setLife(currentHealth - 20);
            }
    }

    public void setLife(int newHealth){
        if(newHealth >100 || newHealth < 0)
            return;

        this.currentHealth = newHealth;
        Vector3 newLifeBar = lifebar.rectTransform.localScale;
        newLifeBar.x = (float) newHealth / maxHealth;
        lifebar.rectTransform.localScale = newLifeBar;
        StartCoroutine(this.DecreasingRedBar(lifebar.rectTransform.localScale));
    }

    private IEnumerator DecreasingRedBar(Vector3 actualLifeBar){
        yield return new WaitForSeconds(0.5f);
        Vector3 redBarScale = this.damageBar.transform.localScale;

        while(damageBar.transform.localScale.x > actualLifeBar.x){
            redBarScale.x -= Time.deltaTime * 0.25f;
            damageBar.transform.localScale = redBarScale;

            yield return null;
        }
        damageBar.transform.localScale = actualLifeBar;
    }


    // Ataque simples com arma
    void Fire(){
        if(Input.GetButtonDown("Fire1")){
            // Controle de animacao
            animator.SetBool("isAtk", true);
            timeLastBullet = 0.5f;

            // Cria a bala no bulletPoint quando é acionado o disparo
            GameObject bulletObject = Instantiate(bulletPrefab, bulletPoint.position, transform.rotation);

            // Determina a direcao do projeto com base na direcao do personagem
            if(isFacingRight){
                bulletObject.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed, 0f);
            }else{
                bulletObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-bulletSpeed, 0f);
            }

            // Controle de animacao
            timeLastBullet -= Time.deltaTime;
            if(timeLastBullet <= 0) animator.SetBool("isAtk", false);
        }
    }

    // Deteccao de entrada do colisor do personagem
    void OnCollisionEnter2D(Collision2D collision2D){
        // Ground(Layer 10)
        if(collision2D.gameObject.layer == 10) isJumping = false;
        
    }

    // Deteccao de saida do colisor do personagem
    void OnCollisionExit2D(Collision2D collision2D){
        // Ground(Layer 10)
        if(collision2D.gameObject.layer == 10) isJumping = true;
    }
}
