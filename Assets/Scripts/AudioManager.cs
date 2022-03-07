using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSourceBg;
    public AudioSource audioSource;
    public List<AudioClip> audioClips;
    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        if (!audioSourceBg.isPlaying)
        {
            audioSourceBg.Play();
        }
    }
    private void OnEnable()
    {
        Card.OnCardFlipped += PlayFlipSound;
        Player.OnPlayerReady += PlayReadySound;
    }

    private void OnDisable()
    {
        Card.OnCardFlipped -= PlayFlipSound;
        Player.OnPlayerReady -= PlayReadySound;
    }

    public void PlaySFX(Global.AUDIOCLIPS clip)
    {
        audioSource.PlayOneShot(audioClips[(int) clip]);
    }

    public void PlayFlipSound()
    {
        PlaySFX(Global.AUDIOCLIPS.FLIP);
    }
    public void PlayReadySound()
    {
        PlaySFX(Global.AUDIOCLIPS.READY);
    }
}
