using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSpawner : RyoMonoBehaviour
{
    private static SoundSpawner instance;
    public static SoundSpawner Instance => instance;

    [Header("Audio Source")]
    private AudioSource _audioSource;

    protected override void Awake()
    {
        if (SoundSpawner.instance != null) return;

        SoundSpawner.instance = this;

        base.Awake();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();

        if (this._audioSource == null)
            this._audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(AudioClip clip)
    {
        this._audioSource?.PlayOneShot(clip);
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.L))
    //    {
    //        SoundSpawner.Instance.PlayAudio(SoundSpawner.Instance.PainAudio);
    //    }
    //}

}
