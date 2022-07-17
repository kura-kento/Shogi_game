using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Linq;

public class Player : MonoBehaviour
{
    private GameObject parent;
    //カメラ情報
    private Camera camera_object;
    private RaycastHit hit;
    // List<int> komas = new List<int>();
    int[] player_1 = new int[] { 2, 2, 2, 3, 4 };
    string[] test = new string[] {"1","2","3"};

    // 決定ぼたんを押したら,Game Masterに通知する

    private void Start()
    {
        //カメラ情報を取得
        camera_object = GameObject.Find("Main Camera").GetComponent<Camera>();

        SetupKomadai();//盤上から置く時はOK
 
        MasuStatusLog();
    }

    void Update()
    {
       if (Input.GetMouseButtonDown(0)) {

       }
    }

    //初期配置
    public void SetupKomadai()
    {
        GameObject komadai = GameObject.Find("Komadai");
        //自身の駒
        for(int i=0;i<5;i++) {
            Koma Fu = KomaGenerator.instance.Spawn(player_1[i]);
            Fu.transform.parent = GameObject.Find("Komadai").transform;
            Fu.tag = "Komadai";

            Fu.transform.localPosition = new Vector3(0, 0.40f - 0.2f * i, 0);
            Fu.ClickAction = SelectKoma; //クリックされた時関数を呼ぶ
            // Debug.Log(Fu.transform.parent.gameObject.name);
        }

        //相手のコマ
        Koma koma = KomaGenerator.instance.Spawn(-2);
        // koma.tag = "Player";
        koma.ClickAction = SelectKoma; //クリックされた時関数を呼ぶ
        foreach (Masu obj in FindObjectsOfType<Masu>())
        {
            koma.transform.parent = obj.transform; 
            break;
        }
        koma.transform.localPosition = new Vector3(0, 0, 0);
        koma.transform.Rotate(0, 0, 180.0f);
    }

    //コマクリック時
    public void SelectKoma(Koma koma)
    {
        GameObject parent = koma.transform.parent.gameObject;

        resetMasuTag(koma);

        float x = (float)koma.transform.position.x;
        float y = (float)koma.transform.position.y;
        int index_x = (int)((3.0+x) / 1.5f);
        int index_y = (int)((3.0+y) / 1.5f);
        Debug.Log("iti:"+koma.transform.position);
        Debug.Log("コマ位置：" + "x:"+index_x +"y:"+ index_y + "コマ名前" + koma.name);
        App.slot = koma;

        //駒台からの移動
        if (parent.name == "Komadai") {
            //選択できるマスを表示する(0のステータスのマスを色を変化させる)
            createSelectObj();
        }
        //盤上からの移動
        else if(parent.name == "Masu") {
            SelectObj(koma);
        }

        MasuStatusLog();
    }

    //マスのステータス一覧
    public static void MasuStatusLog() {
        for(int i=0; i<App.MAX_Y;i++)
        {
            string[] intArray = Array.ConvertAll(App.masu_array[App.MAX_Y-i-1], s => {return s.ToString();});
            Debug.Log((App.MAX_Y-i).ToString()+"行目"+string.Join(",", intArray)); 
        }
    }

    //駒台から駒を
    public void createSelectObj() {
        // foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Masu"))
        foreach (Masu obj in FindObjectsOfType<Masu>())
        {
            if (obj.MasuStatus == 0) {
                obj.tag = "Select";
                obj.GetComponent<SpriteRenderer>().color = App.Select_Color;
            }
        }
    }

    //選択をキャンセルする
    public void resetMasuTag(Koma koma) {
        foreach (Masu obj in FindObjectsOfType<Masu>())
        {
            obj.tag = "Masu";
            obj.GetComponent<SpriteRenderer>().color = App.Masu_Color;
        }
    }

    //マスから駒をクリック時
    public void SelectObj(Koma koma) {
        var koma_p = koma.transform.position;
        // foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Masu"))
        if(koma.number == 2) {
            var select_p = new Vector3(koma_p.x, koma_p.y + 1.5f, koma_p.z);
            Debug.Log("これは歩です"+(koma_p.x,koma_p.y + 1.5f,koma_p.z));
            
            foreach (Masu obj in FindObjectsOfType<Masu>())
            {
                // 上方向に１つ上のマスのみ指定する
                if (obj.transform.position == select_p) {
                   obj.tag = "Select";
                   obj.GetComponent<SpriteRenderer>().color = App.Select_Color;
                }
            }
        }
    }
}
// //初期動作
// //4*4 で行う外周を99で配置する enam positionning,+自分で追加
// //initPositioning _ids = [0,1,2,2,2];
// //onclick

// if ($isdai || $isBan) {
//     if($this->App.slot && false); // 置く
        
//     if(!$this->App.slot && true);  //取る
//     //その場所に対してのデータ  $this->App.slotに入れる
// }
// //5つになったら活性化させる

//裏にしたまま
// //範囲指定
// $item->require_flg;
// //範囲外の場合処理を中断する
// //DBの値を
// if ($item->id === 2) {
//     for($i=1; $i<10; $i++) {
//         //現在の値から - $i +$iする
//         if(true){ break;} //範囲判定(99 or 1) //共通化？
//         //要素追加
//     }
//    // -1 +1を確認して範囲ないなら要素を生成
// }
//どの『item』がおこなっている処理？
//選択する処理は共通化
//⓵『選択肢』を選んでその制御によった選択肢を表示する(範囲 0,-1 選んだitem_idによって変わる)
//⓶以外の『選択肢』を選ぶ(範囲 0のみ全て)
//共通処理 -1の時だけ別処理
//終了判定
//次の判定


// //判定ルール
// item_idが判定時に存在しない
// item_idが-4の位置に存在する。
// item_id==1以外がなくなる
// 処理時間が

// Player
// Koma App.slot = null;
// Awake()
// List<int> komas = List<int>(); 台に
// int komas[5] = [1,2,2,3,4]; 固定
// for(i=0; i<5; i++;)



//         if (Input.GetMouseButtonDown(0)) {
//             Ray ray = camera_object.ScreenPointToRay(Input.mousePosition);
// //Physics2D.Raycast(原点、方向、長さ、指定レイヤー)
//             RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
 
//             if (hit2d) {
                
//                 clickedGameObject = hit2d.transform.gameObject;
//                 int x = (int)clickedGameObject.transform.position.x;
//                 int y = (int)clickedGameObject.transform.position.y;
//                 Debug.Log(clickedGameObject.name+"x" + x.ToString() + "y:" + y.ToString());

//                 // Debug.Log(clickedGameObject.name);
//                 if(clickedGameObject.name == "Koma(Clone)") {
//                     Debug.Log("成功");
//                 }
//             } 
//             Debug.Log(clickedGameObject);
//         }

            // Debug.Log(clickedGameObject);
            //  Debug.Log(clickedGameObject.transform.position);
            // clickedGameObject = null;
 
            // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
 
            // if (hit2d) {
            //     clickedGameObject = hit2d.transform.gameObject;
            // } 
 
            // Debug.Log(hit2d.transform.gameObject.position);