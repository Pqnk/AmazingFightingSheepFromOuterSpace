using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserArea : MonoBehaviour
{
    public GameLevelManager GameLevelManager;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Debris")
        {
            //Instantiate(SuperManager.instance.particlesManager., other.transform);
            Destroy(other.gameObject);
        }

        if (other.tag == "Player")
        {
            Debug.Log(other.gameObject.name);
            other.gameObject.transform.position = GameLevelManager.spawnPoints[0].transform.position;
        }
    }
}
