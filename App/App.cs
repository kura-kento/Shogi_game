using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class App : MonoBehaviour
{
    public static int[][] masu_array = new int[4][];
    public static Koma slot = null;
    public static int MAX_Y = 4;
    public static int MAX_X = 4;

    public static Color32 Select_Color = new Color32(248, 168, 146, 255);
    public static Color32 Masu_Color   = new Color32(212, 187, 99, 255);
    // int int_x = 4;


    void Awake(){
        for(int i=0; i<App.MAX_Y; i++) { masu_array[i] = new int[] {0,0,0,0};}  
    }
}
