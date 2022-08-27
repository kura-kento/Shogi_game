using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Linq;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

public class GameMaster : MonoBehaviour, IOnEventCallback
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
    // int[] player_2 = new int[] { -6, -4, -8, -7, -1 };
    //駒セット時の手順
    private List<Dictionary<string, string?>> KOMA_SET_ACTION = new List<Dictionary<string, string?>>{
        // new Dictionary<string, string?> {{"koma_num","1"}, {"before",null} ,{"after","5_1"},},
        // new Dictionary<string, string?> {{"koma_num","2"}, {"before",null} ,{"after","4_2"},},
        new Dictionary<string, string?> {{"koma_num","3"}, {"before",null} ,{"after","4_3"},},
        new Dictionary<string, string?> {{"koma_num","4"}, {"before",null} ,{"after","4_4"},},
        new Dictionary<string, string?> {{"koma_num","5"}, {"before",null} ,{"after","5_5"},},

        new Dictionary<string, string?> {{"koma_num","-6"}, {"before",null} ,{"after","1_2"},},
        new Dictionary<string, string?> {{"koma_num","-4"}, {"before",null} ,{"after","1_3"},},
        new Dictionary<string, string?> {{"koma_num","-8"}, {"before",null} ,{"after","1_4"},},
        new Dictionary<string, string?> {{"koma_num","-7"}, {"before",null} ,{"after","2_2"},},
        new Dictionary<string, string?> {{"koma_num","-1"}, {"before",null} ,{"after","3_2"},},

    };

    //バトル中の手順
    private List<Dictionary<string, string>> BATTLE_ACTION = new List<Dictionary<string, string>>{
        // new Dictionary<string, string> {{"koma_num","5"}, {"before","4_3"} ,{"after","4_2"},},
    };

    // Start is called before the first frame update
    void Start()
    {
        // Dictionary<string, string> i = new Dictionary<string, string>{{"koma_num","8"}, {"before","5_1"} ,{"after","5_2"},};
        // BATTLE_ACTION.Add(i);
        MoveList(KOMA_SET_ACTION);
        // MoveList(BATTLE_ACTION);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMoveAction(Dictionary<string, string> moveList)
    {
        BATTLE_ACTION.Add(moveList);
        MoveAction(KOMA_SET_ACTION.Last());
        //　初期配置に戻して
        //　MoveList(BATTLE_ACTION);
        // シーン自体を初期化にして
    }
//=======全体=============
    public void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void AllEvent(Dictionary<string, string> MoveAction)
    {
        Debug.Log("AllEvent");

        object[] content = new object[] { MoveAction }; // Array contains the target position and the IDs of the selected units
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(1, MoveAction, raiseEventOptions, SendOptions.SendReliable);
    }

    public void OnEvent(EventData photonEvent)
    {   
        // object[] data = (object[])photonEvent.CustomData;
        // Dictionary<string, string> MoveAction = (Dictionary<string, string>)data[0];

        byte eventCode = photonEvent.Code;
        switch( eventCode )
        {
            case 1:
                Dictionary<string, string> MoveAction2 = new Dictionary<string, string>{{"koma_num", "5"}, {"before", "5_5"} ,{"after", "4_5"},};

                // SetMoveAction(MoveAction2);
                // SetMoveAction(MoveAction);
                MoveAction(MoveAction2);
                // MoveAction(KOMA_SET_ACTION.Last());
                break;
            default:
                break;
        }
    }

//=======全体END=============
    public void GetMoveAction()
    {
        foreach(var moveList in BATTLE_ACTION) {
            Debug.Log("koma_num:" + moveList["koma_num"] + "before:" + moveList["before"] + "after:" + moveList["after"]);
        }

    }

    //先手後手を入れる。（一度しか呼ばれない)
    public void SetPlayerType(PLAYER_TYPE type)
    {
        // if(this.player_type != PLAYER_TYPE.NULL) { throw new System.Exception("先手後手判定エラー:既に設定されている"+this.player_type.ToString());}
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
        string masu_after = "Masu" + moveList["after"];
        Masu masu = GameObject.Find(masu_after).GetComponent<Masu>();
            Debug.Log("マス:" + masu.ToString());
        //　駒台から
        if(moveList["before"] == null) {
            
            GameObject komadai_obj = GameObject.Find(Int32.Parse(moveList["koma_num"]) > 0 ? App.KOMADAI1_NAME : App.KOMADAI2_NAME);
            Koma masu_koma = App.GetChildKoma(obj: komadai_obj, koma_name: moveList["koma_num"]);

            // if(masu_koma == null) {
            //     Debug.Log("駒: NULL");
            // }else{
            //     Debug.Log("駒:" + masu_koma.number.ToString());
            // }
            //駒台 => 盤上
            masu_koma.transform.parent = masu.transform; //マスを親にする。


            // //使ったら駒台の位置を並び替える。
            var koma_children = komadai_obj.GetComponentsInChildren<Koma>();
            int i = 0;
            foreach(Koma koma_child in koma_children) {
                koma_child.transform.localPosition = new Vector3(KomadaiVectorX(i), 0, 0);
                i++;
            }

            //マスの状態をリセットする
            GameMaster.ResetMasu();
            masu_koma.transform.localPosition = new Vector3(0, 0, 0);
        // 盤上
        } else {
            var masu_before = "Masu" + moveList["before"];

            GameObject masu_before_obj = GameObject.Find(masu_before);
            Koma before_koma = App.GetChildKoma(obj: masu_before_obj, koma_name: moveList["koma_num"]);

            GameObject masu_after_obj = GameObject.Find(masu_after);
            Masu after_masu = masu_after_obj.GetComponent<Masu>();
            // Koma after_koma = App.GetChildKoma(obj: masu_after_obj, koma_name: moveList["koma_num"]);

            before_koma.transform.parent = after_masu.transform; //マスを親にする。
            before_koma.transform.localPosition = new Vector3(0, 0, 0);
        }
    }
    //駒台の座標
    public float KomadaiVectorX(int i) {
        return App.isTurePlayer1 ? (App.MASU_SIZE * 2.0f) - (App.MASU_SIZE * i) : -(App.MASU_SIZE * 2.0f) + (App.MASU_SIZE * i) * i;
    }
}
