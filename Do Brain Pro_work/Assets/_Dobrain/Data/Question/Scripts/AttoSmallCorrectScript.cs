using System.Collections;
using UnityEngine;

public class AttoSmallCorrectScript : MonoBehaviour
{
    public AudioSource attoSoundSource;
    public AudioClip[] attoSounds;
    public float delay = 0.1f;


    void OnEnable()
    {
        attoSoundSource.clip = attoSounds[Random.Range(0, 4)];
        StartCoroutine(PlayAttoSound());
    }


    IEnumerator PlayAttoSound()
    {
        yield return new WaitForSeconds(delay);
        attoSoundSource.Play();
    }
}
