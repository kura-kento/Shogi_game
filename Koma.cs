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
        int abs_number = Mathf.Abs(number);

        switch(abs_number)
        {
            case 1: //変数が値Aのとき実行される
                text.text = "玉";
                break;
            case 2:
                text.text = "歩";
                break;
            case 3:
                text.text = "香";
                break;
            case 4:
                text.text = "桂";
                break;
            case 7:
                text.text = "飛";
                break;
            case 8:
                text.text = "角";
                break;
            default:
                text.text = (number).ToString();
                break;
        }
    }

    public void OnClickThis()
    {
        // Debug.Log("Komaが押されたよ！");
        // PalyerのSelectCardを実行　//外部に(player.cs)設定しているcard.ClickActionの処理を呼び出す？渡す？
        ClickAction.Invoke(this);
    }
}
