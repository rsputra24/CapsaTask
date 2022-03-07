using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton instance { get; private set; }

    public AudioManager audioManager { get; private set; }
    public SpritesManager spritesManager { get; private set; }
    public PlayerManager playerManager { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        audioManager = GetComponentInChildren<AudioManager>();
        spritesManager = GetComponentInChildren<SpritesManager>();
        playerManager = GetComponentInChildren<PlayerManager>();
    }
}
