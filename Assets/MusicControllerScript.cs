using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControllerScript : MonoBehaviour
{
    public AudioClip BossTrack;
    public AudioClip FightTrack;
    public AudioClip IdleTrack;
    public AudioClip StandartTrack;

    AudioSource player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public IEnumerator ChangeToBossMusic()
    {
        StartCoroutine(volDown(0.5f));
        yield return new WaitForSeconds(0.5f);
        player.clip = BossTrack;
        player.Play();
        StartCoroutine(volUp(1f));
    }
    public IEnumerator ChangeToFightMusic()
    {
        StartCoroutine(volDown(0.5f));
        yield return new WaitForSeconds(0.5f);
        player.clip = FightTrack;
        player.Play();
        StartCoroutine(volUp(1f));
    }
    public IEnumerator ChangeToIdleMusic()
    {
        StartCoroutine(volDown(0.5f));
        yield return new WaitForSeconds(0.5f);
        player.clip = IdleTrack;
        player.Play();
        StartCoroutine(volUp(1f));
    }
    public IEnumerator ChangeToStandartMusic()
    {
        StartCoroutine(volDown(0.5f));
        yield return new WaitForSeconds(0.5f);
        player.clip = StandartTrack;
        player.Play();
        StartCoroutine(volUp(1f));
    }

    IEnumerator volDown(float transitiontime)
    {
        for (float i = transitiontime; i > 0; i -= Time.deltaTime)
        {
            player.volume = i / transitiontime;
            yield return null;
        }
    }

    IEnumerator volUp(float transitiontime)
    {
        for (float i = 0; i < transitiontime; i += Time.deltaTime)
        {
            player.volume = i / transitiontime;
            yield return null;
        }
    }
    
}
