using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Linq;

public class GameMaster : MonoBehaviour
{
    private PLAYER_TYPE player_type; // 先手？後手？
    int?[,] MASU_DATA = new int?[6, 6] {
        {null,   0,   0,   0,   0,null},
        {null,   0,null,null,   0,null},
        {null,   0,   0,   0,   0,null},
        {   0,   0,   0,   0,   0,   0},
        {   0,   0,null,null,   0,   0},
        {null,null,null,null,null,null}
    };
    List<List<int?>> MASU_DATA2 = new List<List<int?>> {
       new List<int?> {null,   0,   0,   0,   0,null},
       new List<int?> {null,   0,null,null,   0,null},
       new List<int?> {null,   0,   0,   0,   0,null},
       
       new List<int?> {   0,   0,   0,   0,   0,   0},
       new List<int?> {   0,   0,null,null,   0,   0},
       new List<int?> {null,null,null,null,null,null}
    };
    // int[] player_1 = new int[] { 1, 2, 3, 4, 5 };
    //駒セット時の手順
    private List<Dictionary<string, string?>> KOMA_SET_ACTION = new List<Dictionary<string, string?>>{
        new Dictionary<string, string?> {{"koma_num","1"}, {"before",null} ,{"after","5_1"},},
        new Dictionary<string, string?> {{"koma_num","2"}, {"before",null} ,{"after","4_2"},},
        new Dictionary<string, string?> {{"koma_num","3"}, {"before",null} ,{"after","4_3"},},
        new Dictionary<string, string?> {{"koma_num","4"}, {"before",null} ,{"after","4_4"},},
        new Dictionary<string, string?> {{"koma_num","5"}, {"before",null} ,{"after","5_5"},},
    };

    //バトル中の手順
    private List<Dictionary<string, string>> BATTLE_ACTION = new List<Dictionary<string, string>>{
        new Dictionary<string, string> {{"koma_num","6"}, {"before","4_1"} ,{"after","4_2"},},
    };


    // Start is called before the first frame update
    void Start()
    {
        Dictionary<string, string> i = new Dictionary<string, string>{{"koma_num","8"}, {"before","5_1"} ,{"after","5_2"},};
        BATTLE_ACTION.Add(i);
        MoveList(KOMA_SET_ACTION);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //先手後手を入れる。（一度しか呼ばれない)
    public void SetPlayerType(PLAYER_TYPE type)
    {
        // if(this.player_type != PLAYER_TYPE.NULL) { throw new System.Exception("先手後手判定エラー:既に設定されている"+this.player_type.ToString());}
        // Debug.Log("ああああ");
        this.player_type = type;
    }

    public PLAYER_TYPE GetPlayerType()
    {
        return this.player_type;
    }

    //マスのステータス(ログ)
    public static void MasuStatusLog() {
        for(int i=0; i<App.MAX_Y;i++)
        {
            string[] intArray = Array.ConvertAll(App.masu_array[App.MAX_Y-i-1], s => {return s.ToString();});
            // Debug.Log((App.MAX_Y-i).ToString()+"行目"+string.Join(",", intArray)); 
        }
    }

    //全てのマスの選択をキャンセルする
    public static void ResetMasu() {
        foreach (Masu masu in FindObjectsOfType<Masu>())
        {
            if(Masu.isNoUseMasu(masu)) continue;

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

    //手順　引数手順:[41金,42金] {koma_num: 6, before:[4,1] , after[4,2]} 42打金 {koma_num: 6, before: null, after:[4,2]} 
    public void MoveList(List<Dictionary<string, string>> list) {        
        //初期配置からローカルで動かして盤面を配置する。
        foreach(var moveList in list) {
            MoveAction(moveList);
        }
    }

    public void MoveAction(Dictionary<string, string> moveList) {
        // Debug.Log(moveList);
        // masu1_init["before"].Split('_').ToList()
        Debug.Log("koma_num:" + moveList["koma_num"] + "before:" + moveList["before"] + "after:" + moveList["after"]);
        // moveList["koma_num"];// moveList["before"];// moveList["after"];

        // 【共通】盤上,駒台
        var masu_name = "Masu" + moveList["after"];
        Masu masu = GameObject.Find(masu_name).GetComponent<Masu>();
            Debug.Log("マス:" + masu.ToString());
        //　駒台から
        if(moveList["before"] == null) {
            
            GameObject gameObject2 = GameObject.Find("Komadai1");
            // var koma_list = gameObject2.GetComponentsInChildren<Koma>();
            // foreach(Koma koma in koma_list) {
            //     Debug.Log("駒:" + koma.ToString());
            // }
            Koma masu_koma = App.KomadaiGetChildKoma(gameObject2, koma_name: moveList["koma_num"]);

            if(masu_koma == null) {
                Debug.Log("駒: NULL");
            }else{
                Debug.Log("駒:" + masu_koma.number.ToString());
            }
            //駒台 => 盤上
            masu_koma.transform.parent = masu.transform; //マスを親にする。
            // //使ったら駒台の位置を並び替える。
            // var koma_children = komadai_obj.GetComponentsInChildren<Koma>();
            // int i = 0;
            // foreach(Koma koma_child in koma_children) {
            //     koma_child.transform.localPosition = new Vector3(KomadaiVectorX(i), 0, 0);
            //     i++;
            // }

            //マスの状態をリセットする
            // GameMaster.ResetMasu();
            masu_koma.transform.localPosition = new Vector3(0, 0, 0);
            
        }

        // Debug.Log(masu_koma);
        //盤上
        // if(moveList["before"] != null) {
        //     // 【駒を成る】この処理はなしkoma_numで成り不成がわかる
        //     // moveMap["after"]

        //     //駒有り　かつ　相手の駒の時
        //     // if(masu_koma != null &&  App.isEnemyKoma(masu_koma)) {}
        //         //TODO:【玉が取られた時】
        //         // if(Mathf.Abs(masu_koma.number) == 1){ Win();}

        //         //TODO: そこに駒が有れば取る
        //         // moveMap["before"];41のマス情報を取得 4_1_Masuで配置
        //         // 子の要素があるか？
        //         //【駒を取った時の処理】

        //         masu_koma.tag = "Komadai";
        //         masu_koma.number *= -1;//ステータスを自分のコマに

        //         Koma new_koma = App.isTurePlayer1 ?
        //         Player.instance.CreateKoma(masu_koma.number):
        //         Player2.instance.CreateKoma(masu_koma.number);

        //         Destroy(masu_koma.transform.gameObject);
        //         new_koma.transform.parent = komadai_obj.transform;

        //         //【全駒】判定
        //         if(isEnemyZero() == true){ Win();}
        // }
        // //共通処理
        // App.slot.transform.parent = masu.transform; //マスを親にする。
        // //使ったら駒台の位置を並び替える。
        // var koma_children = komadai_obj.GetComponentsInChildren<Koma>();
        // int i = 0;
        // foreach(Koma koma_child in koma_children) {
        //     koma_child.transform.localPosition = new Vector3(KomadaiVectorX(i), 0, 0);
        //     i++;
        // }
        // //マスの状態をリセットする
        // GameMaster.ResetMasu();
        // App.slot.transform.localPosition = new Vector3(0, 0, 0);
        // App.slot = null;

        // //「BATTLE」の時
        // if(App.game_type == GAME_TYPE.BATTLE) {
        //     // TODO:ここに勝利判定を入れる
        //     TurnEnd();//ダーン終了の処理    
        // }
    }
    

    //駒台の順番 
    // public static void KomadaiSetPosition(Masu masu) {
    //     masu.tag = "Select";
    //     masu.GetComponent<SpriteRenderer>().color = App.Select_Color;
    //     masu.GetComponent<Renderer>().sortingLayerName = "front++"; 
    // }
}
