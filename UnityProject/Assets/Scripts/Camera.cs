using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public float speedCamera;
    public float yOffesetCamera;
    public Transform player;

    // Update is called once per frame
    void Update(){
        // Movimentacao da camera com delay
        Vector3 movement = new Vector3(player.position.x, (player.position.y + yOffesetCamera), -10f);
        transform.position = Vector3.Slerp(transform.position, movement, (speedCamera * Time.deltaTime));
    }
}
