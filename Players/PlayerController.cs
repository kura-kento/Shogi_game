using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //駒台から駒を
    public void CreateSelectObj() {
        foreach (Masu obj in FindObjectsOfType<Masu>())
        {
            if (obj.transform.childCount == 0) { //駒が置いていない場合(マスの子要素にデータが無い場合)
                //マスを選択できる状態にする
                GameMaster.SelectMasu(obj);
            }
        }
    }

    //マスから駒をクリック時
    public void SelectObj(Koma koma) {
        if (App.isTurePlayer1 && koma.number < 0) return;
        Debug.Log(App.isTurePlayer1 ? "先手" : "後手");
        var koma_p = koma.transform.position;
        //歩を押した時  
        if(Mathf.Abs(koma.number) == 2) {
            float interval = koma.number > 0 ? 1.5f : -1.5f;//
            var select_p = new Vector3(koma_p.x, koma_p.y + interval, koma_p.z);
            Debug.Log("これは歩です"+(koma_p.x,koma_p.y + interval,koma_p.z));
            
            foreach (Masu obj in FindObjectsOfType<Masu>())
            {
                Koma masu_koma = App.GetChildKoma(obj);
                // 駒が無い　または　(先攻+マイナス駒　または　後攻+プラス駒)
                if(masu_koma == null || (App.isTurePlayer1 &&  masu_koma.number < 0) || (App.isTurePlayer1 == false && masu_koma.number > 0)) {
                    // 上方向に１つ上のマスのみ指定する
                    if (obj.transform.position == select_p) {
                        GameMaster.SelectMasu(obj); //マスを選択できる状態にする
                    }
                }
            }
        }
    }
}
