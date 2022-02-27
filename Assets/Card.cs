using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public Global.CARDTYPE type;
    public Global.CARD number;

    public void SetCardInfo(int index)
    {
        if (index <= 12)
        {
            number = (Global.CARD)index;
            type = Global.CARDTYPE.CLUB;
        }
        else if (index <= 25)
        {
            Debug.Log("index " + index);
            number = (Global.CARD) index - 13;
            type = Global.CARDTYPE.DIAMOND;
        }
        else if (index <= 38)
        {
            number = (Global.CARD) index - 26;
            type = Global.CARDTYPE.HEART;
        }
        else
        {
            number = (Global.CARD) index - 39;
            type = Global.CARDTYPE.SPADE;
        }
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
