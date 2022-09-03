using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using TMPro;

// 場所
public enum GAME_TYPE
{
    SET,    // 並べる
    WAIT,
    BATTLE,  // 対戦中
}

// 先手：後手
public enum PLAYER_TYPE
{
    NULL,
    FIRST,   // 先手
    SECOND,  // 後手
}
 

public class App : MonoBehaviour
{
    public static GAME_TYPE game_type = GAME_TYPE.SET;
    public static List<Dictionary<string, string>> SET_ACTION = new List<Dictionary<string, string>>{};
    // public static int[][] masu_array = new int[4][];

    public static int MAX_Y = 4;//固定値
    public static int MAX_X = 4;//固定値
   
    public static float MASU_SIZE = 275f;//固定値
    public static string KOMADAI1_NAME = "Komadai1";//固定値
    public static string KOMADAI2_NAME = "Komadai2";//固定値

    //色
    public static Color32 SELECT_COLOR = new Color32(248, 168, 146, 200);
    public static Color32 MASU_COLOR   = new Color32(212, 187, 99, 255);
    
    // int int_x = 4;
    [SerializeField] private GameObject TurnText;
    public static GameObject TurnObject;

    public static Koma slot = null;
    public static bool isTurePlayer1 = true;
    public static int Turn = 1;

    void Awake() {
        TurnObject = TurnText;
    }

    void Start() {

    }

    //マスに置いているコマの情報を取得する。
    public static Koma GetChildKoma(GameObject obj, string koma_name = null) {
        Debug.Log("マスに置いているコマの情報を取得する。");
        Transform children = obj.GetComponentInChildren<Transform>();

        if (children.childCount == 0) {
            Debug.Log("子要素がいなければ終了");
            return null;
        } else if (children.childCount == 1) {
            return obj.transform.GetChild(0).gameObject.GetComponent<Koma>();
        }
        Koma koma;
        int index = 0;
        foreach(var child in children) {
            koma = obj.transform.GetChild(index).gameObject.GetComponent<Koma>();
            Debug.Log(new Koma().GetText(koma.number));
            if(koma.number.ToString() == koma_name) {
                return koma;
            }
            index++;
        }
        return null;
    }


    //相手の駒か？(0がnullにしても良いかも)
    public static bool isEnemyKoma(Koma koma) {
        //先攻のターン　かつ　駒がマイナス
        if(App.isTurePlayer1 &&  koma.number < 0){
            return true;
        }
        //後攻のターン　かつ　駒がプラス
        if(App.isTurePlayer1 == false && koma.number > 0){
            return true;
        }
        return false;
    }

    //手数を表示させる(更新)
    public static void turnUp() {
        TMP_Text Turn_Text = TurnObject.GetComponent<TMP_Text>();
        Turn_Text.SetText((App.isTurePlayer1 ? "先手" : "後手") + Turn.ToString());
    }

    //向きを反転する。
    public static Vector3 ReverseVector(Vector3 vector3) {
        return new Vector3(-(vector3.x), -(vector3.y), vector3.z);
    }

    
  

//色を変える
//obj.GetComponent<SpriteRenderer>().color = App.MASU_COLOR;

//objectからKomaに変換
//obj.GetComponent<Koma>();

//配列に当てはまっている
//Array.IndexOf(evolutionList, App.slot.number) >= 0 

//文字
//str.Contains("にゃん")
}
