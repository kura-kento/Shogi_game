using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;  // 追加しましょう

public class Prefs : MonoBehaviour {

    void Start ()
    {
        //プロフィール画像ロック
        PlayerPrefs.SetInt ("Image1", 0);
        PlayerPrefs.SetInt ("Image2", 0);
        PlayerPrefs.SetInt ("Image3", 0);
        PlayerPrefs.SetInt ("Image4", 0);
        PlayerPrefs.SetInt ("Image5", 0);
        //名前
        PlayerPrefs.GetString("Name", " SIYA");
        //勝利数
        PlayerPrefs.SetInt ("win", 0);
        PlayerPrefs.Save ();
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