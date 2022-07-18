using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class Koma : MonoBehaviour
{

    public int number;
    public TMP_Text text;
    public UnityAction<Koma> ClickAction;

    void Start () {
        
    }

    public void Init(int number)
    {
        this.number = number;

        if(Mathf.Abs(number) == 2)
        {
            text.text = "歩";
        }else{
            text.text = (number).ToString();
        }
    }

    public void OnClickThis()
    {
        Debug.Log("Komaが押されたよ！");
        // PalyerのSelectCardを実行　//外部に(player.cs)設定しているcard.ClickActionの処理を呼び出す？渡す？
        ClickAction.Invoke(this);
    }
}
