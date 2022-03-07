using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;
    public List<Card> pooledCards;
    public GameObject cardPrefab;

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
        pooledCards = new List<Card>();
        for (int i = 0; i < Global.CARDSCOUNT; i++)
        {
            GameObject cardObject = Instantiate(cardPrefab, gameObject.transform);
            Card card = cardObject.GetComponent<Card>();
            card.Init();
            card.SetCardInfo(i);
            cardObject.SetActive(false);
            pooledCards.Add(card);
        }
    }

    public Card GetPooledCard(int index)
    {
        return pooledCards[index];
    }

    public void ReturnCardToPool(Card card)
    {
        card.transform.parent = gameObject.transform;
        card.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        foreach (Card card in pooledCards)
        {
            Destroy(card);
        }
        pooledCards.Clear();
    }
}
