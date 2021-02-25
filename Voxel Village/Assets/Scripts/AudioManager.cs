using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip clip1;
    public AudioClip clip2;
    public AudioClip clip3;

    private int currentClip = 2;

    // Start is called before the first frame update
    void Start()
    {
        audioSource.loop = false;
        ChooseNextAudio();
    }

    // Update is called once per frame
    void Update()
    {
        if(!audioSource.isPlaying)
        {
            ChooseNextAudio();
        }
    }

    private void ChooseNextAudio()
    {
        currentClip = (currentClip + 1) % 3;
        if(currentClip == 0)
        {
            audioSource.clip = clip1;
        }
        else if(currentClip == 1)
        {
            audioSource.clip = clip2;
        }
        else
        {
            audioSource.clip = clip3;
        }
        audioSource.Play();
    }
}
