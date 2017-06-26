using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class QuestionSoundManager : MonoBehaviour
{

    private AudioSource BeforeAnimSound;
    private AudioSource QuestionSound;
    private AudioSource EffectSound;
    private bool isQuestionSoundPlayed = false;
    private bool isBeforeAnimSoundPlayed = false;
    private float defaultDuration = 0;

    public AudioClip[] effectSounds;

    void Awake()
    {
        Init();
    }
    void Init()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        BeforeAnimSound = audioSources[0];
        QuestionSound = audioSources[1];
        EffectSound = audioSources[2];
    }

    public void SetBeforeAnimSound(AudioClip audioClip)
    {
        if (BeforeAnimSound == null)
            Init();
        BeforeAnimSound.clip = audioClip;
    }

    public void SetQuestionSound(AudioClip audioClip)
    {
        if (QuestionSound == null)
            Init();
        QuestionSound.clip = audioClip;
    }

    public void PlayBeforeSound()
    {
        if (!isBeforeAnimSoundPlayed && BeforeAnimSound.clip != null)
        {
            BeforeAnimSound.Play();
            isBeforeAnimSoundPlayed = true;
        }
    }
    public void PlayEffectSound(AudioClip effect)
    {
        EffectSound.clip = effect;

        if (EffectSound.clip != null)
            EffectSound.Play();
    }
    public void PlayQuestionSound()
    {
        if (QuestionSound.clip != null)
            QuestionSound.Play();
    }
    public void PlayQuestionSoundJustOnce()
    {
        if (!isQuestionSoundPlayed && QuestionSound.clip != null)
        {
            QuestionSound.Play();
            isQuestionSoundPlayed = true;
        }
    }
    public float GetBeforeSoundDuration()
    {
        if (BeforeAnimSound.clip == null)
            return defaultDuration;
        return BeforeAnimSound.clip.length;
    }

    public void StopAll()
    {
        BeforeAnimSound.Stop();
        QuestionSound.Stop();
    }
}
