using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // 追加しましょう

public class ProfileData : MonoBehaviour {

    void Start ()
    {
        //プロフィール画像
        int image_id = PlayerPrefs.GetInt ("image_id");
        SqliteDatabase sqlDB = new SqliteDatabase("config.db");
        string selectQuery = "select * from images where id = " + image_id.ToString();
        // string selectQuery = "update images set image_name = 'Sarome' where id = " + image_id.ToString();
        DataTable dataTable = sqlDB.ExecuteQuery(selectQuery);
        GameObject ProfileImage = GameObject.Find("ProfileImage");
        Debug.Log(dataTable.Rows[0]["image_name"]);
        ProfileImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/ProfileImage/" + dataTable.Rows[0]["image_name"]);
        //名前
        string player_name = PlayerPrefs.GetString("player_name");
        GameObject.Find("PlayerName").GetComponent<Text>().text = player_name;

        //勝利数
        int win = PlayerPrefs.GetInt ("win");
        GameObject.Find("WIN").GetComponent<Text>().text = win.ToString() + "勝";
    }

    void OnDestroy() {
        // スコアを保存
        // PlayerPrefs.SetInt ("SCORE", score_num);
        // PlayerPrefs.Save ();
    }

}