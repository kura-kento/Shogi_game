using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Linq;
using Photon.Pun;
using UnityEngine.EventSystems;
using TMPro;
//先攻プレーヤー
public class Player : PlayerController
{
    private GameObject parent;
    public static Player instance;
    string KOMADAI;
    bool isFirstPlayer;
    int[] player_1 = new int[] { 1, 2, 3, 4, 5 };
    int[] player_2 = new int[] { -6, -4, -8, -7, -1 };
    

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // 先手後手の情報取得
        Debug.Log(PhotonMaster.GM.GetPlayerType());
        Debug.Log("PhotonNetwork.LocalPlayer:" + PhotonNetwork.LocalPlayer.ToString());
        Debug.Log(PhotonNetwork.LocalPlayer.ToString().Contains("#01"));

        //　ルーム作成者が先手　かつ　ルーム作成者の場合 または　ルーム作成者が後手　かつ　ルーム参加者の場合 
        isFirstPlayer = PhotonMaster.GM.GetPlayerType() == PLAYER_TYPE.FIRST;
        KOMADAI = isFirstPlayer ? App.KOMADAI1_NAME : App.KOMADAI2_NAME;

        //後手のカメラ180度回転
        // if (!isFirstPlayer) {
        //     GameObject camera = GameObject.Find("Main Camera");
        //     camera.transform.Rotate(0, 0, 180.0f);
        // }
        //
        // photonView.RPC(nameof(SetupPhoton), RpcTarget.All);
        // if (photonView.IsMine) {
        //     photonView.RPC(nameof(SetupPhoton), RpcTarget.All);
        // }
        // SetupPhoton();
        SetupKomadai();
        // GameMaster.MasuStatusLog();
    }

    // [PunRPC]
    // private void RpcSendMessage(string message) {
    //     Debug.Log(message);
    // }

    // [PunRPC]
    public void SetupPhoton() {
        for(int i=0;i<5;i++) {
            var obj = PhotonNetwork.Instantiate("Koma", new Vector3(0, 0, 0), Quaternion.identity, 0);
            obj.AddComponent<Koma>();
            Koma koma = obj.transform.gameObject.GetComponent<Koma>();
            koma.number = isFirstPlayer ? player_1[i] : player_2[i];
            Transform children = obj.GetComponentInChildren<Transform>();
            koma.text = obj.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();;
            KomaGenerator.instance.TextChange(koma);
            EventTrigger trigger = obj.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) => { 
                    SelectKoma(koma);
                });
            trigger.triggers.Add(entry);

            koma.transform.parent = GameObject.Find(KOMADAI).transform;
            koma.tag = "Komadai";

            koma.transform.localPosition = new Vector3((App.MASU_SIZE * 2.0f) - (App.MASU_SIZE * i), 0, 0);
            koma.transform.localScale = new Vector3(App.MASU_SIZE, App.MASU_SIZE, App.MASU_SIZE);
            if(!isFirstPlayer) { koma.transform.Rotate(0, 0, 180.0f); };
        }
    }

//     void Update()
//     {
//         if (Input.GetMouseButtonDown(0)) {
//             Debug.Log("クリックしたよ");
//             photonView.RPC(nameof(RpcSendMessage), RpcTarget.All, "こんにちは");
//         }
//     }

//    [PunRPC]
//     private void RpcSendMessage(string message) {
//         Debug.Log(message);
//     }

    //初期配置
    public void SetupKomadai()
    {
        // GameObject komadai = GameObject.Find("Komadai1");
        //自身の駒
        for(int i=0;i<5;i++) {
            Koma Koma = KomaGenerator.instance.Spawn(player_1[i]);
            Koma.transform.parent = GameObject.Find(App.KOMADAI1_NAME).transform;
            Koma.tag = "Komadai";
            Koma.transform.localPosition = new Vector3((App.MASU_SIZE * 2.0f) - (App.MASU_SIZE * i), 0, 0);
            Koma.transform.localScale = new Vector3(App.MASU_SIZE, App.MASU_SIZE, App.MASU_SIZE);
            Koma.ClickAction = isFirstPlayer ? SelectKoma : NotSelectKoma; //クリックされた時関数を呼ぶ
        }

        //相手の駒
        for(int i=0;i<5;i++) {
            Koma Koma = KomaGenerator.instance.Spawn(player_2[i]);
            Koma.transform.parent = GameObject.Find(App.KOMADAI2_NAME).transform;
            Koma.tag = "Komadai";
            Koma.transform.localPosition = new Vector3(-(App.MASU_SIZE * 2.0f) + (App.MASU_SIZE * i), 0, 0);
            Koma.transform.localScale = new Vector3(App.MASU_SIZE, App.MASU_SIZE, App.MASU_SIZE);
            Koma.ClickAction = !isFirstPlayer ? SelectKoma : NotSelectKoma; //クリックされた時関数を呼ぶ
            Koma.transform.Rotate(0, 0, 180.0f);
        }
    }

    public void NotSelectKoma(Koma koma){}

    //コマクリック時(自分が生成した)
    public void SelectKoma(Koma koma)
    {   
        GameMaster.ResetMasu(); //リセット
        if(App.game_type == GAME_TYPE.WAIT) { return ;}
        //同じ駒を押した時、キャンセルする。
        if(App.slot == koma) {
            App.slot = null;
            return;
        }

        GameObject parent = koma.transform.parent.gameObject;//駒の親要素を取得
        App.slot = koma;
        
        //「KOMA_SET」の時
        if(App.game_type == GAME_TYPE.KOMA_SET) {
            //全ての
            mySelectObj(isFirstPlayer: isFirstPlayer);
            return;
        }
        //駒台からの移動
        if (parent.name == KOMADAI) {
            Debug.Log("駒台からの移動");
            //選択できるマスを表示する(0のステータスのマスを色を変化させる)
            CreateSelectObj(isFirstPlayer: isFirstPlayer);
        }
        //盤上からの移動
        else if(parent.name.Contains("Masu")) {
            Debug.Log("盤上からの移動");
            SelectObj(koma, isFirstPlayer: isFirstPlayer);
        }else {
            Debug.Log("エラー");
        }
    }

    public Koma CreateKoma(int status) {
        Koma k = KomaGenerator.instance.Spawn(status);
        k.transform.parent = GameObject.Find(KOMADAI).transform;
        k.tag = "Komadai";
        k.ClickAction = SelectKoma; //クリックされた時関数を呼ぶ
        return k;
    }
}