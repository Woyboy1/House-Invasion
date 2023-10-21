using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAudioController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Sound[] groanSFX;

    [Header("Settings")]
    [SerializeField] private bool autoGroan = true;
    [SerializeField] private float groanInterval = 2.5f;

    public Sound[] GroanSFX => groanSFX;

    private void Awake()
    {
        foreach (Sound s in groanSFX)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.spatialBlend = 1; // 3D spatial blend
        }
    }

    private void Start()
    {
        if (autoGroan) StartCoroutine(GroanSFXInterval(groanInterval));
    }

    public void PlayLocalSFX(string audioName, Sound[] list)
    {
        Sound s = Array.Find(list, sound => sound.name == audioName);
        if (s == null)
            return;
        s.source.Play();
    }

    IEnumerator GroanSFXInterval(float groanInterval)
    {
        while (true)
        {
            int randGroan = UnityEngine.Random.Range(0, groanSFX.Length);

            PlayLocalSFX(groanSFX[randGroan].name, groanSFX);
            yield return new WaitForSeconds(groanInterval);
        }
    }
}
