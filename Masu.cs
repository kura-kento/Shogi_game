using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Masu : MonoBehaviour
{
    public UnityAction<Masu> ClickAction;
    public int MasuStatus;

    // public static int status = 0;
    public static float hight = 1.0f;
    public static float width = 1.0f;
    // private void Start()
    // {
    //     Init(99);
    // }
        public void Init(int number)
    {
        this.MasuStatus = number;
    }

    public void OnClickThis()
    {
        // Debug.Log("Masuが押されたよ！");
        // PalyerのSelectCardを実行　//外部に(player.cs)設定しているcard.ClickActionの処理を呼び出す？渡す？
        ClickAction.Invoke(this);
    }


    //使用不可パネルの場合
    public static bool isNoUseMasu(Masu masu) {
        return masu.tag == "NoUse";
    }
}
