using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSound : MonoBehaviour
{
    private float timeSound;

    void Start()
    {
        GetComponent<AudioSource>().Play();
        timeSound = GetComponent<AudioSource>().clip.length;
        StartCoroutine(DestroyAfterSoundFinished());
    }

    IEnumerator DestroyAfterSoundFinished()
    {
        yield return new WaitForSeconds(timeSound + 1);

        Destroy(this.gameObject);
    }
}
