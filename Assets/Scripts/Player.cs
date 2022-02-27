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
    private List<Card> cardListInHand;
    private List<Card> line1CardList;
    private List<Card> line2CardList;
    private List<Card> line3CardList;

    public Player()
    {
    }
    public List<Card> GetCardList(int index)
    {
        if(index == 1)
        {
            return line1CardList;
        } else if(index ==2)
        {
            return line2CardList;
        } else if (index == 3)
        {
            return line3CardList;
        } else
        {
            return cardListInHand;
        }
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
