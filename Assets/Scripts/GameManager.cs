using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<Card> cardList;
    public GameObject cardPrefab;
    public GameObject table;

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

    void Start() {
        for (int i=0; i<Global.CARDSCOUNT; i++) {
            GameObject cardObject = Instantiate(cardPrefab, table.transform);
            Card card = cardObject.GetComponent<Card>();
            card.SetCardInfo(i);
            cardList.Add(card);
        }
        ShuffleCards();
        GameUIManager.instance.SetUI();
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
}
