using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    public Character playerData = null;
    public GameObject line1;
    public GameObject line2;
    public GameObject line3;
    private List<Card> cardListInHand = new List<Card>();
    private List<Card> line1CardList = new List<Card>();
    private List<Card> line2CardList = new List<Card>();
    private List<Card> line3CardList = new List<Card>();
    public bool isReady = false;

    public Player()
    {
    }
    public List<Card> GetCardList(int index)
    {
        if(index == 1) {
            return line1CardList;
        } else if(index ==2) {
            return line2CardList;
        } else if (index == 3) {
            return line3CardList;
        } else {
            return cardListInHand;
        }
    }

    public void AddCardToList(int lineIndex, Card card)
    {
        if (lineIndex == 1) {
            line1CardList.Add(card);
        } else if (lineIndex == 1) {
            line2CardList.Add(card);
        } else if (lineIndex == 1) {
            line3CardList.Add(card);
        }
        cardListInHand.Add(card);
    }

    public Character GetPlayerData()
    {
        return playerData;
    }

    public void SetPlayerData(Character value)
    {
        playerData = value;
    }
}
