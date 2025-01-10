using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysic : MonoBehaviour
{
    [Header("Player Settings")]
    public GameObject goat;
    public int idPlayer;
    public Color colorPlayer;
    public string namePlayer;
    public int scorePlayer;

    void Start()
    {
        foreach (SkinnedMeshRenderer renderer in goat.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            renderer.material.color = colorPlayer;
        }
    }


    public void ChangeColorPlayer(Color newColorPlayer)
    {
        colorPlayer = newColorPlayer;
    }
}
