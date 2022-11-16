using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour{
    [SerializeField] private TextMeshProUGUI cachacaText;
    [SerializeField] private TextMeshProUGUI balaText;

    public CachacaController cachacaController; 
    public BalaController balaController;


    //Barra de vida e vida
    private const int maxHealth = 100;
    private int currentHealth;
    public Image lifebar;
    public Image damageBar;
    private bool canTakeDamage;
    
    //Outros
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
    private SpriteRenderer sprite;

    void Start(){
        this.cachacaController = new CachacaController(cachacaText);
        this.balaController = new BalaController(balaText);
        sprite = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        canTakeDamage = true;
    }

    void Update(){
        if(canTakeDamage == true){
            Fire();
            IsAlive();
            Move();
            Jump();
        }
    }

    void IsAlive(){
        if(currentHealth == 0){
            canTakeDamage = false;
            //ACIONAR ANIMAÇÃO DE MORTO AQUI
            Invoke("ReloadScene",3f);
        }
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

    void Jump(){
        if (Input.GetButtonDown("Jump") && !isJumping){
            rigidbody2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    //Método chamado pelos inimigos para dar dano no player
    public void takeDamage(int damage){
        if(canTakeDamage == true){
            canTakeDamage = false;
            StartCoroutine(this.damageCoroutine());
            setLife(currentHealth - damage);
        }
    }

    // Faz o personagem piscar ao tomar dano 
    IEnumerator damageCoroutine(){
        for(float i=0;i < 0.6f; i+=0.2f){
            sprite.enabled = false;
            yield return new WaitForSeconds(0.05f);
            sprite.enabled = true;
            yield return new WaitForSeconds(0.05f);
        }
        canTakeDamage = true;
    }

    //Reinicia a fase
    public void ReloadScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Ajusta a vida do personagem (dá para curar por aqui também)
    public void setLife(int newHealth){
        if(newHealth >100){
            setLife(100);
            return;
        }
        if(newHealth < 0){
            setLife(0);
            return;
        }else{
            this.currentHealth = newHealth; 
            Vector3 newLifeBar = lifebar.rectTransform.localScale;
            newLifeBar.x = (float) newHealth / maxHealth;
            lifebar.rectTransform.localScale = newLifeBar;
            StartCoroutine(this.DecreasingRedBar(lifebar.rectTransform.localScale));
        }
    }

    //Diminui a barra vermelha de dano lentamente
    private IEnumerator DecreasingRedBar(Vector3 actualLifeBar){
        yield return new WaitForSeconds(0.5f);
        Vector3 redBarScale = this.damageBar.transform.localScale;

        while(damageBar.transform.localScale.x > actualLifeBar.x){
            redBarScale.x -= Time.deltaTime * 0.4f;
            damageBar.transform.localScale = redBarScale;

            yield return null;
        }
        damageBar.transform.localScale = actualLifeBar;
    }


    // Ataque simples com arma
    void Fire(){
        if(Input.GetButtonDown("Fire1") && balaController.getQuantidadeBala() > 0){
            balaController.atirar();
            // Controle de animacao
            //animator.SetBool("isAtk", true);
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
            //if(timeLastBullet <= 0) animator.SetBool("isAtk", false);
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