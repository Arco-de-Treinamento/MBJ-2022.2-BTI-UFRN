using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camera : MonoBehaviour
{
    public float speedCamera;
    public float yOffesetCamera;
    public Transform player;

    //backgorund
    public RawImage background;
    private float rectMove = 0f;
    void Start(){
        background.uvRect = new Rect(rectMove, 0, 1, 1);
    }

    // Update is called once per frame
    void Update(){
        // Movimentacao da camera com delay
        Vector3 movement = new Vector3(player.position.x, (player.position.y + yOffesetCamera), -10f);
        transform.position = Vector3.Slerp(transform.position, movement, (speedCamera * Time.deltaTime));
                    // Movimenta o background conforme o player anda
        backgroundEffect(player.position.x);
    }

    void backgroundEffect(float move){
        background.uvRect = new Rect((move / 500), 0, 1, 1);
        rectMove = background.uvRect.x;
    }
}
