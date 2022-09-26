using DG.Tweening;  
using UnityEngine;  
using UnityEngine.EventSystems;  

public class SurrenderButton : MonoBehaviour,  
    IPointerClickHandler,  
    IPointerDownHandler,  
    IPointerUpHandler  
{
    public System.Action onClickCallback;  

    [SerializeField] private CanvasGroup _canvasGroup;  
    //爆破エフェクト
    [SerializeField] private GameObject explosionPrefab;

    public void OnPointerClick(PointerEventData eventData)  
    {
        Debug.Log("SurrenderButton");
        GameObject gameobject = GameObject.Find("Profile1");
        Instantiate (explosionPrefab, gameobject.transform.position, Quaternion.identity);
        Destroy(gameobject);
        //勝ち判定
        ResultDialog.instance.Open();
        Debug.Log("かち");
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
}