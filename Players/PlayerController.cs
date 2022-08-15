using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{
    Vector3 select_1 = new Vector3(App.MASU_SIZE * -1.0f,App.MASU_SIZE *  1.0f, 0);
    Vector3 select_2 = new Vector3(    0,App.MASU_SIZE *  1.0f, 0);
    Vector3 select_3 = new Vector3(App.MASU_SIZE * 1.0f, App.MASU_SIZE * 1.0f, 0);
    Vector3 select_4 = new Vector3(App.MASU_SIZE * 1.0f,     0, 0);
    Vector3 select_5 = new Vector3(    0,     0, 0);
    Vector3 select_6 = new Vector3(App.MASU_SIZE * -1.0f,     0, 0);
    Vector3 select_7 = new Vector3(App.MASU_SIZE * -1.0f,App.MASU_SIZE * -1.0f, 0);
    Vector3 select_8 = new Vector3(    0,App.MASU_SIZE * -1.0f, 0);
    Vector3 select_9 = new Vector3(App.MASU_SIZE * 1.0f,App.MASU_SIZE * -1.0f, 0);
    bool isEnd = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //駒台から駒を【駒セット用】
    public void mySelectObj(bool isFirstPlayer) {
        if (App.game_type != GAME_TYPE.KOMA_SET) return; 
        foreach (Masu obj in FindObjectsOfType<Masu>())
        {
            if(Masu.isNoUseMasu(obj)) continue;

            if (obj.transform.childCount == 0 && obj.MasuStatus == (isFirstPlayer ? 1 : -1)) { //駒が置いていない場合(マスの子要素にデータが無い場合)
                //マスを選択できる状態にする
                GameMaster.SelectMasu(obj);
            }
        }
    }

    //駒台から駒を
    public void CreateSelectObj(bool isFirstPlayer) {
        Debug.Log(isFirstPlayer ? "駒台から駒を先手" : "駒台から駒を後手");
        if (!isMyTurn(isFirstPlayer) && App.game_type == GAME_TYPE.BATTLE) return; //ターンプレイヤーでない場合何もしない
        
        foreach (Masu obj in FindObjectsOfType<Masu>())
        {
            Debug.Log(obj.name);
            Debug.Log(Masu.isNoUseMasu(obj));
            if(Masu.isNoUseMasu(obj)) continue;

            if (obj.transform.childCount == 0) { //駒が置いていない場合(マスの子要素にデータが無い場合)
                //マスを選択できる状態にする
                GameMaster.SelectMasu(obj);
            }
        }
    }

    //盤上から駒をクリック時
    public void SelectObj(Koma koma, bool isFirstPlayer) {
        // Debug.Log(isFirstPlayer ? "先手" : "後手");
        if (!isMyTurn(isFirstPlayer) && App.game_type == GAME_TYPE.BATTLE) return; //ターンプレイヤーでない場合何もしない
        Debug.Log(isFirstPlayer ? "盤上から駒をクリック時:先手" : "盤上から駒をクリック時:後手");
        // if (App.isTurePlayer1 && koma.number < 0) return; //先手 かつ 駒が相手
        var koma_p = koma.transform.parent.transform.localPosition;
        Debug.Log("マスの位置" + koma_p.y.ToString());
        //歩を押した時  
        switch(Mathf.Abs(koma.number)) {
            case 1: //王
                Vector3 select_ou_1 = koma_p + (koma.number > 0 ? select_1 : App.ReverseVector(select_1));
                Vector3 select_ou_2 = koma_p + (koma.number > 0 ? select_2 : App.ReverseVector(select_2));
                Vector3 select_ou_3 = koma_p + (koma.number > 0 ? select_3 : App.ReverseVector(select_3));
                Vector3 select_ou_4 = koma_p + (koma.number > 0 ? select_4 : App.ReverseVector(select_4));
                Vector3 select_ou_6 = koma_p + (koma.number > 0 ? select_6 : App.ReverseVector(select_6));
                Vector3 select_ou_7 = koma_p + (koma.number > 0 ? select_7 : App.ReverseVector(select_7));
                Vector3 select_ou_8 = koma_p + (koma.number > 0 ? select_8 : App.ReverseVector(select_8));
                Vector3 select_ou_9 = koma_p + (koma.number > 0 ? select_9 : App.ReverseVector(select_9));
                SelectMasuCheck(new Vector3[] { select_ou_1,select_ou_2,select_ou_3,select_ou_4,select_ou_6,select_ou_7,select_ou_8,select_ou_9 });
                break;
            case 2: //歩
                Vector3 select_fu = koma_p + (koma.number > 0 ? select_2 : App.ReverseVector(select_2));  
                Debug.Log("歩の移動先の座標："+select_fu.ToString());
                // Vector3 select_fu = new Vector3(-13.95f, -4.65f, 90.00f);
                SelectMasuCheck(new Vector3[] { select_fu });
                break;
            case 3: //香車
                Debug.Log("香車");
                isEnd = false;
                Vector3 select_kyo = koma_p + (koma.number > 0 ? select_2 : App.ReverseVector(select_2));
                for (int i=0; i<10; i++)
                {
                    isEnd = SelectMasuCheck(new Vector3[] { select_kyo });
                    if(isEnd == true) break;
                    select_kyo += (koma.number > 0 ? select_2 : App.ReverseVector(select_2));
                }
                Debug.Log(select_kyo);
                break;
            case 4: //桂馬
                Vector3 select_left  = new Vector3(App.MASU_SIZE * -1.0f,App.MASU_SIZE * 2.0f, 0);
                Vector3 select_right = new Vector3(App.MASU_SIZE * 1.0f,App.MASU_SIZE * 2.0f, 0);

                Vector3 select_ke_left  = koma_p + (isFirstPlayer ? select_left  : App.ReverseVector(select_left));
                Vector3 select_ke_right = koma_p + (isFirstPlayer ? select_right : App.ReverseVector(select_right));

                foreach (Masu obj in FindObjectsOfType<Masu>())
                {
                    Koma masu_koma = App.GetChildKoma(obj);
                    if (obj.transform.localPosition == select_ke_left || obj.transform.localPosition == select_ke_right) {

                        // 駒が無い　または　(先攻+マイナス駒　または　後攻+プラス駒)
                        if(!Masu.isNoUseMasu(obj) && (masu_koma == null || (App.isTurePlayer1 &&  masu_koma.number < 0) || (App.isTurePlayer1 == false && masu_koma.number > 0))) {
                            GameMaster.SelectMasu(obj); //マスを選択できる状態にする
                        }
                    }
                }
                break;

            case 5: //銀
                Vector3 select_gin_1 = koma_p + (koma.number > 0 ? select_1 : App.ReverseVector(select_1));  
                Vector3 select_gin_2 = koma_p + (koma.number > 0 ? select_2 : App.ReverseVector(select_2));
                Vector3 select_gin_3 = koma_p + (koma.number > 0 ? select_3 : App.ReverseVector(select_3));
                Vector3 select_gin_7 = koma_p + (koma.number > 0 ? select_7 : App.ReverseVector(select_7));
                Vector3 select_gin_9 = koma_p + (koma.number > 0 ? select_9 : App.ReverseVector(select_9));
                SelectMasuCheck(new Vector3[] { select_gin_1,select_gin_2,select_gin_3,select_gin_7,select_gin_9});
                break;

            case 6:  //金
            case 9:  //と金
            case 10: //香車金
            case 11: //桂馬金
            case 12: //銀金
                Vector3 select_kin_1 = koma_p + (koma.number > 0 ? select_1 : App.ReverseVector(select_1));  
                Vector3 select_kin_2 = koma_p + (koma.number > 0 ? select_2 : App.ReverseVector(select_2));
                Vector3 select_kin_3 = koma_p + (koma.number > 0 ? select_3 : App.ReverseVector(select_3));
                Vector3 select_kin_4 = koma_p + (koma.number > 0 ? select_4 : App.ReverseVector(select_4));
                Vector3 select_kin_6 = koma_p + (koma.number > 0 ? select_6 : App.ReverseVector(select_6));
                Vector3 select_kin_8 = koma_p + (koma.number > 0 ? select_8 : App.ReverseVector(select_8));
                SelectMasuCheck(new Vector3[] { select_kin_1,select_kin_2,select_kin_3,select_kin_4,select_kin_6,select_kin_8 });
                break;

            case 7: //飛車
                isEnd = false;
                Vector3 select_hi_top    = koma_p + (koma.number > 0 ? select_2 : App.ReverseVector(select_2));
                Vector3 select_hi_bottom = koma_p + (koma.number > 0 ? select_8 : App.ReverseVector(select_8));
                Vector3 select_hi_left   = koma_p + (koma.number > 0 ? select_4 : App.ReverseVector(select_4));
                Vector3 select_hi_right  = koma_p + (koma.number > 0 ? select_6 : App.ReverseVector(select_6));
                
                for (int i=0; i<10; i++)
                {
                    isEnd = SelectMasuCheck(new Vector3[] { select_hi_top });
                    if(isEnd == true) break;
                    select_hi_top += (koma.number > 0 ? select_2 : App.ReverseVector(select_2));
                }
                for (int i=0; i<10; i++)
                {
                    isEnd = SelectMasuCheck(new Vector3[] { select_hi_bottom });
                    if(isEnd == true) break;
                    select_hi_bottom += (koma.number > 0 ? select_8 : App.ReverseVector(select_8));
                }
                for (int i=0; i<10; i++)
                {
                    isEnd = SelectMasuCheck(new Vector3[] { select_hi_left });
                    if(isEnd == true) break;
                    select_hi_left += (koma.number > 0 ? select_4 : App.ReverseVector(select_4));
                }
                for (int i=0; i<10; i++)
                {
                    isEnd = SelectMasuCheck(new Vector3[] { select_hi_right });
                    if(isEnd == true) break;
                    select_hi_right += (koma.number > 0 ? select_6 : App.ReverseVector(select_6));
                }
                break;
            case 8: //角
                isEnd = false;
                Vector3 select_hi_top_left     = koma_p + (koma.number > 0 ? select_1 : App.ReverseVector(select_1));
                Vector3 select_hi_top_right    = koma_p + (koma.number > 0 ? select_3 : App.ReverseVector(select_3));
                Vector3 select_hi_bottom_left  = koma_p + (koma.number > 0 ? select_7 : App.ReverseVector(select_7));
                Vector3 select_hi_bottom_right = koma_p + (koma.number > 0 ? select_9 : App.ReverseVector(select_9));
                
                for (int i=0; i<10; i++)
                {
                    isEnd = SelectMasuCheck(new Vector3[] { select_hi_top_left });
                    if(isEnd == true) break;
                    select_hi_top_left += (koma.number > 0 ? select_1 : App.ReverseVector(select_1));
                }
                for (int i=0; i<10; i++)
                {
                    isEnd = SelectMasuCheck(new Vector3[] { select_hi_top_right });
                    if(isEnd == true) break;
                    select_hi_top_right += (koma.number > 0 ? select_3 : App.ReverseVector(select_3));
                }
                for (int i=0; i<10; i++)
                {
                    isEnd = SelectMasuCheck(new Vector3[] { select_hi_bottom_left });
                    if(isEnd == true) break;
                    select_hi_bottom_left += (koma.number > 0 ? select_7 : App.ReverseVector(select_7));
                }
                for (int i=0; i<10; i++)
                {
                    isEnd = SelectMasuCheck(new Vector3[] { select_hi_bottom_right });
                    if(isEnd == true) break;
                    select_hi_bottom_right += (koma.number > 0 ? select_9 : App.ReverseVector(select_9));
                }
                break;
            case 13: //龍王
                isEnd = false;
                Vector3 select_ryu_top    = koma_p + (koma.number > 0 ? select_2 : App.ReverseVector(select_2));
                Vector3 select_ryu_bottom = koma_p + (koma.number > 0 ? select_8 : App.ReverseVector(select_8));
                Vector3 select_ryu_left   = koma_p + (koma.number > 0 ? select_4 : App.ReverseVector(select_4));
                Vector3 select_ryu_right  = koma_p + (koma.number > 0 ? select_6 : App.ReverseVector(select_6));
                Vector3 select_ryu_1 = koma_p + (koma.number > 0 ? select_1 : App.ReverseVector(select_1));  
                Vector3 select_ryu_3 = koma_p + (koma.number > 0 ? select_3 : App.ReverseVector(select_3));
                Vector3 select_ryu_7 = koma_p + (koma.number > 0 ? select_7 : App.ReverseVector(select_7));
                Vector3 select_ryu_9 = koma_p + (koma.number > 0 ? select_9 : App.ReverseVector(select_9));

                SelectMasuCheck(new Vector3[] { select_ryu_1,select_ryu_3,select_ryu_7,select_ryu_9 });
                for (int i=0; i<10; i++)
                {
                    isEnd = SelectMasuCheck(new Vector3[] { select_ryu_top });
                    if(isEnd == true) break;
                    select_ryu_top += (koma.number > 0 ? select_2 : App.ReverseVector(select_2));
                }
                for (int i=0; i<10; i++)
                {
                    isEnd = SelectMasuCheck(new Vector3[] { select_ryu_bottom });
                    if(isEnd == true) break;
                    select_ryu_bottom += (koma.number > 0 ? select_8 : App.ReverseVector(select_8));
                }
                for (int i=0; i<10; i++)
                {
                    isEnd = SelectMasuCheck(new Vector3[] { select_ryu_left });
                    if(isEnd == true) break;
                    select_ryu_left += (koma.number > 0 ? select_4 : App.ReverseVector(select_4));
                }
                for (int i=0; i<10; i++)
                {
                    isEnd = SelectMasuCheck(new Vector3[] { select_ryu_right });
                    if(isEnd == true) break;
                    select_ryu_right += (koma.number > 0 ? select_6 : App.ReverseVector(select_6));
                }
                break;
            case 14: //龍馬
                isEnd = false;
                Vector3 select_uma_top_left     = koma_p + (koma.number > 0 ? select_1 : App.ReverseVector(select_1));
                Vector3 select_uma_top_right    = koma_p + (koma.number > 0 ? select_3 : App.ReverseVector(select_3));
                Vector3 select_uma_bottom_left  = koma_p + (koma.number > 0 ? select_7 : App.ReverseVector(select_7));
                Vector3 select_uma_bottom_right = koma_p + (koma.number > 0 ? select_9 : App.ReverseVector(select_9));
                Vector3 select_uma_2 = koma_p + (koma.number > 0 ? select_2 : App.ReverseVector(select_2));
                Vector3 select_uma_4 = koma_p + (koma.number > 0 ? select_4 : App.ReverseVector(select_4));
                Vector3 select_uma_6 = koma_p + (koma.number > 0 ? select_6 : App.ReverseVector(select_6));
                Vector3 select_uma_8 = koma_p + (koma.number > 0 ? select_8 : App.ReverseVector(select_8));
                SelectMasuCheck(new Vector3[] { select_uma_2,select_uma_4,select_uma_6,select_uma_8 });
                for (int i=0; i<10; i++)
                {
                    isEnd = SelectMasuCheck(new Vector3[] { select_uma_top_left });
                    if(isEnd == true) break;
                    select_uma_top_left += (koma.number > 0 ? select_1 : App.ReverseVector(select_1));
                }
                for (int i=0; i<10; i++)
                {
                    isEnd = SelectMasuCheck(new Vector3[] { select_uma_top_right });
                    if(isEnd == true) break;
                    select_uma_top_right += (koma.number > 0 ? select_3 : App.ReverseVector(select_3));
                }
                for (int i=0; i<10; i++)
                {
                    isEnd = SelectMasuCheck(new Vector3[] { select_uma_bottom_left });
                    if(isEnd == true) break;
                    select_uma_bottom_left += (koma.number > 0 ? select_7 : App.ReverseVector(select_7));
                }
                for (int i=0; i<10; i++)
                {
                    isEnd = SelectMasuCheck(new Vector3[] { select_uma_bottom_right });
                    if(isEnd == true) break;
                    select_uma_bottom_right += (koma.number > 0 ? select_9 : App.ReverseVector(select_9));
                }
                break;
        }
    }

    public bool isMyTurn(bool isFirstPlayer) {
        return App.isTurePlayer1 == isFirstPlayer;
    }

    //条件に合うマスがあるか？　処理が完了したらtrueを返す
    public bool SelectMasuCheck(Vector3[] selectArray) {
        bool isEnd = false;
        // Debug.Log("条件に合うマスがあるか？:SelectMasuCheck");
        // Debug.Log(FindObjectsOfType<Masu>().Length);
        foreach (Masu obj in FindObjectsOfType<Masu>())
        {
            // セットした座標か？
            if (Array.IndexOf(selectArray, obj.transform.localPosition) >= 0) {
                
                Koma masu_koma = App.GetChildKoma(obj);
                // 駒が無い
                if(Masu.isNoUseMasu(obj)) {
                    isEnd = true; //処理を止める
                }
                else if(masu_koma == null) {
                    GameMaster.SelectMasu(obj); //マスを選択できる状態にする
                    isEnd = false;
                }
                // 相手の駒
                else if((App.isTurePlayer1 &&  masu_koma.number < 0) || (App.isTurePlayer1 == false && masu_koma.number > 0)){
                    GameMaster.SelectMasu(obj); //マスを選択できる状態にする
                    isEnd = true; //処理を止める
                }
                // 自分の駒
                else if((App.isTurePlayer1 &&  masu_koma.number > 0) || (App.isTurePlayer1 == false && masu_koma.number < 0)){
                    isEnd = true; //処理を止める
                }
                else{
                    throw new System.Exception("例外発生");
                }
            }
        }
        return isEnd;
    }
}
