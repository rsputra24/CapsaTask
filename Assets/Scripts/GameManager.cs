using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<Card> cardList;
    public GameObject cardPrefab;
    public GameObject table;
    public List<GameObject> playersDeckObjects;

    private Player[] players = new Player[4];

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    void Start()
    {
        players = Singleton.instance.playerManager.players;
        GameUIManager.instance.SetUI();
        for (int i=0; i<Global.CARDSCOUNT; i++) {
            GameObject cardObject = Instantiate(cardPrefab, table.transform);
            Card card = cardObject.GetComponent<Card>();
            card.SetCardInfo(i);
            cardList.Add(card);
        }
        SetPlayersDeck();
        ShuffleCards();
        MoveCardsToPlayers();
    }

    void SetPlayersDeck()
    {
        for (int i = 0; i < Global.PLAYERSCOUNT; i++)
        {
            players[i].line1 = playersDeckObjects[i].transform.GetChild(0).gameObject;
            players[i].line2 = playersDeckObjects[i].transform.GetChild(1).gameObject;
            players[i].line3 = playersDeckObjects[i].transform.GetChild(2).gameObject;
        }
    }

    void ShuffleCards()
    {
        for(int i=0; i<cardList.Count; i++)
        {
            Card tempCard = cardList[i];
            int randomIndex = Random.Range(i, cardList.Count);
            cardList[i] = cardList[randomIndex];
            cardList[randomIndex] = tempCard;
        }
    }

    void Update() {
        
    }

    void MoveCardsToPlayers()
    {
        int cardIndex = 0;
        for (int i = 0; i < Global.PLAYERSCOUNT; i++) {
            for(int j = 0; j<Global.LINE1CARDMAX; j++)
            {
                cardList[cardIndex].transform.parent = players[i].line1.transform;
                players[i].AddCardToList(1, cardList[cardIndex]);
                cardIndex++;
            }
            for (int j = 0; j < Global.LINE23CARDMAX; j++)
            {
                cardList[cardIndex].transform.parent = players[i].line2.transform;
                players[i].AddCardToList(2, cardList[cardIndex]);
                cardIndex++;
            }
            for (int j = 0; j < Global.LINE23CARDMAX; j++)
            {
                cardList[cardIndex].transform.parent = players[i].line3.transform;
                players[i].AddCardToList(3, cardList[cardIndex]);
                cardIndex++;
            }
        }
        Debug.Log("card index " + cardIndex);
    }
}
