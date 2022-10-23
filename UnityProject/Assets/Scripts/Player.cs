using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{
    public float speed;
    public float jumpForce;
    public bool isJumping;
    private new Rigidbody2D rigidbody2D;

    // Start is called before the first frame update
    void Start(){
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update(){
        Move();
        Jump();
    }

    // Movimentacao lateral do personagem
    void Move(){
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movement * Time.deltaTime * speed;

        // Rotacionando personagem para a direita
        if(Input.GetAxis("Horizontal") > 0f)
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        
        // Rotacionando personagem para a esquerda
        if(Input.GetAxis("Horizontal") < 0f)
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
    }

    // Pulo simples do personagem
    void Jump(){
        if (Input.GetButtonDown("Jump") && !isJumping)
            rigidbody2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        
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
