using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

/// <summary>
/// ダイアログのアニメーション
/// </summary>
public class AnimatedDialog : MonoBehaviour
{
    // アニメーター
    [SerializeField] private Animator _animator;

    // アニメーターコントローラーのレイヤー(通常は0)
    [SerializeField] private int _layer;

    // IsOpenフラグ(アニメーターコントローラー内で定義したフラグ)
    private static readonly int ParamIsOpen = Animator.StringToHash("isOpen");

    // ダイアログは開いているかどうか
    public bool IsOpen => gameObject.activeSelf;

    // アニメーション中かどうか
    public bool IsTransition { get; private set; }

    //インスタンス化
    public static AnimatedDialog instance;
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

    // ダイアログを開く
    public void Open()
    {
        
        // 不正操作防止
        if (IsOpen || IsTransition) return;
        Debug.Log("ダイアログを開く");
        IsClose = false;
        // パネル自体をアクティブにする
        gameObject.SetActive(true);

        // IsOpenフラグをセット
        _animator.SetBool(ParamIsOpen, true);

        // アニメーション待機
        StartCoroutine(WaitAnimation("Open Dialog"));
    }    

    public void Evolt() {
        IsEvolt = true;
        IsClose = true;
        Close();
    }

    public void NoEvolt() {
        IsEvolt = false;
        IsClose = true;
        Close();
    }
    
    // ダイアログを閉じる
    public void Close()
    {
        // 不正操作防止
        if (!IsOpen || IsTransition) return;

        // IsOpenフラグをクリア
        _animator.SetBool(ParamIsOpen, false);

        // アニメーション待機し、終わったらパネル自体を非アクティブにする
        StartCoroutine(WaitAnimation("Hidden", () => gameObject.SetActive(false)));
    }

    // 開閉アニメーションの待機コルーチン
    private IEnumerator WaitAnimation(string stateName, UnityAction onCompleted = null)
    {
        IsTransition = true;

        yield return new WaitUntil(() =>
        {
            // ステートが変化し、アニメーションが終了するまでループ
            var state = _animator.GetCurrentAnimatorStateInfo(_layer);
            return state.IsName(stateName) && state.normalizedTime >= 1;
        });

        IsTransition = false;

        onCompleted?.Invoke();
    }
}