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

    // public void Start()
    // {

        
    // }

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
                masu.Init(0);
                masu.ClickAction = SelectMasu; //クリックされた時関数を呼ぶ
            }
        }
    }

    public void SelectMasu(Masu masu)
    {
        float x = (float)masu.transform.position.x;
        float y = (float)masu.transform.position.y;
        Debug.Log("マスの名前" + masu.name + "y:" + y.ToString() + "x:" + x.ToString());
        // App.masu_array[y][x] = 99;
//0_0 = -3,-3
        Debug.Log("コマ位置：" + masu.transform.position + "コマ名前" + masu.name);
        //コマが選択されている時
        if(App.slot != null) {
            App.slot.transform.localPosition = new Vector3(x*1.0f, y*1.0f, 0);
            App.slot = null;
            Debug.Log("スロット => 空");
        }
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