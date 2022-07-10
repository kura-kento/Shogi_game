using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Masu : MonoBehaviour
{
    public UnityAction<Masu> ClickAction;
    public int status;
    
    public static float hight = 1.5f;
    public static float width = 1.5f;
    // private void Start()
    // {
    //     Init(99);
    // }
        public void Init(int number)
    {
        this.status = number;
    }

    public void OnClickThis()
    {
        // Debug.Log("Masuが押されたよ！");
        // PalyerのSelectCardを実行　//外部に(player.cs)設定しているcard.ClickActionの処理を呼び出す？渡す？
        ClickAction.Invoke(this);
    }


    // // Update is called once per frame
    // void Update()
    // {
        
    // }

}
