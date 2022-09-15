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
    private List<Dictionary<string, string>> KOMA_SET_ACTION = new List<Dictionary<string, string>>{};
    // private List<Dictionary<string, string>> INIT_KOMA_SET;
    bool isFirstPlayer;
    //バトル中の手順
    private List<Dictionary<string, string>> BATTLE_ACTION = new List<Dictionary<string, string>>{};

    // Start is called before the first frame update
    void Start()
    {
        isFirstPlayer = PhotonMaster.GM.GetPlayerType() == PLAYER_TYPE.FIRST;
        Debug.Log("GameMaster:Start()");
        //【セット状態以外】
        // if(App.game_type != GAME_TYPE.SET) { 
        //     Debug.Log("MoveList(App.SET_ACTION)");
        //     Debug.Log("App.SET_ACTION.Count:" + App.SET_ACTION.Count);
        MoveList(App.SET_ACTION); 
        // }
        
        foreach(var moveList in App.SET_ACTION) {
            Debug.Log("koma_num:" + new Koma().GetText(Int32.Parse(moveList["koma_num"])) + "before:" + moveList["before"] + "after:" + moveList["after"]);

            // MoveAction(test);
        }

        if(App.game_type == GAME_TYPE.BATTLE){
            DontDestroyOnLoad(gameObject);
        };       
    }

    public void SetMoveAction(Dictionary<string, string> moveAction)
    {
        this.KOMA_SET_ACTION.Add(moveAction);
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
        Dictionary<string, string>[] content = new Dictionary<string, string>[] {};
        // ictionary<string, string>[] content2 = content.Append();
        foreach(Dictionary<string, string> SET_ACTION in KOMA_SET_ACTION) {
                    Array.Resize(ref content, content.Length + 1);
                     content[content.Length - 1] = SET_ACTION;
        }

        PhotonNetwork.RaiseEvent(1, content, raiseEventOptions, SendOptions.SendReliable);
        AllSetPlayerCount();
    }

    public void AllSetPlayerCount()
    {
        object[] content = new object[] { PhotonMaster.GM.GetPlayerType() == PLAYER_TYPE.FIRST ? 0 : 1 };

        PhotonNetwork.RaiseEvent(2, content, raiseEventOptions, SendOptions.SendReliable);
    }

    //【バトル中】
    public void ALLMoveAction(Dictionary<string, string> moveAction)
    {
        Debug.Log("ALLMoveAction");
        Dictionary<string, string>[] content = new Dictionary<string, string>[] { moveAction };

        PhotonNetwork.RaiseEvent(4, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void OnEvent(EventData photonEvent)
    {   
        Debug.Log("photonEvent.CustomData");
        Debug.Log(photonEvent.CustomData);

        // Dictionary<string, string> moveAction = (Dictionary<string, string>)photonEvent.CustomData;

        byte eventCode = photonEvent.Code;
        Debug.Log("eventCode：" + eventCode);
        switch( eventCode )
        {
            case 1:
                //【セット中】自分のセットした配置を変数に追加する
                Dictionary<string, string>[] datas1 = (Dictionary<string, string>[])photonEvent.CustomData;
                foreach(Dictionary<string, string> data in datas1) {
                    App.SET_ACTION.Add(data);
                }
                break;
            case 2:
                //【セット中】「完了ボタン」のカウントと全員押した時の動作
                Debug.Log("AllSetPlayerCount");
                object[] data2 = (object[])photonEvent.CustomData;
                player_cnt[(int)data2[0]] = 1;
                setPlayerCount(); //isFirstPlayer
                break;
            case 3:
                setPlayerCount(); //isFirstPlayer
                break;
            case 4:
                //【バトル中】1回動かす
                Dictionary<string, string>[] data4 = (Dictionary<string, string>[])photonEvent.CustomData;
                MoveAction(data4[0]);
                BATTLE_ACTION.Add(data4[0]);
                break;
            default:
                break;
        }
    }

    public void setPlayerCount()
    {   
        bool isDone = player_cnt.All( value => value > 0 );
        // ２人とも終わったら
        if(isDone) { 
            App.game_type = GAME_TYPE.BATTLE;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            // Debug.Log("配置モード　→　バトルモード");
        }
        Debug.Log("player_cnt:" + ":1番目⇨" +player_cnt[0] + ":2番目⇨" + player_cnt[1]);
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
        Debug.Log("MoveAction開始" + new Koma().GetText(Int32.Parse(moveAction["koma_num"])) + "koma_num:" + moveAction["koma_num"] + "before:" + moveAction["before"] + "after:" + moveAction["after"]);

        // 【共通】盤上,駒台
        string masu_after = "Masu" + moveAction["after"];
        Masu masu = GameObject.Find(masu_after).GetComponent<Masu>();
        Debug.Log("マス:" + masu.ToString());
        GameObject komadai_obj = GameObject.Find(Int32.Parse(moveAction["koma_num"]) > 0 ? App.KOMADAI1_NAME : App.KOMADAI2_NAME);
        //　駒台から
        if(moveAction["before"].Contains("Komadai")) {
            Koma masu_koma = App.GetChildKoma(obj: komadai_obj, koma_name: moveAction["koma_num"]);

            //駒台 => 盤上
            masu_koma.transform.parent = masu.transform; //マスを親にする。

            //使ったら駒台の位置を並び替える。
            var koma_children = komadai_obj.GetComponentsInChildren<Koma>();
            int koma_i = 0;
            foreach(Koma koma_child in koma_children) {
                koma_child.transform.localPosition = new Vector3(KomadaiVectorX(koma_i), 0, 0);
                koma_i++;
            }

            masu_koma.transform.localPosition = new Vector3(0, 0, 0);
        // 盤上
        } else {
            var masu_before = "Masu" + moveAction["before"];

            //【移動前】
            GameObject masu_before_obj = GameObject.Find(masu_before);
            Koma before_koma = App.GetChildKoma(obj: masu_before_obj, koma_name: moveAction["koma_num"]);
            //【移動後】
            GameObject masu_after_obj = GameObject.Find(masu_after);
            Koma after_koma = App.GetChildKoma(obj: masu_after_obj, koma_name: null);

            // 【駒を取得】
            if(after_koma != null) {
                after_koma.tag = "Komadai";
                after_koma.number *= -1;//ステータスを自分のコマに
                //TODO:成り駒を元に戻す。
                
                after_koma.transform.parent = komadai_obj.transform;
                after_koma.transform.Rotate(0f, 0f, 180f);
                after_koma.transform.localPosition = new Vector3(0, 0, 0);
                after_koma.transform.localScale = new Vector3(App.MASU_SIZE, App.MASU_SIZE, App.MASU_SIZE);
                after_koma.ClickAction = Player.instance.SelectKoma;
                
                var koma_children = komadai_obj.GetComponentsInChildren<Koma>();
                int i = 0;
                foreach(Koma koma_child in koma_children) {
                    koma_child.transform.localPosition = new Vector3(KomadaiVectorX(i), 0, 0);
                    i++;
                }
            }
            before_koma.number = Int32.Parse(moveAction["koma_num"]);
            KomaGenerator.instance.TextChange(before_koma);//　成る時の名前
            Masu after_masu = masu_after_obj.GetComponent<Masu>();
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

// 投了ボタン===========================
    // スタート時に呼ばれる
    // void Start () {

    //     // myProcess.ExeMyProcess();
    // }
}
