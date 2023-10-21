using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAudioController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Sound[] sounds;

    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.spatialBlend = 1; // 3D spatial blend
        }
    }

    public void PlayLocalSFX(string audioName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == audioName);
        if (s == null)
            return;
        s.source.Play();
    }

    public void PlayClipAtPosition(string audioName, Transform pos)
    {
        Sound s = Array.Find(sounds, sound => sound.name == audioName);

        if (s == null)
            return;

        AudioSource.PlayClipAtPoint(s.source.clip, pos.position);
    }

}
