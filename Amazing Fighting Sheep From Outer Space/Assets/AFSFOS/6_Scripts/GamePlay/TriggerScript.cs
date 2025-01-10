using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerScript : MonoBehaviour
{
    [Header("Information Detection")]
    public Vector3 boxSize = new Vector3(5f, 3f, 7f);
    public LayerMask detectionLayerP1;
    public LayerMask detectionLayerP2;
    public bool isLevel = false;

    [Header("Timer")]
    public float requiredTimeInZone = 3f;
    public float elapsedTime = 0f;

    [Header("Anim")]
    public GameObject timelineDirectorStart;
    public GameObject timelineDirectorEnd;

    [Header("Number of Player To Start")]
    public int numberPlayerToStart = 2;

    [Header("Script")]
    public PlayerData playerData;
    public GameLevelManager gameLevelManager;

    private bool isFinished = false;


    void Update()
    {
        #region Dection Player Zone
        Vector3 boxCenter = transform.position;
        Collider[] hitCollidersP1 = Physics.OverlapBox(boxCenter, boxSize / 2, Quaternion.identity, detectionLayerP1);
        Collider[] hitCollidersP2 = Physics.OverlapBox(boxCenter, boxSize / 2, Quaternion.identity, detectionLayerP2);
        HashSet<GameObject> detectedPlayers = new HashSet<GameObject>();


        foreach (var collider in hitCollidersP1)
        {
            GameObject player = collider.transform.root.gameObject;

            if (((1 << collider.gameObject.layer) & detectionLayerP1) != 0)
            {
                detectedPlayers.Add(player);
            }
        }

        foreach (var collider in hitCollidersP2)
        {
            GameObject player = collider.transform.root.gameObject;

            if (((1 << collider.gameObject.layer) & detectionLayerP2) != 0)
            {
                detectedPlayers.Add(player);
            }
        }
        #endregion

        // Si c'est un niveau
        if (isLevel)
        {
            if (detectedPlayers.Count == 1 && !isFinished)
            {
                isFinished = true;

                //if (timelineDirectorStart != null)
                //{
                //    timelineDirectorStart.SetActive(false);
                //}

                foreach (GameObject player in detectedPlayers)
                {
                    playerData = SuperManager.instance.saveManager.GetPlayer(player.name);

                    if (playerData != null)
                    {
                        playerData.Score++;

                    }
                }

                foreach (var item in SuperManager.instance.saveManager.allPlayersObject)
                {
                    item.GetComponent<Rigidbody>().isKinematic = true;
                }

                if (timelineDirectorEnd != null)
                {
                    timelineDirectorEnd.SetActive(true);
                }
            }
        }
        else
        {
            if (detectedPlayers.Count >= numberPlayerToStart)
            {
                elapsedTime += Time.deltaTime;
            }
            else
            {
                elapsedTime = 0f;
            }
        }
    }


    public void Action_NextGame()
    {
        int bestScore = 0;

        foreach (var item in SuperManager.instance.saveManager.GetPlayersScores())
        {
            if (item >= bestScore)
            {
                bestScore = item;
            }
        }

        if (bestScore >= SuperManager.instance.saveManager.maxScore)
        {
            SuperManager.instance.levelManager.LoadScene(Scenes.FinishGame);
        }
        else
        {
            SuperManager.instance.levelManager.LoadRandomScene();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}
