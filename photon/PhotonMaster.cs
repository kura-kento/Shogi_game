using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
public class PhotonMaster : MonoBehaviourPunCallbacks
{
    public TMP_Text statusText;
    private const int MaxPlayerPerRoom = 2;
    
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    // Start is called before the first frame update
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }

    //これをボタンにつける
    public void FindOponent()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    //Photonのコールバック //ルームに入室前に呼び出される
    public override void OnConnectedToMaster()
    {
        Debug.Log("マスターに繋ぎました。");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"{cause}の理由で繋げませんでした。");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("ルームを作成します。");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayerPerRoom });
    }

    //ルームに入室後に呼び出される
    public override void OnJoinedRoom()
    {
        Debug.Log("ルームに参加しました");
        //ここに入った

        // var gameObject = PhotonNetwork.Instantiate("Koma", new Vector3(0, 0, 0), Quaternion.identity, 0);

        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        if (playerCount != MaxPlayerPerRoom)
        {
            statusText.text = "対戦相手を待っています。";

                PhotonNetwork.CurrentRoom.IsOpen = false;
                statusText.text = "対戦相手が揃いました。バトルシーンに移動します。";
                PhotonNetwork.LoadLevel("BattleScene");
        }
        else
        {
            statusText.text = "対戦相手が揃いました。バトルシーンに移動します。";
        }
    }

    //ルーム作成者の処理
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom:ルーム作成者に通知 部屋に入りました");
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayerPerRoom)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                statusText.text = "対戦相手が揃いました。バトルシーンに移動します。";
                PhotonNetwork.LoadLevel("BattleScene");
            }
        }
    }
}
