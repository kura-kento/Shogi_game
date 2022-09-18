using DG.Tweening;  
using UnityEngine;  
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleButton : MonoBehaviour,  
    IPointerClickHandler,  
    IPointerDownHandler,  
    IPointerUpHandler  
{
    public System.Action onClickCallback;  

    [SerializeField] private CanvasGroup _canvasGroup;  

    public void OnPointerClick(PointerEventData eventData)  
    {
        Debug.Log("BattleButton");
        PhotonMaster.FindOponent();
        onClickCallback?.Invoke();  
    }

    public void OnPointerDown(PointerEventData eventData)  
    {
        transform.DOScale(0.95f, 0.24f).SetEase(Ease.OutCubic);  
        _canvasGroup.DOFade(0.8f, 0.24f).SetEase(Ease.OutCubic);  
    }

    public void OnPointerUp(PointerEventData eventData)  
    {
        transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic);  
        _canvasGroup.DOFade(1f, 0.24f).SetEase(Ease.OutCubic);  
    }

    public void Update() {
        // 【ネット環境の状態】
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            // 機内モードなど、ネットワーク接続エラー状態
            this.GetComponent<Image>().color = new Color32(63, 63, 63, 255);
        }else
        {
            // ネットワーク接続OK状態  
            this.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
            } 
}