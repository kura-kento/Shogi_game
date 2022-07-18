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
    public Player Player;

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
                // masu.name = "Masu" + i.ToString()+ "_" + j.ToString();
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
                    masu_koma.tag = "Komadai";
                    masu_koma.number *= -1;//ステータスを自分のコマに

                    Koma new_koma = App.isTurePlayer1 ?
                    Player.instance.CreateKoma(masu_koma.number):
                    Player2.instance.CreateKoma(masu_koma.number);

                    Destroy(masu_koma.transform.gameObject);
                    new_koma.transform.parent = komadai_obj.transform;
                }
            }
            App.slot.transform.parent = masu.transform; //マスを親にする。
            //使ったら駒台の位置を並び替える。
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

            // TODO:ここに勝利判定を入れる
            // 玉が取られる
            // 全駒
            // タッチダウン


            TurnEnd();//ダーン終了の処理
        }
    }

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