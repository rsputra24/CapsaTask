using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public struct CardRankComboInfo
{
    public Global.CARDSRANK rank;
    public Global.CARDTYPE type;
    public Global.CARD number;
    public Card highestCard;
    public List<Card> cardList;
    public List<List<Card>> pairsList;
    public float value;
}

public struct CardInfo
{
    public Global.CARDTYPE type;
    public Global.CARD number;
    public Player holder;
    public float value;
    public int row;
    public int col;
    public Card cardOnDeck;
}
public class Card : MonoBehaviour
{
    private Global.CARDTYPE type;
    private Global.CARD number;
    private Player holder;
    private GameObject backImage;
    private GameObject frontImage;
    private RectTransform rectTransform;
    private bool isFacingBackward = false;
    private bool isRed = false;
    private bool animateFlip = false;
    private float flipSpeed = 2.5f;
    private int row = 0;
    private int col = 0;
    public Card cardOnDeck;

    public bool controlable = false;
    public static UnityAction OnCardFlipped;

    public CardInfo GetCardInfo()
    {
        CardInfo info = new CardInfo
        {
            type = type,
            number = number,
            holder = holder,
            value = (int)number + (float)type / 5,
            row = row
        };
        if (controlable)
        {
            info.cardOnDeck = cardOnDeck;
        }
        return info;
    }

    public void SetRowCol(int row, int col)
    {
        this.row = row;
        this.col = col;
    }

    public void SetHolder(Player value)
    {
        holder = value;
    }

    public void SetCardInfo(int index)
    {
        if (index <= 12)
        {
            number = (Global.CARD)index;
            type = Global.CARDTYPE.CLUB;
        }
        else if (index <= 25)
        {
            number = (Global.CARD) index - 13;
            type = Global.CARDTYPE.DIAMOND;
            isRed = true;
        }
        else if (index <= 38)
        {
            number = (Global.CARD) index - 26;
            type = Global.CARDTYPE.HEART;
            isRed = true;
        }
        else
        {
            number = (Global.CARD) index - 39;
            type = Global.CARDTYPE.SPADE;
        }

        if (isRed)
        {
            frontImage.transform.GetChild(0).GetComponent<Image>().sprite = Singleton.instance.spritesManager.cardNumberSpritesRed[(int) number];
        }
        else
        {
            frontImage.transform.GetChild(0).GetComponent<Image>().sprite = Singleton.instance.spritesManager.cardNumberSpritesBlack[(int) number];
        }
        frontImage.transform.GetChild(1).GetComponent<Image>().sprite = Singleton.instance.spritesManager.cardTypeSprites[(int) type];
        frontImage.transform.GetChild(2).GetComponent<Image>().sprite = Singleton.instance.spritesManager.cardTypeSprites[(int)type];
    }

    public void SetCardInfo(CardInfo info, Card cardOnDeck = null)
    {
        holder = info.holder;
        number = info.number;
        type = info.type;
        if (controlable)
        {
            this.cardOnDeck = cardOnDeck;
        }
        if (type == Global.CARDTYPE.DIAMOND || type == Global.CARDTYPE.HEART)
        {
            isRed = true;
            frontImage.transform.GetChild(0).GetComponent<Image>().sprite = Singleton.instance.spritesManager.cardNumberSpritesRed[(int)number];
        }
        else
        {
            frontImage.transform.GetChild(0).GetComponent<Image>().sprite = Singleton.instance.spritesManager.cardNumberSpritesBlack[(int)number];
        }
        frontImage.transform.GetChild(1).GetComponent<Image>().sprite = Singleton.instance.spritesManager.cardTypeSprites[(int)type];
        frontImage.transform.GetChild(2).GetComponent<Image>().sprite = Singleton.instance.spritesManager.cardTypeSprites[(int)type];
    }

    public void FlipCard()
    {
        animateFlip = true;
        OnCardFlipped?.Invoke();
    }

    public void FaceBackward()
    {
        isFacingBackward = true;
        frontImage.SetActive(false);
        rectTransform.eulerAngles = new Vector3(0, 179.99f, 0);
    }

    public void Init()
    {
        backImage = transform.GetChild(0).gameObject;
        frontImage = transform.GetChild(1).gameObject;
        rectTransform = GetComponent<RectTransform>();
        rectTransform.eulerAngles = new Vector3(0, 0, 0);
    }

    void AnimateFlip()
    {
        if (!isFacingBackward)
        {
            float yAngle = rectTransform.eulerAngles.y + flipSpeed;
            rectTransform.eulerAngles = new Vector3(0, yAngle, 0);
            if (rectTransform.eulerAngles.y >= 90 && rectTransform.eulerAngles.y < 180)
            {
                frontImage.SetActive(false);
            }
            else if (rectTransform.eulerAngles.y > 180)
            {
                isFacingBackward = true;
                rectTransform.eulerAngles = new Vector3(0, 179.99f, 0);
                animateFlip = false;
            }
        }
        else
        {
            float yAngle = rectTransform.eulerAngles.y - flipSpeed;
            rectTransform.eulerAngles = new Vector3(0, yAngle, 0);
            if (rectTransform.eulerAngles.y <= 3.2f)
            {
                isFacingBackward = false;
                rectTransform.eulerAngles = new Vector3(0, 0, 0);
                animateFlip = false;
            }
            else if (rectTransform.eulerAngles.y <= 90)
            {
                frontImage.SetActive(true);
            }
        }
    }

    void Update()
    {
        if (animateFlip)
        {
            AnimateFlip();
        }
    }

    public void SetColorTone(Color color)
    {
        frontImage.GetComponent<Image>().color = color;
    }
}
