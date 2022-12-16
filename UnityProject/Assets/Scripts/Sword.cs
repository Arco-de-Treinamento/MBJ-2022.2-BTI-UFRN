using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    
    Alien alienCollider;
    Predador predadorCollider;
    CactoVerde cactoCollider;

    void Start()
    {
        gameObject.SetActive(true);
    }

    public void attack(){
        gameObject.SetActive(true);
        if(alienCollider != null){
            alienCollider.TakeDamage(3);
        }
        if(predadorCollider != null){
            predadorCollider.TakeDamage(3);
        }
        if(cactoCollider != null){
            cactoCollider.TakeDamage(3);
        }
        
        StartCoroutine(performAttack());
    }

    private void OnTriggerEnter2D(Collider2D collision){
        alienCollider = collision.gameObject.GetComponent<Alien>();
        predadorCollider = collision.gameObject.GetComponent<Predador>();
        cactoCollider = collision.gameObject.GetComponent<CactoVerde>();
    }

    private IEnumerator performAttack(){
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(true);
    }

}
