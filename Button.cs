using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{

    public GameObject btn;
    // Start is called before the first frame update
    void Start()
    {
        //  btn.Getnent<Button>().interactable = false;
    }

    // ボタンが押された場合、今回呼び出される関数
    public void OnClick()
    {
    //    PhotonMaster.GM.AddDonePlayer();
       
       //【待ち状態】押されたらApp.SET_ACTIONより再配置
       if(App.game_type == GAME_TYPE.WAIT) {
        //【全体】再構築
            PhotonMaster.GM.AllEvent2();
            Debug.Log("ここ？：" + App.SET_ACTION.Count);
            // foreach(var moveList in App.SET_ACTION){
            //     Debug.Log("koma_num:" + moveList["koma_num"] + "before:" + moveList["before"] + "after:" + moveList["after"]);
            // }
            // PhotonMaster.GM.MoveList(App.SET_ACTION);
       }else{
            //プレイヤーのコマをセット
            PhotonMaster.GM.AllSetAction();
       }
    
       App.game_type = GAME_TYPE.WAIT;
       Debug.Log("配置モード　→　待ち"+App.game_type.ToString());
    }
}
