using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

public class ResultDialog : AnimatedDialog
{
    //インスタンス化
    public static ResultDialog instance;
    
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    // public void Evolt() {
    //     Debug.Log("ダイアログ:OK");
    //     Close();
    // }

    // public void NoEvolt() {
    //     Close();
    // }
}