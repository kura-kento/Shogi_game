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
        float x = (float)masu.transform.position.x;
        float y = (float)masu.transform.position.y;
        int index_x = (int)((3.0+x) / 1.5f);
        int index_y = (int)((3.0+y) / 1.5f);

        Debug.Log("マスの名前" + masu.name + "y:" + index_y.ToString() + "x:" + index_x.ToString());

        //コマが選択されている時 かつ　「選択可能マス」
        if(App.slot != null && masu.tag == "Select") {
            float koma_x = App.slot.transform.localPosition.x;
            float koma_y = App.slot.transform.localPosition.y;
            int koma_i_x = (int)((3.0 + koma_x) / 1.5f);
            int koma_i_y = (int)((3.0 + koma_y) / 1.5f);

            GameObject parent = App.slot.transform.parent.gameObject;
             //駒台
            if(parent.name == "Komadai") {
                Debug.Log("駒台から");
                 //盤上
            } else {
                Debug.Log("============================盤上から");
                App.masu_array[koma_i_y][koma_i_x] = 0; //移動した後は0になる
                
                //相手の駒の判定
                Koma enemmy_koma = GetChild(masu);
                Debug.Log("選択マスの中身:"+enemmy_koma);
                //駒有り　かつ　相手の駒の時
                if(enemmy_koma != null &&  enemmy_koma.number < 0) {
                    enemmy_koma.transform.parent = GameObject.Find("Komadai").transform;
                    enemmy_koma.tag = "Komadai";
                    int i = 6; //駒台の数によって置く位置を変更する
                    enemmy_koma.transform.localPosition = new Vector3(0, 0.40f - 0.2f * i, 0);
                    enemmy_koma.transform.Rotate(0, 0, 0);
                }
            
            }
            App.slot.transform.parent = masu.transform; //マスを親にする。
            masu.MasuStatus = 2;//今だけ
            masu.GetComponent<SpriteRenderer>().color = App.Masu_Color;

            //色とタグを戻す
            foreach (Masu obj in FindObjectsOfType<Masu>())
            {
                obj.tag = "Masu";
                obj.GetComponent<SpriteRenderer>().color = App.Masu_Color;
            }
            App.masu_array[index_y][index_x] = App.slot.number;
            App.slot.transform.localPosition = new Vector3(0, 0, 0);
            App.slot = null;

            Player.MasuStatusLog();
        }
    }

    Koma GetChild(Masu obj) {
	Transform children = obj.GetComponentInChildren<Transform>();
	//子要素がいなければ終了
	if (children.childCount == 0) {
		return null;
	}
    return obj.transform.GetChild(0).gameObject.GetComponent<Koma>();
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