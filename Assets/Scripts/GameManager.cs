using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<Card> cardList;
    public List<GameObject> playersDeckObjects;

    public GameObject cardPrefab;
    public GameObject table;
    public Global.GAMESTATE currentGameState;
    public float swapTimeLeft = 0;

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
        SetPlayersDeck();
        StartGame();
        //StartCoroutine(StartNewGame(0.5f));
    }

    void PrepareCards()
    {
        for (int i = 0; i < Global.CARDSCOUNT; i++)
        {
            Card card = PoolManager.instance.GetPooledCard(i);
            card.gameObject.SetActive(true);
            cardList.Add(card);
        }
        
    }

    void ResetCards()
    {
        for (int i = 0; i < Global.PLAYERSCOUNT; i++)
        {
            players[i].SetReady(false);
            players[i].ResetCards();
            if (i == 0)
            {
                FindObjectOfType<PlayerController>().ResetCards();
            }
        }
        foreach(Card card in cardList)
        {
            PoolManager.instance.ReturnCardToPool(card);
        }
        cardList.Clear();
    }

    void StartGame()
    {
        ResetCards();
        PrepareCards();
        GameUIManager.instance.SetUI();
        currentGameState = Global.GAMESTATE.GAME_START;
        swapTimeLeft = Global.SWAPTIME;
        AdvanceNextState();
    }

    IEnumerator StartNewGame(float delay = 3)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("start new game");
        StartGame();
    }

    void AdvanceNextState()
    {
        switch (currentGameState)
        {
            case Global.GAMESTATE.GAME_START:
                currentGameState = Global.GAMESTATE.GAME_DISTRIBUTING;
                DistributeCardsToPlayers();
                break;
            case Global.GAMESTATE.GAME_DISTRIBUTING:
                currentGameState = Global.GAMESTATE.GAME_SWAPPING;
                break;
            case Global.GAMESTATE.GAME_SWAPPING:
                FindObjectOfType<PlayerController>().HideControllerUI();
                currentGameState = Global.GAMESTATE.GAME_COMPARING;
                CompareCards();
                break;
            case Global.GAMESTATE.GAME_COMPARING:
                currentGameState = Global.GAMESTATE.GAME_RESULT;
                PlayerResult();
                break;
            case Global.GAMESTATE.GAME_RESULT:
                StartCoroutine(StartNewGame());
                break;
        }
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
        for (int i = 0; i < cardList.Count; i++)
        {
            Card tempCard = cardList[i];
            int randomIndex = Random.Range(i, cardList.Count);
            cardList[i] = cardList[randomIndex];
            cardList[randomIndex] = tempCard;
        }
    }

    void Update()
    {
        SwappingState();
    }

    void SwappingState()
    {
        if (currentGameState == Global.GAMESTATE.GAME_SWAPPING)
        {
            swapTimeLeft -= Time.deltaTime;
            if (swapTimeLeft <= 0)
            {
                AdvanceNextState();
            }

            int readyCount = 0;
            for (int i = 0; i < Global.PLAYERSCOUNT; i++)
            {
                if (players[i].isReady)
                {
                    readyCount++;
                }
            }
            if (readyCount >= Global.PLAYERSCOUNT)
            {
                AdvanceNextState();
            }
        }
    }

    void DistributeCardsToPlayers()
    {
        ShuffleCards();
        int cardIndex = 0;
        for (int i = 0; i < Global.PLAYERSCOUNT; i++)
        {
            for (int j = 0; j < Global.LINE1CARDMAX; j++)
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

            if (i == 0)
            {
                FindObjectOfType<PlayerController>().Init();
            }
        }
        AdvanceNextState();
    }

    void CompareCards()
    {
        StartCoroutine(CompareCoroutine());
    }

    IEnumerator CompareCoroutine()
    {
        List<CardRankComboInfo> cardRankComboInfos = new List<CardRankComboInfo>();

        //first line
        for (int i = 0; i < Global.PLAYERSCOUNT; i++)
        {
            Player player = players[i];
            players[i].SetReady(false);
            if (i != 0)
            {
                for (int j = 0; j < Global.LINE1CARDMAX; j++)
                {
                    players[i].GetCardList(1)[j].FlipCard();
                }
            }
            CardRankComboInfo cardRankComboInfo = CapsaLogic.CheckCardsRank(player.GetCardList(1));
            cardRankComboInfos.Add(cardRankComboInfo);
            Debug.Log("player " + players[i].playerIndex + ", rank " + cardRankComboInfo.rank + ", number " + cardRankComboInfo.number + ", type " + cardRankComboInfo.type);
        }
        PlayersScoring(cardRankComboInfos);
        yield return new WaitForSeconds(1.5f);

        //second line
        cardRankComboInfos.Clear();
        for (int i = 0; i < Global.PLAYERSCOUNT; i++)
        {
            Player player = players[i];
            if (i != 0)
            {
                for (int j = 0; j < Global.LINE23CARDMAX; j++)
                {
                    players[i].GetCardList(2)[j].FlipCard();
                }
            }
            CardRankComboInfo cardRankComboInfo = CapsaLogic.CheckCardsRank(player.GetCardList(2));
            cardRankComboInfos.Add(cardRankComboInfo);
        }
        PlayersScoring(cardRankComboInfos);
        yield return new WaitForSeconds(1.5f);

        //third line
        cardRankComboInfos.Clear();
        for (int i = 0; i < Global.PLAYERSCOUNT; i++)
        {
            Player player = players[i];
            if (i != 0)
            {
                for (int j = 0; j < Global.LINE23CARDMAX; j++)
                {
                    players[i].GetCardList(3)[j].FlipCard();
                }
            }
            CardRankComboInfo cardRankComboInfo = CapsaLogic.CheckCardsRank(player.GetCardList(3));
            cardRankComboInfos.Add(cardRankComboInfo);
        }
        PlayersScoring(cardRankComboInfos);
        yield return new WaitForSeconds(1.5f);

        AdvanceNextState();
    }

    void PlayersScoring(List<CardRankComboInfo> cardRankComboInfos)
    {
        int size = cardRankComboInfos.Count;
        cardRankComboInfos.Sort((card1, card2) => card1.value.CompareTo(card2.value));
        for (int i = 0; i < size; i++)
        {
            Player player = cardRankComboInfos[i].highestCard.GetCardInfo().holder;
            int score = 0;
            if (i == 0)
            {
                score = -size;
            } else if(i == size - 1)
            {
                score = size;
            }
            else
            {
                if(i == 2 || size == 3)
                {
                     score =1;
                }
                else if(i == 1)
                {
                     score =-1;
                }
            }
            player.SetScore(score);
            GameUIManager.instance.DisplayCardsRankScore(player, cardRankComboInfos[i].rank, score);
            GameUIManager.instance.UpdateUI();
        }
    }

    void PlayerResult()
    {
        StopCoroutine(CompareCoroutine());
        for (int i = 0; i < Global.PLAYERSCOUNT; i++)
        {
            Player player = players[i];
            int moneyEarned = player.GetScore() * Global.BET;

            player.SetMoney(moneyEarned);
            GameUIManager.instance.DisplayEarning(player, moneyEarned);

            if (moneyEarned > 0)
            {
                Singleton.instance.audioManager.PlaySFX(Global.AUDIOCLIPS.WIN);
            }
            else
            {
                Singleton.instance.audioManager.PlaySFX(Global.AUDIOCLIPS.LOSE);
            }
        }
        GameUIManager.instance.UpdateUI();
        AdvanceNextState();
    }

    public void LeaveGame()
    {
        Singleton.instance.playerManager.ResetData();
        SceneManager.LoadScene("MenuScene");
    }
}
