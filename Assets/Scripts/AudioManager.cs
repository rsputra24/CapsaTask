using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> audioClips;

    private void OnEnable()
    {
        Card.OnCardFlipped += PlayFlipSound;
    }
    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void PlaySFX(Global.AUDIOCLIPS clip)
    {
        audioSource.PlayOneShot(audioClips[(int) clip]);
    }

    public void PlayFlipSound()
    {
        PlaySFX(Global.AUDIOCLIPS.FLIP);
    }
}
