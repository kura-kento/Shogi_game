using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
       PhotonMaster.GM.AddDonePlayer();
       App.game_type = GAME_TYPE.WAIT;
       Debug.Log("配置モード　→　待ち");
    }
}
