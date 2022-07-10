using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] Komadai komadaiPrefab;

    //カメラ情報
    private Camera camera_object;
    private RaycastHit hit;
    // List<int> komas = new List<int>();
    int[] player_1 = new int[] { 1, 2, 2, 3, 4 };
    string[] test = new string[] {"1","2","3"};

    // 決定ぼたんを押したら,Game Masterに通知する

    private void Start()
    {
        //カメラ情報を取得
        camera_object = GameObject.Find("Main Camera").GetComponent<Camera>();

        SetupKomadai();
 
        MasuStatusLog();
    }

    void Update()
    {
       if (Input.GetMouseButtonDown(0)) {

       }
    }

    public void SetupKomadai()
    {
        Instantiate(komadaiPrefab); //駒台
    
        Koma Ou = KomaGenerator.instance.Spawn(2);
        Debug.Log("王が移動する");
        Ou.transform.localPosition = new Vector3(1.5f, 1.5f, 0);
        Ou.ClickAction = SelectKoma; //クリックされた時関数を呼ぶ
    }

    //コマクリック時
    public void SelectKoma(Koma koma)
    {
        float x = (float)koma.transform.position.x;
        float y = (float)koma.transform.position.y;

        Debug.Log("コマ位置：" + koma.transform.position + "コマ名前" + koma.name);
        if(App.slot == null) {
            App.slot = koma;
            Debug.Log("スロット空 =>" + App.slot.name);
        } else {
            App.slot = null;

            koma.transform.localPosition = new Vector3(0f, 0f, 0);
            Debug.Log("スロット => 空");
        }
        
        MasuStatusLog();
    }

    //マスのステータス一覧
    public void MasuStatusLog() {
        int i = 1;
        foreach(var masu in App.masu_array)
        { 
            string[] intArray = Array.ConvertAll(masu, s => {return s.ToString();});
            Debug.Log(i+"行目"+string.Join(",", intArray)); 
            i++;
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