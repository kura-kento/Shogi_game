using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;



public class MasuGenerator : MonoBehaviour
{
    [SerializeField] Masu masuPrefab;

    // どこからでも使えるようにする
    public static MasuGenerator instance;
    public static int int_y = 6;
    public static int int_x = 6;
    public Player Player;
    

    // masu_init[0] = new int[]{0,1,1,1,1,0};
    //     {0,1,0,0,1,0},
    //     {0,1,1,1,1,0},
    //     {1,1,1,1,1,1},
    //     {1,1,0,0,1,1},
    //     {0,0,0,0,0,0},

    private void Awake()
    {
        Spawn();
        instance = this;
    }

    public void Spawn() 
    {
  
    // int[,] masu_init = new int[6, 6]{
    //     {0,1,1,1,1,0},
    //     {0,1,0,0,1,0},
    //     {0,1,1,1,1,0},
    //     {1,1,1,1,1,1},
    //     {1,1,0,0,1,1},
    //     {0,0,0,0,0,0},
    // };
    int[,] masu1_init = new int[3, 6]{
        {0,1,1,1,1,0},
        {0,1,0,0,1,0},
        {0,1,1,1,1,0},
    };

    int[,] masu2_init = new int[3, 6]{
        {1,1,1,1,1,1},
        {1,1,0,0,1,1},
        {0,0,0,0,0,0},
    };



        for(int i=0; i<3; i++) {
            for(int j=0; j<int_x; j++) {
                
                Masu masu = Instantiate(masuPrefab);
                float y = 2.5f - (1.0f * i);//6枚
                float x = 2.5f - (1.0f * j);//6枚
                masu.transform.localPosition = new Vector3(x * Masu.width, y *Masu.hight, 0);
                // masu.name = "Masu" + i.ToString()+ "_" + j.ToString();
                masu.name = "Masu";
                if (masu1_init[i,j] == 1 ) {
                    masu.Init(-1);
                    masu.ClickAction = SelectMasu; //クリックされた時関数を呼ぶ
                } else {
                    masu.Init(99);//使用でき無いマス
                    masu.tag = "NoUse";
                    masu.GetComponent<SpriteRenderer>().color = App.No_Use_Color;
                    masu.GetComponent<Renderer>().sortingLayerName = "front++"; 
                }

            }
        }

        for(int i=0; i<3; i++) {
            for(int j=0; j<int_x; j++) {
                
                Masu masu = Instantiate(masuPrefab);
                float y = 2.5f - (1.0f * i+3);//6枚
                float x = 2.5f - (1.0f * j);//6枚
                masu.transform.localPosition = new Vector3(x * Masu.width, y *Masu.hight, 0);
                // masu.name = "Masu" + i.ToString()+ "_" + j.ToString();
                masu.name = "Masu";
                if (masu2_init[i,j] == 1 ) {
                    masu.Init(1);
                    masu.ClickAction = SelectMasu; //クリックされた時関数を呼ぶ
                } else {
                    masu.Init(99);//使用でき無いマス
                    masu.tag = "NoUse";
                    masu.GetComponent<SpriteRenderer>().color = App.No_Use_Color;
                    masu.GetComponent<Renderer>().sortingLayerName = "front++"; 
                }

            }
        }
    }

    public async void SelectMasu(Masu masu)
    {
        // コマが選択されている時
        if(App.slot != null) {
            //コマが選択されている時 かつ　「選択可能マス」
            if(masu.tag == "Select") {
                //選択したマスの駒情報 取得
                Koma masu_koma = App.GetChildKoma(masu);

                GameObject parent = App.slot.transform.parent.gameObject;

                string komadai_name = App.isTurePlayer1 ? "Komadai" : "Komadai2";
                GameObject komadai_obj = GameObject.Find(komadai_name);
                //駒台
                if(parent.name == "Komadai" || parent.name == "Komadai2") {      
                } 
                //盤上
                else {
                    // 駒を成る
                    // （相手のマスに入る または　相手のマスから出る）　かつ　成れる駒
                    // マス ✖️ 駒 = マイナスなら相手のマス 　-1 * +1 = -1　または +1 * -1 = -1
                    int[] evolutionBefore = new int[] {2,3,4,5,7,8};
                    if(((masu.MasuStatus * App.slot.number) < 0 || (parent.GetComponent<Masu>().MasuStatus * App.slot.number) < 0) && Array.IndexOf(evolutionBefore, Mathf.Abs(App.slot.number)) >= 0 ) {
                        //駒を成りますか？
                        // MyDialog.Confirm("駒を成りますか？", Click_OK, Click_Cancel);
                        AnimatedDialog.instance.Open();
                        await TestUniTask();

                        //TODO:不具合 インスタンスがない
                        App.slot.number = (App.slot.number > 0 ? 1 : -1) * (9 + Array.IndexOf(evolutionBefore, Mathf.Abs(App.slot.number)));
                        KomaGenerator.instance.TextChange(App.slot);
                    }

                    //駒有り　かつ　相手の駒の時
                    if(masu_koma != null &&  App.isEnemyKoma(masu_koma)) {
                        //駒を取った時の処理
                        masu_koma.tag = "Komadai";
                        masu_koma.number *= -1;//ステータスを自分のコマに

                        Koma new_koma = App.isTurePlayer1 ?
                        Player.instance.CreateKoma(masu_koma.number):
                        Player2.instance.CreateKoma(masu_koma.number);

                        Destroy(masu_koma.transform.gameObject);
                        new_koma.transform.parent = komadai_obj.transform;
                    }
                }
                App.slot.transform.parent = masu.transform; //マスを親にする。
                //使ったら駒台の位置を並び替える。
                var koma_children = komadai_obj.GetComponentsInChildren<Koma>();
                int i = 0;
                foreach(Koma koma_child in koma_children) {
                    koma_child.transform.localPosition = new Vector3(KomadaiVectorX(i), 0, 0);
                    i++;
                }
                //マスの状態をリセットする
                GameMaster.ResetMasu();
                App.slot.transform.localPosition = new Vector3(0, 0, 0);
                App.slot = null;

                // TODO:ここに勝利判定を入れる
                // 玉が取られる
                // 全駒
                // タッチダウン
                TurnEnd();//ダーン終了の処理
            }
        } 

    }

    //駒台の座標
    public float KomadaiVectorX(int i) {
        return App.isTurePlayer1 ? 0.40f - 0.2f * i : -0.40f + 0.2f * i;
    }

    //ダーン終了の処理
    public void TurnEnd() {
        App.isTurePlayer1 = !(App.isTurePlayer1);
        App.Turn++; //ターン数増やす
        App.turnUp();
    }

    //ダイアログが押されるまで待つ
    async UniTask TestUniTask()  
    {
        //ダイアログが閉じるまで処理を止める
        await UniTask.WaitUntil(() => AnimatedDialog.IsClose);
    }

}
