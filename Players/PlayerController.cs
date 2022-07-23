using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    Vector3 select_1 = new Vector3(-1.0f,  1.0f, 0);
    Vector3 select_2 = new Vector3(    0,  1.0f, 0);
    Vector3 select_3 = new Vector3( 1.0f,  1.0f, 0);
    Vector3 select_4 = new Vector3( 1.0f,     0, 0);
    Vector3 select_5 = new Vector3(    0,     0, 0);
    Vector3 select_6 = new Vector3(-1.0f,     0, 0);
    Vector3 select_7 = new Vector3(-1.0f, -1.0f, 0);
    Vector3 select_8 = new Vector3(    0, -1.0f, 0);
    Vector3 select_9 = new Vector3( 1.0f, -1.0f, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //駒台から駒を
    public void CreateSelectObj(bool isFirstPlayer) {
        // Debug.Log(isFirstPlayer ? "先手" : "後手");
        if (!isMyTurn(isFirstPlayer)) return; //ターンプレイヤーでない場合何もしない
        foreach (Masu obj in FindObjectsOfType<Masu>())
        {
            if (obj.transform.childCount == 0) { //駒が置いていない場合(マスの子要素にデータが無い場合)
                //マスを選択できる状態にする
                GameMaster.SelectMasu(obj);
            }
        }
    }

    //盤上から駒をクリック時
    public void SelectObj(Koma koma, bool isFirstPlayer) {
        if (!isMyTurn(isFirstPlayer)) return; //ターンプレイヤーでない場合何もしない
        // if (App.isTurePlayer1 && koma.number < 0) return; //先手 かつ 駒が相手
        var koma_p = koma.transform.position;
        //歩を押した時  
        switch(Mathf.Abs(koma.number)) {
            case 2: //歩
                Vector3 select_fu = koma_p + (koma.number > 0 ? select_2 : App.ReverseVector(select_2));  
                SelectMasuCheck(new Vector3[] { select_fu });
                break;
            case 3: //香車
                bool isEnd = false;
                Vector3 select_kyo = koma_p + (koma.number > 0 ? select_2 : App.ReverseVector(select_2));
                for (int i=0; i<10; i++)
                {
                    isEnd = SelectMasuCheck(new Vector3[] { select_kyo });
                    if(isEnd == true) break;
                    select_kyo += (koma.number > 0 ? select_2 : App.ReverseVector(select_2));
                }
                break;

                case 4: //桂馬
                Vector3 select_left  = new Vector3(-1.0f, 2.0f, 0);
                Vector3 select_right = new Vector3( 1.0f, 2.0f, 0);

                Vector3 select_ke_left  = koma_p + (isFirstPlayer ? select_left  : App.ReverseVector(select_left));
                Vector3 select_ke_right = koma_p + (isFirstPlayer ? select_right : App.ReverseVector(select_right));

                foreach (Masu obj in FindObjectsOfType<Masu>())
                {
                    Koma masu_koma = App.GetChildKoma(obj);
                    // 駒が無い　または　(先攻+マイナス駒　または　後攻+プラス駒)
                    if(masu_koma == null || (App.isTurePlayer1 &&  masu_koma.number < 0) || (App.isTurePlayer1 == false && masu_koma.number > 0)) {
                        // 上方向に１つ上のマスのみ指定する
                        if (obj.transform.position == select_ke_left || obj.transform.position == select_ke_right) {
                            GameMaster.SelectMasu(obj); //マスを選択できる状態にする
                        }
                    }
                }
                break;

            //     case 5: //銀
            //     select_1 = new Vector3(koma_p.x + interval*-1.0f, koma_p.y + interval*2.0f, koma_p.z);
            //     select_2 = new Vector3(koma_p.x + interval*1.0f, koma_p.y + interval*2.0f, koma_p.z);
            //     select_3 = new Vector3(koma_p.x + interval*1.0f, koma_p.y + interval*2.0f, koma_p.z);
            //     select_7 = new Vector3(koma_p.x + interval*-1.0f, koma_p.y + interval*2.0f, koma_p.z);
            //     select_9 = new Vector3(koma_p.x + interval*1.0f, koma_p.y + interval*2.0f, koma_p.z);

            //     foreach (Masu obj in FindObjectsOfType<Masu>())
            //     {
            //         Koma masu_koma = App.GetChildKoma(obj);
            //         // 駒が無い　または　(先攻+マイナス駒　または　後攻+プラス駒)
            //         if(masu_koma == null || (App.isTurePlayer1 &&  masu_koma.number < 0) || (App.isTurePlayer1 == false && masu_koma.number > 0)) {
            //             // 上方向に１つ上のマスのみ指定する
            //             if (obj.transform.position == select_1) {
            //                 GameMaster.SelectMasu(obj); //マスを選択できる状態にする
            //             }
            //         }
            //     }
            //     break;
        }

    }

    public bool isMyTurn(bool isFirstPlayer) {
        return App.isTurePlayer1 == isFirstPlayer;
    }

    //条件に合うマスがあるか？　処理が完了したらtrueを返す
    public bool SelectMasuCheck(Vector3[] selectArray) {
        bool isEnd = false;
        foreach (Masu obj in FindObjectsOfType<Masu>())
        {
            // セットした座標か？
            if (Array.IndexOf(selectArray, obj.transform.position) >= 0) {
                Koma masu_koma = App.GetChildKoma(obj);
                // 駒が無い
                if(masu_koma == null) {
                    GameMaster.SelectMasu(obj); //マスを選択できる状態にする
                    isEnd = false; //相手の駒の場合
                    Debug.Log("駒が無い");
                }
                // 相手の駒
                else if((App.isTurePlayer1 &&  masu_koma.number < 0) || (App.isTurePlayer1 == false && masu_koma.number > 0)){
                    GameMaster.SelectMasu(obj); //マスを選択できる状態にする
                    isEnd = true; //処理を止める
                    Debug.Log("相手の駒");
                }
                // 自分の駒
                else if((App.isTurePlayer1 &&  masu_koma.number > 0) || (App.isTurePlayer1 == false && masu_koma.number < 0)){
                    isEnd = true; //処理を止める
                    Debug.Log("自分の駒");
                }
                else{
                    throw new System.Exception("例外発生");
                }
            }
        }
        Debug.Log("isEnd:"+ (isEnd ? "True":"False"));
        return isEnd;
    }
}
