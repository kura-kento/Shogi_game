using UnityEngine;
using UnityEngine.UIElements;

public class TapEffect : MonoBehaviour
{
    public GameObject prefab;
    public float deleteTime = 1.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameObject.Find("HitEffect_B(Clone)") == null)
        {
            //マウスカーソルの位置を取得。
            var mousePosition = Input.mousePosition;
            mousePosition.z = 3f;
            GameObject clone = Instantiate(prefab, Camera.main.ScreenToWorldPoint(mousePosition),
                Quaternion.identity);
            Destroy(clone, deleteTime);
        }
    }
}