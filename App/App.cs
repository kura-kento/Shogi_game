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
    public static int int_y = 4;
    // int int_x = 4;


    void Awake(){
        for(int i=0; i<App.int_y; i++) { App.masu_array[i] = new int[] {0,0,0,0};}   
    }
}
