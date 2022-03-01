using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Global.CARDTYPE type;
    public Global.CARD number;
    bool isFacingBackward = false;
    bool isRed = false;

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
            transform.GetChild(0).GetComponent<Image>().sprite = Singleton.instance.spritesManager.cardNumberSpritesRed[(int) number];
        }
        else
        {
            transform.GetChild(0).GetComponent<Image>().sprite = Singleton.instance.spritesManager.cardNumberSpritesBlack[(int) number];
        }
        transform.GetChild(1).GetComponent<Image>().sprite = Singleton.instance.spritesManager.cardTypeSprites[(int) type];
        transform.GetChild(2).GetComponent<Image>().sprite = Singleton.instance.spritesManager.cardTypeSprites[(int)type];
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
