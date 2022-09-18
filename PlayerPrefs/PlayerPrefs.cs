using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;  // 追加しましょう

public class Prefs : MonoBehaviour {

    void Start ()
    {
    //     //プロフィール画像
    //     PlayerPrefs.SetInt ("image_id", 1);
    //     //名前
    //     PlayerPrefs.SetString("player_name", "PLAYER NAME");
    //     //勝利数
    //     PlayerPrefs.SetInt ("win", 0);
    //     PlayerPrefs.Save ();
    }

    void OnDestroy() {
        // スコアを保存
        // PlayerPrefs.SetInt ("SCORE", score_num);
        // PlayerPrefs.Save ();
    }

}
        // PlayerPrefs.SetFloat("Health", 50.0f);
        // PlayerPrefs.GetInt("Score", 20);
        // PlayerPrefs.GetString("Player Name", "Foobar");