using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BalaController{
    [SerializeField] private TextMeshProUGUI balaText;
    private int quantidadeBala;

    public BalaController([SerializeField] TextMeshProUGUI balaText){
        this.balaText = balaText;
        this.setQuantidadeBala(10);
    }

    public void setQuantidadeBala(int quantidade){
        this.quantidadeBala = quantidade;
        balaText.text = this.quantidadeBala.ToString();
    }

    public void atirar(){
        this.setQuantidadeBala(this.quantidadeBala-1);
    }

    public int getQuantidadeBala(){
        return this.quantidadeBala;
    }

    public void catchBullet(){
        int increaseBullet = Random.Range(1,3);
        this.setQuantidadeBala(this.quantidadeBala + increaseBullet);
    }

}
