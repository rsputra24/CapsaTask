using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Player : MonoBehaviour
{
    public Character playerCharData = null;
    public GameObject line1;
    public GameObject line2;
    public GameObject line3;
    public bool isReady = false;
    public int playerIndex = 0;

    private List<Card> cardListInHand = new List<Card>();
    private List<Card> line1CardList = new List<Card>();
    private List<Card> line2CardList = new List<Card>();
    private List<Card> line3CardList = new List<Card>();

    private int score = 0;
    private int money = 0;
    private bool isBot = false;

    public static UnityAction OnPlayerReady;

    private void Start()
    {
        cardListInHand = new List<Card>();
        line1CardList = new List<Card>();
        line2CardList = new List<Card>();
        line3CardList = new List<Card>();
    }

    public void SetAsBot()
    {
        money = Global.BOTMONEY;
        isBot = true;
    }

    public bool GetIsBot()
    {
        return isBot;
    }

    public void SetIndex(int index)
    {
        playerIndex = index;
    }

    public void SetReady(bool value)
    {
        isReady = value;
        GameUIManager.instance.playersUIObjects[playerIndex].readyLabel.gameObject.SetActive(value);
        if (value)
        {
            OnPlayerReady?.Invoke();
        }
    }

    public void SetMoney(int value)
    {
        money += value;
        if (playerIndex == 0) //save player money
        {
            PlayerPrefs.SetInt("money", money);
        }
    }

    public int GetMoney()
    {
        return money;
    }

    public void SetScore(int value)
    {
        score += value;
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetCards()
    {
        line1.transform.DetachChildren();
        line2.transform.DetachChildren();
        line3.transform.DetachChildren();
        line1CardList.Clear();
        line2CardList.Clear();
        line3CardList.Clear();
        cardListInHand.Clear();
    }

    public List<Card> GetCardList(int index)
    {
        if (index == 1)
        {
            return line1CardList;
        }
        else if (index == 2)
        {
            return line2CardList;
        }
        else if (index == 3)
        {
            return line3CardList;
        }
        else
        {
            return cardListInHand;
        }
    }

    public void SwapCardBot(Card sourceCard, Card targetCard)
    {
        CardInfo sourceCardInfo = sourceCard.GetCardInfo();
        CardInfo targetCardInfo = targetCard.GetCardInfo();
        List<Card> sourceCardList = new List<Card>();
        List<Card> targetCardList = new List<Card>();
        GameObject deckSourceLineObject = sourceCard.transform.parent.gameObject;
        GameObject deckTargetLineObject = targetCard.transform.parent.gameObject;
        if (sourceCardInfo.row == 1)
        {
            sourceCardList = line1CardList;
        }
        else if (sourceCardInfo.row == 2)
        {
            sourceCardList = line2CardList;
        }
        else if (sourceCardInfo.row == 3)
        {
            sourceCardList = line3CardList;
        }
        if (targetCardInfo.row == 1)
        {
            targetCardList = line1CardList;
        }
        else if (targetCardInfo.row == 2)
        {
            targetCardList = line2CardList;
        }
        else if (targetCardInfo.row == 3)
        {
            targetCardList = line3CardList;
        }
        sourceCardList.Remove(sourceCard);
        targetCardList.Remove(targetCard);

        sourceCardList.Insert(sourceCardInfo.row, targetCard);
        targetCardList.Insert(targetCardInfo.row, sourceCard);

        sourceCard.transform.parent = deckTargetLineObject.transform;
        targetCard.transform.parent = deckSourceLineObject.transform;

        sourceCard.transform.SetSiblingIndex(targetCardInfo.col);
        targetCard.transform.SetSiblingIndex(sourceCardInfo.col);

        sourceCard.SetRowCol(targetCardInfo.row, targetCardInfo.col);
        targetCard.SetRowCol(sourceCardInfo.row, sourceCardInfo.col);
    }

    public void SwapCard(Card sourceCard, Card targetCard)
    {
        CardInfo sourceCardInfo = sourceCard.GetCardInfo();
        CardInfo targetCardInfo = targetCard.GetCardInfo();
        GameObject sourceLineObject = sourceCard.transform.parent.gameObject;
        GameObject targetLineObject = targetCard.transform.parent.gameObject;
        GameObject deckSourceLineObject = sourceCardInfo.cardOnDeck.transform.parent.gameObject;
        GameObject deckTargetLineObject = targetCardInfo.cardOnDeck.transform.parent.gameObject;
        List<Card> sourceCardList = new List<Card>();
        List<Card> targetCardList = new List<Card>();
        if (sourceCardInfo.row == 1)
        {
            sourceCardList = line1CardList;
        }
        else if (sourceCardInfo.row == 2)
        {
            sourceCardList = line2CardList;
        }
        else if (sourceCardInfo.row == 3)
        {
            sourceCardList = line3CardList;
        }
        if (targetCardInfo.row == 1)
        {
            targetCardList = line1CardList;
        }
        else if (targetCardInfo.row == 2)
        {
            targetCardList = line2CardList;
        }
        else if (targetCardInfo.row == 3)
        {
            targetCardList = line3CardList;
        }

        sourceCardList.Remove(sourceCard.cardOnDeck);
        targetCardList.Remove(targetCard.cardOnDeck);

        sourceCardList.Insert(sourceCardInfo.row, targetCard.cardOnDeck);
        targetCardList.Insert(targetCardInfo.row, sourceCard.cardOnDeck);

        //change controlable card position
        Vector3 tempSourcePos = sourceCard.GetComponent<RectTransform>().anchoredPosition;
        Vector3 tempTargetPos = targetCard.GetComponent<RectTransform>().anchoredPosition;
        sourceCard.transform.parent = targetLineObject.transform;
        targetCard.transform.parent = sourceLineObject.transform;
        sourceCard.transform.SetSiblingIndex(targetCardInfo.col);
        targetCard.transform.SetSiblingIndex(sourceCardInfo.col);
        sourceCard.GetComponent<RectTransform>().anchoredPosition = tempTargetPos;
        targetCard.GetComponent<RectTransform>().anchoredPosition = tempSourcePos;

        //change card on deck position
        sourceCardInfo.cardOnDeck.transform.parent = deckTargetLineObject.transform;
        targetCardInfo.cardOnDeck.transform.parent = deckSourceLineObject.transform;
        sourceCardInfo.cardOnDeck.transform.SetSiblingIndex(targetCardInfo.col);
        targetCardInfo.cardOnDeck.transform.SetSiblingIndex(sourceCardInfo.col);

        //apply changed position row and col for both controlable and card on deck
        sourceCard.SetRowCol(targetCardInfo.row, targetCardInfo.col);
        targetCard.SetRowCol(sourceCardInfo.row, sourceCardInfo.col);
        sourceCardInfo.cardOnDeck.SetRowCol(targetCardInfo.row, targetCardInfo.col);
        targetCardInfo.cardOnDeck.SetRowCol(sourceCardInfo.row, sourceCardInfo.col);
    }

    public void AddCardToList(int lineIndex, Card card)
    {
        int col = 0;
        if (lineIndex == 1)
        {
            line1CardList.Add(card);
            col = line1CardList.Count - 1;
        }
        else if (lineIndex == 2)
        {
            line2CardList.Add(card);
            col = line2CardList.Count - 1;
        }
        else if (lineIndex == 3)
        {
            line3CardList.Add(card);
            col = line3CardList.Count - 1;
        }
        card.SetHolder(this);
        card.SetRowCol(lineIndex, col);
        cardListInHand.Add(card);

        if (playerIndex != 0)
        {
            card.FaceBackward();
        }
    }

    public Character GetPlayerCharData()
    {
        return playerCharData;
    }

    public void SetPlayerData(Character value)
    {
        playerCharData = value;
    }
}
