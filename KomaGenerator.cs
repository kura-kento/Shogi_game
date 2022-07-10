using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KomaGenerator : MonoBehaviour
{

    [SerializeField] Koma komaPrefab;
    [SerializeField] Koma OuPrefab;

    // どこからでも使えるようにする
    public static KomaGenerator instance;

    private void Awake()
    {
        instance = this;
    }

    public Koma Spawn(int number) 
    {
        if(number == 1)
        {
            Koma Ou = Instantiate(OuPrefab);
            Ou.Init(number);
            return Ou;
        }
        else
        {
            Koma koma = Instantiate(komaPrefab);
            koma.Init(number);
            return koma;
        }
    }
}

