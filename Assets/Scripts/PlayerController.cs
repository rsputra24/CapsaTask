using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Button controlButton;
    public Button closeButton;
    public Button readyButton;
    public GameObject controlObjects;
    public GameObject line1;
    public GameObject line2;
    public GameObject line3;
    public GameObject controlableCardPrefab;
    public CardPositionHelper cardPositionHelper;

    private List<Card> controlableCardList = new List<Card>();
    private Player player;
    private Card hoveredCard = null;
    private Card swapSourceCard = null;
    private Card swapTargetCard = null;
    private bool touchEnabled = false;

    void Start()
    {
        controlButton.onClick.AddListener(delegate { OnControlButtonClicked(); });
        closeButton.onClick.AddListener(delegate { OnCloseButtonClicked(); });
        readyButton.onClick.AddListener(delegate { OnReadyButtonClicked(); });
    }

    public void ResetCards()
    {
        foreach (Card card in controlableCardList)
        {
            Destroy(card);
        }
        controlableCardList.Clear();
    }

    public void Init()
    {
        player = Singleton.instance.playerManager.players[0];

        for (int i = 0; i < Global.LINE1CARDMAX; i++)
        {
            Vector3 pos = new Vector3(cardPositionHelper.line1Pos[i].x, cardPositionHelper.line1Pos[i].y, 0);
            CardInfo info = player.GetCardList(1)[i].GetCardInfo();
            Card card = Instantiate(controlableCardPrefab, line1.transform).GetComponent<Card>();
            card.Init();
            card.SetHolder(player);
            card.SetRowCol(info.row, info.col);
            card.SetCardInfo(info, player.GetCardList(1)[i]);
            card.GetComponent<RectTransform>().anchoredPosition = pos;
            controlableCardList.Add(card);
        }
        for (int i = 0; i < Global.LINE23CARDMAX; i++)
        {
            Vector3 pos = new Vector3(cardPositionHelper.line2Pos[i].x, cardPositionHelper.line2Pos[i].y, 0);
            CardInfo info = player.GetCardList(2)[i].GetCardInfo();
            Card card = Instantiate(controlableCardPrefab, line2.transform).GetComponent<Card>();
            card.Init();
            card.SetHolder(player);
            card.SetRowCol(info.row, info.col);
            card.SetCardInfo(info, player.GetCardList(2)[i]);
            card.GetComponent<RectTransform>().anchoredPosition = pos;
            controlableCardList.Add(card);
        }
        for (int i = 0; i < Global.LINE23CARDMAX; i++)
        {
            Vector3 pos = new Vector3(cardPositionHelper.line3Pos[i].x, cardPositionHelper.line3Pos[i].y, 0);
            CardInfo info = player.GetCardList(3)[i].GetCardInfo();
            Card card = Instantiate(controlableCardPrefab, line3.transform).GetComponent<Card>();
            card.Init();
            card.SetHolder(player);
            card.SetRowCol(info.row, info.col);
            card.SetCardInfo(info, player.GetCardList(3)[i]);
            card.GetComponent<RectTransform>().anchoredPosition = pos;
            controlableCardList.Add(card);
        }
    }

    void Update()
    {
        if (GameManager.instance.currentGameState == Global.GAMESTATE.GAME_SWAPPING)
        {
            readyButton.interactable = true;
            if (touchEnabled)
            {
                RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);
                if (hit.collider != null)
                {
                    Card thisCard = hit.transform.GetComponent<Card>();
                    /// when hovering a card
                    if (!hoveredCard)
                    {
                        if (!swapSourceCard)
                        {
                            hoveredCard = thisCard;
                            hoveredCard.SetColorTone(Color.green);
                        }
                        else if (swapSourceCard && swapSourceCard != thisCard)
                        {
                            hoveredCard = thisCard;
                            hoveredCard.SetColorTone(Color.yellow);
                        }
                    }
                    else if (hoveredCard != thisCard)
                    {
                        if (!swapSourceCard)
                        {
                            hoveredCard.SetColorTone(Color.white);
                            hoveredCard = thisCard;
                            hoveredCard.SetColorTone(Color.green);
                        }
                        else if (swapSourceCard && swapSourceCard != thisCard)
                        {
                            hoveredCard = thisCard;
                            hoveredCard.SetColorTone(Color.yellow);
                        }
                    }
                    /// when clicking a card
                    if (Input.GetMouseButtonUp(0))
                    {
                        if (!swapSourceCard)
                        {
                            hoveredCard = null;
                            swapSourceCard = thisCard;
                            swapSourceCard.SetColorTone(Color.green);
                        }
                        else if (swapSourceCard != thisCard)
                        {
                            swapTargetCard = thisCard;
                        }
                        else if (swapSourceCard == thisCard)
                        {
                            swapSourceCard.SetColorTone(Color.white);
                            swapSourceCard = null;
                        }
                    }
                }
                else
                {
                    if (!swapSourceCard && hoveredCard)
                    {
                        hoveredCard.SetColorTone(Color.white);
                        hoveredCard = null;
                    }
                    else if (swapSourceCard && hoveredCard)
                    {
                        hoveredCard.SetColorTone(Color.white);
                        hoveredCard = null;
                    }
                }
            }
            if (swapSourceCard && swapTargetCard) //swap when source and target selected
            {
                SwapCard(swapSourceCard, swapTargetCard);
                swapSourceCard.SetColorTone(Color.white);
                swapTargetCard.SetColorTone(Color.white);
                swapSourceCard = null;
                swapTargetCard = null;
            }
        }
        if (GameManager.instance.currentGameState != Global.GAMESTATE.GAME_SWAPPING)
        {
            readyButton.interactable = false;
        }
    }

    public void OnControlButtonClicked()
    {
        controlObjects.SetActive(true);
        StartCoroutine(EnableTouch());
    }

    public void OnReadyButtonClicked()
    {
        player.SetReady(true);
        readyButton.interactable = false;
    }

    public void HideControllerUI()
    {
        controlObjects.SetActive(false);
        if (swapSourceCard)
        {
            swapSourceCard.SetColorTone(Color.white);
        }
        if (swapTargetCard)
        {
            swapTargetCard.SetColorTone(Color.white);
        }
        swapSourceCard = null;
        swapTargetCard = null;
        touchEnabled = false;
    }

    public void OnCloseButtonClicked()
    {
        HideControllerUI();
    }

    public void SwapCard(Card source, Card target)
    {
        player.SwapCard(source, target);
    }

    IEnumerator EnableTouch()
    {
        yield return new WaitForSeconds(0.1f);
        touchEnabled = true;
    }
}
