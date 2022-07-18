using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Linq;

public class GameMaster : MonoBehaviour
{

    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    //マスのステータス(ログ)
    public static void MasuStatusLog() {
        for(int i=0; i<App.MAX_Y;i++)
        {
            string[] intArray = Array.ConvertAll(App.masu_array[App.MAX_Y-i-1], s => {return s.ToString();});
            Debug.Log((App.MAX_Y-i).ToString()+"行目"+string.Join(",", intArray)); 
        }
    }

    //全てのマスの選択をキャンセルする
    public static void ResetMasu() {
        foreach (Masu masu in FindObjectsOfType<Masu>())
        {
            masu.tag = "Masu";
            masu.GetComponent<SpriteRenderer>().color = App.Masu_Color;
            masu.GetComponent<Renderer>().sortingLayerName = "back++";
        }
    }
    //マスを選択させる
    public static void SelectMasu(Masu masu) {
        masu.tag = "Select";
        masu.GetComponent<SpriteRenderer>().color = App.Select_Color;
        masu.GetComponent<Renderer>().sortingLayerName = "front++"; 
    }

    //駒台の順番 
    // public static void KomadaiSetPosition(Masu masu) {
    //     masu.tag = "Select";
    //     masu.GetComponent<SpriteRenderer>().color = App.Select_Color;
    //     masu.GetComponent<Renderer>().sortingLayerName = "front++"; 
    // }
}
