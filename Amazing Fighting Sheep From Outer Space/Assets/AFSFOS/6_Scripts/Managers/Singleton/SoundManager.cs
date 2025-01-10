using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public GameObject soundPrefab;

    public AudioClip music;
    public AudioClip hitSound;
    public AudioClip sheepSound;
    public AudioClip jumpSound;

    public AudioClip sphereSound;

    public AudioClip slurpSound;

    public void SpawnGameMusic01()
    {
        GameObject go = Instantiate(soundPrefab);
        go.GetComponent<AudioSource>().clip = music;
        go.GetComponent<AudioSource>().volume = 0.2f;
        go.GetComponent<AudioSource>().loop = true;
        go.GetComponent<AudioSource>().Play();
        go.AddComponent<PersistOnLoad>();
    }
    public void SpawnHitSound()
    {
        GameObject go = Instantiate(soundPrefab);
        go.GetComponent<AudioSource>().clip = hitSound;
        go.GetComponent<AudioSource>().volume = 0.7f;
        go.GetComponent<AudioSource>().Play();
        go.AddComponent<DestroyAfterSound>();
    }
    public void SpawnSheepSound()
    {
        GameObject go = Instantiate(soundPrefab);
        go.GetComponent<AudioSource>().clip = sheepSound;
        go.GetComponent<AudioSource>().volume = 0.5f;
        go.GetComponent<AudioSource>().Play();
        go.AddComponent<DestroyAfterSound>();
    }

    public void SpawnJumpSound()
    {
        GameObject go = Instantiate(soundPrefab);
        go.GetComponent<AudioSource>().clip = jumpSound;
        go.GetComponent<AudioSource>().volume = 0.2f;
        go.GetComponent<AudioSource>().Play();
        go.AddComponent<DestroyAfterSound>();
    }

    public void SpawnSphereSound()
    {
        GameObject go = Instantiate(soundPrefab);
        go.GetComponent<AudioSource>().clip = sphereSound;
        go.GetComponent<AudioSource>().volume = 0.5f;
        go.GetComponent<AudioSource>().Play();
        go.AddComponent<DestroyAfterSound>();
    }

    public void SpawnSlurpSound() 
    {
        GameObject go = Instantiate(soundPrefab);
        go.GetComponent<AudioSource>().clip = slurpSound;
        go.GetComponent<AudioSource>().volume = 0.3f;
        go.GetComponent<AudioSource>().Play();
        go.AddComponent<DestroyAfterSound>();
    }
}
