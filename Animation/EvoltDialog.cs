using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

/// <summary>
/// ダイアログのアニメーション
/// </summary>
public class EvoltDialog : AnimatedDialog
{

    //インスタンス化
    public static EvoltDialog instance;
    public static bool IsClose;
    public static bool IsEvolt; //駒を成るか？
    
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gameObject.SetActive(false);
        // Close();
    }

    public void Evolt() {
        IsEvolt = true;
        IsClose = true;
        Debug.Log("ダイアログ:OK");
        Close();
    }

    public void NoEvolt() {
        IsEvolt = false;
        IsClose = true;
        Close();
    }
}