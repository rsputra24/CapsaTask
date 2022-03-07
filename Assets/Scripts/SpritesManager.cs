using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesManager : MonoBehaviour
{
    public static SpritesManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public List<Sprite> cardNumberSpritesBlack;
    public List<Sprite> cardNumberSpritesRed;
    public List<Sprite> cardTypeSprites;
    public Sprite cardFront;
    public Sprite cardBack;

    public SpritesManager()
    {

    }
}
