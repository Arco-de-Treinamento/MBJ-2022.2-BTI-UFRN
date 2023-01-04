using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CachacaController{
    private int quantidadeCachaca;
    [SerializeField] private TextMeshProUGUI cachacaText;

    public CachacaController([SerializeField] TextMeshProUGUI cachacaText){
        this.cachacaText = cachacaText;
        this.setQuantidadeCachaca(10);
    }

    public void setQuantidadeCachaca(int quantidade){
        this.quantidadeCachaca = quantidade;
        this.cachacaText.text = this.quantidadeCachaca.ToString();
    }

    public void catchCachaca(){
        int increaseCachaca = Random.Range(1,3);
        this.setQuantidadeCachaca(this.quantidadeCachaca + increaseCachaca);
    }

    public bool useCachaca(){
        if(quantidadeCachaca <= 0) return false;
        setQuantidadeCachaca(quantidadeCachaca - 1);
        return true;
    }

}
