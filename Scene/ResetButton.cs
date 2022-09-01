using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick() {
        //コマセット時のみ使用可
        if(App.game_type == GAME_TYPE.SET){
            SceneManager.LoadScene (SceneManager.GetActiveScene().name);
        }

    }
}
