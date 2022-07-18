using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasuGenerator : MonoBehaviour
{

    [SerializeField] Masu masuPrefab;


    // どこからでも使えるようにする
    public static MasuGenerator instance;
    public static int int_y = 4;
    public static int int_x = 4;

    private void Awake()
    {
        Spawn();
        instance = this;
    }

    public void Spawn() 
    {
        for(int i=0; i<int_y; i++)
        {
            for(int j=0; j<int_x; j++) {
                Masu masu = Instantiate(masuPrefab);
                float y = i - 4 / 2f;
                float x = j - 4 / 2f;
                masu.transform.localPosition = new Vector3(x * Masu.width, y *Masu.hight, 0);
                masu.name = "Masu" + i.ToString()+ "_" + j.ToString();
                masu.name = "Masu";
                masu.Init(0);
                masu.ClickAction = SelectMasu; //クリックされた時関数を呼ぶ
            }
        }
    }

    public void SelectMasu(Masu masu)
    {
        //コマが選択されている時 かつ　「選択可能マス」
        if(App.slot != null && masu.tag == "Select") {

            GameObject parent = App.slot.transform.parent.gameObject;

            string komadai_name = App.isTurePlayer1 ? "Komadai" : "Komadai2";
            GameObject komadai_obj = GameObject.Find(komadai_name);
             //駒台
            if(parent.name == "Komadai") {
                Debug.Log("駒台から");

                //盤上
            } else {
                Debug.Log("盤上から");
                
                //選択したマスの駒情報 取得
                Koma masu_koma = App.GetChildKoma(masu);
                //駒有り　かつ　相手の駒の時
                if(masu_koma != null &&  App.isEnemyKoma(masu_koma)) {
                    //駒を取った時の処理
                    masu_koma.transform.parent = komadai_obj.transform;
                    masu_koma.tag = "Komadai";
                    masu_koma.number *= -1;//ステータスを自分のコマに
                    int koma_count = komadai_obj.transform.childCount; //駒台の数によって置く位置を変更する
                    masu_koma.transform.localPosition = new Vector3(0, KomadaiVectorY(koma_count), 0);
                    masu_koma.transform.Rotate(0, 0, 180f);
                }
            
            }
            App.slot.transform.parent = masu.transform; //マスを親にする。
            masu.MasuStatus = 2; //今だけ
            masu.GetComponent<SpriteRenderer>().color = App.Masu_Color;

            //使ったら駒台の位置を並び替える。TODO: コマ台の操作のみ
            var koma_children = komadai_obj.GetComponentsInChildren<Koma>();
            int i = 0;
            foreach(Koma koma_child in koma_children) {
                koma_child.transform.localPosition = new Vector3(0, KomadaiVectorY(i), 0);
                i++;
            }

            //マスの状態をリセットする
            GameMaster.ResetMasu();
 
            App.slot.transform.localPosition = new Vector3(0, 0, 0);
            App.slot = null;

            TurnEnd();//ダーン終了の処理
        }
    }

    // Koma GetChild(Masu obj) {
    //     Transform children = obj.GetComponentInChildren<Transform>();
    //     //子要素がいなければ終了
    //     if (children.childCount == 0) {
    //         return null;
    //     }
    //     return obj.transform.GetChild(0).gameObject.GetComponent<Koma>();
    // }
    //駒台の座標
    public float KomadaiVectorY(int i) {
        return App.isTurePlayer1 ? 0.40f -0.1f*i : -0.4f + 0.1f*i;
    }

    //ダーン終了の処理
    public void TurnEnd() {
        App.isTurePlayer1 = !(App.isTurePlayer1);
        App.Turn++; //ターン数増やす
        App.turnUp();
    }
}

// RuleController
// Masu int[4,6] = {};空配列 0 -id +id
// 生成の処理 この一回しか使用しない為ここに追記
// init() {}

// App
// Koma slot = null;
// Awake()
// List<int> komas = List<int>(); 台に
// int komas[5] = [1,2,2,3,4]; 固定
// for(i=0; i<5; i++;)

// Update() { }
// if(click) { //何かに
// 配列の値を取得（0,-n,+n,99）
//    if(slot) { 
//      0の時、上書き処理
//      slot=null;
//    }
//    else { //slotに何もない
//      opacity=0.5;
//      slot=komaの座標のobjectを取得;
//    }
// }


// KomaGenerator
// public Koma spawn(i) {if(i===1){komaPrefab_fu}}
// 取られたとき削除してスポーン
// komaPrefab
// komaPrefab
// GameObject[] komas = new GameObject[3];
// [serializefield]  komaPrefab_fu;