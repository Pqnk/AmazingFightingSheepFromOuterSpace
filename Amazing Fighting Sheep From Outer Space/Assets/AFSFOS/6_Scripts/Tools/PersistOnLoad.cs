using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistOnLoad : MonoBehaviour
{

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
