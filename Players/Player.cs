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
    
    [SerializeField] public Camera Camera2;
    [SerializeField] public GameObject Canvas2;
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

        SetupKomadai(); //【駒台生成】

        // 【駒台を180度回転】
        if(!isFirstPlayer) {
            GameObject.Find("Shogiban").transform.Rotate(0f, 0f, 180f); 
            GameObject komadai1 = GameObject.Find(App.KOMADAI1_NAME);
            komadai1.transform.Rotate(0f, 0f, 180f);
            komadai1.transform.localPosition = App.ReverseVector(komadai1.transform.localPosition);
            GameObject.Find(App.KOMADAI1_NAME).transform.Rotate(0f, 0f, 180f);
            GameObject komadai2 = GameObject.Find(App.KOMADAI2_NAME);
            komadai2.transform.Rotate(0f, 0f, 180f);
            komadai2.transform.localPosition = App.ReverseVector(komadai2.transform.localPosition);
            GameObject.Find(App.KOMADAI1_NAME).transform.Rotate(0f, 0f, 180f);
        }
    }

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
        
        //「SET」の時
        if(App.game_type == GAME_TYPE.SET) {
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