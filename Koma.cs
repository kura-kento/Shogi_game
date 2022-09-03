using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using Photon.Pun;

public class Koma : MonoBehaviourPunCallbacks
{

    public int number;
    public TMP_Text text;
    public UnityAction<Koma> ClickAction;

    void Start () {
        
    }

    public void Init(int number)
    {
        this.number = number;
        
        text.text = GetText(number);
    }

    public void OnClickThis()
    {
        // Debug.Log("Komaが押されたよ！");
        // PalyerのSelectCardを実行　//外部に(player.cs)設定しているcard.ClickActionの処理を呼び出す？渡す？
        ClickAction.Invoke(this);
    }

    public string GetText(int number) {
        int abs_number = Mathf.Abs(number);

        switch(abs_number)
        {
            case 1: //変数が値Aのとき実行される
                return "玉";
            case 2:
                return "歩";
            case 3:
                return "香";
            case 4:
                return "桂";
            case 5:
                return "銀";
            case 6:
                return "金";
            case 7:
                return "飛";
            case 8:
                return "角";
            case 9:
                return "と";
            case 10:
                return "杏";//成香
            case 11:
                return "圭";//成桂
            case 12:
                return "全";//成銀
            case 13:
                return "龍";
            case 14:
                return "馬";
            default:
                return (number).ToString();
        }
    }
}
