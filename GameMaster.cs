using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Linq;

public class GameMaster : MonoBehaviour
{

    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    //マスのステータス(ログ)
    public static void MasuStatusLog() {
        for(int i=0; i<App.MAX_Y;i++)
        {
            string[] intArray = Array.ConvertAll(App.masu_array[App.MAX_Y-i-1], s => {return s.ToString();});
            Debug.Log((App.MAX_Y-i).ToString()+"行目"+string.Join(",", intArray)); 
        }
    }
}
