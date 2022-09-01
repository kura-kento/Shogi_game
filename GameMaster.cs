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
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour, IOnEventCallback
{
    static int[] player_cnt = new int[2]{0, 0}; //コマのセットが完了した人数
    // isAllDone = SET_PLAYER_CNT.All( value => value > 0 );
    private PLAYER_TYPE player_type; // 先手？後手？
    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
    // int[] player_1 = new int[] { 1, 2, 3, 4, 5 };
    // int[] player_2 = new int[] { -6, -4, -8, -7, -1 };
    //駒セット時の手順
    private List<Dictionary<string, string>> KOMA_SET_ACTION = new List<Dictionary<string, string>>{
        // new Dictionary<string, string> {{"koma_num","1"}, {"before",null} ,{"after","5_1"},},
        // new Dictionary<string, string> {{"koma_num","2"}, {"before",null} ,{"after","4_2"},},
        // new Dictionary<string, string> {{"koma_num","3"}, {"before",App.KOMADAI1_NAME} ,{"after","4_3"},},
        // new Dictionary<string, string> {{"koma_num","4"}, {"before",App.KOMADAI1_NAME} ,{"after","4_4"},},
        // new Dictionary<string, string> {{"koma_num","5"}, {"before",App.KOMADAI1_NAME} ,{"after","5_5"},},

        // new Dictionary<string, string> {{"koma_num","-6"}, {"before",App.KOMADAI2_NAME} ,{"after","1_2"},},
        // new Dictionary<string, string> {{"koma_num","-4"}, {"before",App.KOMADAI2_NAME} ,{"after","1_3"},},
        // new Dictionary<string, string> {{"koma_num","-8"}, {"before",App.KOMADAI2_NAME} ,{"after","1_4"},},
        // new Dictionary<string, string> {{"koma_num","-7"}, {"before",App.KOMADAI2_NAME} ,{"after","2_2"},},
        // new Dictionary<string, string> {{"koma_num","-1"}, {"before",App.KOMADAI2_NAME} ,{"after","3_2"},},
    };
    // private List<Dictionary<string, string>> INIT_KOMA_SET;

    //バトル中の手順
    private List<Dictionary<string, string>> BATTLE_ACTION = new List<Dictionary<string, string>>{};

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameMaster:Start()");
        //【セット状態以外】
        if(App.game_type != GAME_TYPE.SET) { 
            Debug.Log("MoveList(App.SET_ACTION)");
            Debug.Log("App.SET_ACTION.Count:" + App.SET_ACTION.Count);
            MoveList(App.SET_ACTION); 
        }
        
        foreach(var moveList in App.SET_ACTION) {
            Debug.Log("koma_num:" + moveList["koma_num"] + "before:" + moveList["before"] + "after:" + moveList["after"]);

            // MoveAction(test);
        }
       
    }

    public void SetMoveAction(Dictionary<string, string> moveAction)
    {
        Debug.Log("SetMoveAction:"+this.KOMA_SET_ACTION.Count);
        this.KOMA_SET_ACTION.Add(moveAction);
         Debug.Log("SetMoveAction:"+this.KOMA_SET_ACTION.Count);
        // MoveAction(BATTLE_ACTION.Last());
        //　初期配置に戻して  //　MoveList(BATTLE_ACTION); // シーン自体を初期化にして
    }

    public List<Dictionary<string, string>> GetSetKoma()
    {
        Debug.Log(this.KOMA_SET_ACTION.Count);
        return this.KOMA_SET_ACTION;
    }

    
//=======全体=========================================================================================================================================================================
    public void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void AllSetAction()
    {
        Debug.Log("AllEvent");
        PhotonNetwork.RaiseEvent(1, null, raiseEventOptions, SendOptions.SendReliable);
    }

    public void AllEvent2()
    {
        PhotonNetwork.RaiseEvent(2, null, raiseEventOptions, SendOptions.SendReliable);
    }

    //セットした人数を追加する
    public void AddDonePlayer()
    {
        Debug.Log("AddDonePlayer");
        PhotonNetwork.RaiseEvent(3, null,raiseEventOptions, SendOptions.SendReliable);
    }

    public void OnEvent(EventData photonEvent)
    {   
        // Debug.Log(photonEvent.CustomData);
        Dictionary<string, string> moveAction = (Dictionary<string, string>)photonEvent.CustomData;

        byte eventCode = photonEvent.Code;
        Debug.Log("eventCode：" + eventCode);
        switch( eventCode )
        {
            case 1:
                Debug.Log("App.SET_ACTION.AddRange(KOMA_SET_ACTION);");
                App.SET_ACTION.AddRange(this.KOMA_SET_ACTION);
                Debug.Log("ase 0:：" + App.SET_ACTION.Count);
                break;
            case 2:
                Debug.Log("AllEvent2");
                Debug.Log("ase 1:：" + App.SET_ACTION.Count);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                Debug.Log("ase 2:：" + App.SET_ACTION.Count);
                break;
            case 3:
                setPlayerCount(); //isFirstPlayer
                break;
            default:
                break;
        }
    }

    public void setPlayerCount()
    {   
        // AllEvent2();
        // this.BATTLE_ACTION.AddRange(KOMA_SET_ACTION);
        // Debug.Log(this.BATTLE_ACTION.ToString());
        // player_cnt[PhotonMaster.GM.GetPlayerType() == PLAYER_TYPE.FIRST ? 0 : 1] = 1;
        // bool isDone = player_cnt.All( value => value > 0 );
        // // ２人とも終わったら
        // if(isDone) { 
        //     DontDestroyOnLoad(gameObject);
        //     App.game_type = GAME_TYPE.BATTLE;
        //     Debug.Log("配置モード　→　バトルモード");
        // }
        // Debug.Log("player_cnt:" + ":1番目⇨" +player_cnt[0] + ":2番目⇨" + player_cnt[1]);
    }

//=======全体END===========================================================================================================================================
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
    // public static void MasuStatusLog() {
    //     for(int i=0; i<App.MAX_Y;i++)
    //     {
    //         string[] intArray = Array.ConvertAll(App.masu_array[App.MAX_Y-i-1], s => {return s.ToString();});
    //     }
    // }
 
    //全てのマスの選択をキャンセルする
    public static void ResetMasu() {
        foreach (Masu masu in FindObjectsOfType<Masu>())
        {
            if(Masu.isNoUseMasu(masu)) continue;

            masu.tag = "Masu";
            masu.GetComponent<SpriteRenderer>().color = App.MASU_COLOR;
            masu.GetComponent<Renderer>().sortingLayerName = "back++";
        }
    }
    //マスを選択させる
    public static void SelectMasu(Masu masu) {
        masu.tag = "Select";
        masu.GetComponent<SpriteRenderer>().color = App.SELECT_COLOR;
        masu.GetComponent<Renderer>().sortingLayerName = "front++"; 
    }

    //手順　引数手順:[41金,42金] {koma_num: 6, before:[4,1] , after[4,2]} 42打金 {koma_num: 6, before: null, after:[4,2]} 
    public void MoveList(List<Dictionary<string, string>> list) {     
         
        //初期配置からローカルで動かして盤面を配置する。
        var i=1;
        foreach(var moveAction in list) {
            Debug.Log("MoveList:" + i + "回目");  
            MoveAction(moveAction);
            i++;
        }
    }

    public void MoveAction(Dictionary<string, string> moveAction) {
        // if(App.game_type == GAME_TYPE.KOMA_SET){
        //    KOMA_SET_ACTION.Add(moveAction);
        // }
        Debug.Log("koma_num:" + moveAction["koma_num"] + "before:" + moveAction["before"] + "after:" + moveAction["after"]);

        // 【共通】盤上,駒台
        string masu_after = "Masu" + moveAction["after"];
        Masu masu = GameObject.Find(masu_after).GetComponent<Masu>();
            Debug.Log("マス:" + masu.ToString());
        //　駒台から
        if(moveAction["before"].Contains("Komadai")) {
            GameObject komadai_obj = GameObject.Find(Int32.Parse(moveAction["koma_num"]) > 0 ? App.KOMADAI1_NAME : App.KOMADAI2_NAME);
            Koma masu_koma = App.GetChildKoma(obj: komadai_obj, koma_name: moveAction["koma_num"]);

            //駒台 => 盤上
            masu_koma.transform.parent = masu.transform; //マスを親にする。


            // //使ったら駒台の位置を並び替える。
            var koma_children = komadai_obj.GetComponentsInChildren<Koma>();
            int i = 0;
            foreach(Koma koma_child in koma_children) {
                koma_child.transform.localPosition = new Vector3(KomadaiVectorX(i), 0, 0);
                i++;
            }


            masu_koma.transform.localPosition = new Vector3(0, 0, 0);
        // 盤上
        } else {
            var masu_before = "Masu" + moveAction["before"];

            GameObject masu_before_obj = GameObject.Find(masu_before);
            Koma before_koma = App.GetChildKoma(obj: masu_before_obj, koma_name: moveAction["koma_num"]);

            GameObject masu_after_obj = GameObject.Find(masu_after);
            Masu after_masu = masu_after_obj.GetComponent<Masu>();
            // Koma after_koma = App.GetChildKoma(obj: masu_after_obj, koma_name: moveAction["koma_num"]);

            before_koma.transform.parent = after_masu.transform; //マスを親にする。
            before_koma.transform.localPosition = new Vector3(0, 0, 0);
        }

        //マスの状態をリセットする
        ResetMasu();
        //「BATTLE」の時
        if(App.game_type == GAME_TYPE.BATTLE) {
            // TODO:ここに勝利判定を入れる
            //駒を動かす
            TurnEnd();//ダーン終了の処理    
        }
    }
    //駒台の座標
    public float KomadaiVectorX(int i) {
        return App.isTurePlayer1 ? (App.MASU_SIZE * 2.0f) - (App.MASU_SIZE * i) : -(App.MASU_SIZE * 2.0f) + (App.MASU_SIZE * i) * i;
    }

    //ダーン終了の処理
    public void TurnEnd() {
        App.isTurePlayer1 = !(App.isTurePlayer1);
        App.Turn++; //ターン数増やす
        App.turnUp();
    }
}
