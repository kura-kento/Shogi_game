using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGenerator : MonoBehaviour
{

    [SerializeField] Card cardPrefab;

    // どこからでも使えるようにする
    public static CardGenerator instance;

    private void Awake()
    {
        instance = this;
    }

    // public void Start()
    // {

        
    // }

    public Card Spawn(int number) 
    {
        Card card = Instantiate(cardPrefab);
        card.Init(number);
        return card;
    }
}
