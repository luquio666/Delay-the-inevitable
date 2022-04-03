using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEvents : MonoBehaviour
{
    public string AudioName;
    public bool Loop;
    public float StartDelay;
    [Space]
    public bool PlayOnStart;
    public bool PlayOnEnable;
    [Space]
    public bool StopOnStart;
    public bool StopOnEnable;

    private Coroutine _playAudioDelayedCo;

    private void Awake()
    {
        SetName();
    }
    private void Start()
    {
        if (StopOnStart)
            StopAudio();
        if (PlayOnStart)
            PlayAudio();
    }

    private void OnEnable()
    {
        if (StopOnEnable)
            StopAudio();
        if (PlayOnEnable)
            PlayAudio();
    }

    [ContextMenu("SetName")]
    public void SetName()
    {
        this.name = $"SoundEvent";
        if (PlayOnStart)
            this.name += $" (PlayOnStart)";
        if (PlayOnEnable)
            this.name += $" (PlayOnEnable)";
        if (StopOnStart)
            this.name += $" (StopOnStart)";
        if (StopOnEnable)
            this.name += $" (StopOnEnable)";
        this.name += $" :: \"{AudioName}\"";
        if (Loop)
            this.name += $", loops";
        if (StartDelay > 0)
            this.name += $", delay: {StartDelay}";
    }

    private void PlayAudio()
    {
        if (_playAudioDelayedCo != null)
            StopCoroutine(_playAudioDelayedCo);
        _playAudioDelayedCo = StartCoroutine(PlayAudioDelayedCo(StartDelay));
    }
    private void StopAudio()
    { 
        GameEvents.StopSound(AudioName); 
    }

    private IEnumerator PlayAudioDelayedCo(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameEvents.PlaySound(AudioName, Loop);
    }

}
