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
    private float axisMovement;
    private bool isFacingRight = true;
    public float jumpForce;
    private bool isJumping;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isPush;
    public Transform boxCheck;
    public LayerMask boxLayer;
    private Rigidbody2D rigidbody2D;
    public Transform bulletPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    private float timeLastBullet;
    private Animator animator;
    private SpriteRenderer sprite;
    private bool usandoArma;

    void Start(){
        this.cachacaController = new CachacaController(cachacaText);
        this.balaController = new BalaController(balaText);
        sprite = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        canTakeDamage = true;
        usandoArma = false;
    }

    void Update(){
        if(canTakeDamage == true){
            Fire();
            IsAlive();
            Move();
            Jump();
            pushElement();
            checkButtons();
        }
    }

    private void checkButtons(){
        if(Input.GetKeyDown(KeyCode.E)){
            if(cachacaController.useCachaca()){
                this.setLife(currentHealth + 20);
            }
        }
        if(Input.GetKeyDown(KeyCode.Q)){
            this.usandoArma = !(this.usandoArma);
        }
    }

    void IsAlive(){
        if(currentHealth == 0){
            canTakeDamage = false;

            animator.SetTrigger("isDead");
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            GetComponent<BoxCollider2D>().enabled = false;

            Invoke("ReloadScene",3f);
        }
    }

    // Movimentacao lateral do personagem
    void Move(){
        axisMovement = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(axisMovement, 0f, 0f);

        transform.position += movement * Time.deltaTime * speed;
        animator.SetFloat("Speed", Mathf.Abs(axisMovement));

        if(((axisMovement < 0) && isFacingRight) || ((axisMovement > 0) && !isFacingRight)){
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    // Metodo chamado para verificar a colisão com elementos moveis
    void pushElement(){
        isPush = Physics2D.OverlapCircle(boxCheck.position, 0.1f, boxLayer);
        animator.SetBool("isPush", isPush);
    }

    void Jump(){
        isJumping = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        animator.SetBool("checkGround", isJumping);
        animator.SetFloat("jumpSpeed", rigidbody2D.velocity.y);

        if (Input.GetButtonDown("Jump") && isJumping){
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
        if(Input.GetButtonDown("Fire1") && balaController.getQuantidadeBala() > 0 && usandoArma){
            balaController.atirar();
            // Controle de animacao
            animator.SetTrigger("isAtkRifle");
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
        }
        if(Input.GetButtonDown("Fire1") && !(usandoArma)){
            animator.SetTrigger("isAtkPunhal");
            GetComponentInChildren<Sword>(true).attack();
        }
    }
}
   