using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    //手札のカードを保持する
    List<Card> cards = new List<Card>();

    // 最初に手札を受け取る。
    public void SethandCards(List<Card> cards)
    {
        this.cards = cards;
        foreach (Card card in cards)
        {
            Debug.Log("カードを受け取るました"+card.number);
        }
        RefreshHand();
    }

    //手札を整列する。
    void RefreshHand()
    {

    }
}
