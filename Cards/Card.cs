using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
    // TODO : 役職
public class Card : MonoBehaviour
{

    public int number;
    public TMP_Text text;
    public UnityAction<Card> ClickAction;

    // private void Start()
    // {
    //     Init(99);
    // }

    public void Init(int number) 
    {
        this.number = number;
        text.text = number.ToString();
    }

    public void OnClickThis()
    {
        Debug.Log("Cardが押されたよ！");
        // PalyerのSelectCardを実行　//外部に(player.cs)設定しているcard.ClickActionの処理を呼び出す？渡す？
        ClickAction.Invoke(this);
    }


    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
