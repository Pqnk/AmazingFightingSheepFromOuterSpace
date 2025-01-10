using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameLevelManager : MonoBehaviour
{
    [Header("Gestion Players")]
    public GameObject[] spawnPoints = new GameObject[2];
    public GameObject[] players = new GameObject[2];

    [Header("Gestion Cinematique")]
    public bool cinematicFinished = false;
    public GameObject timelineDirectorStart;
    public GameObject timelineDirectorAllPlayerEnd;

    [Header("Timer")]
    public int timerDuration = 5;
    public TMP_Text timerText;
    private float currentTime = 0.0f;
    private bool timeFinished = false;

    [Header("Obstacle Option")]
    [SerializeField] bool withObstacle = false;
    [SerializeField] float spawnInterval = 2.0f;

    [Header("Zone de spawn")]
    public GameObject zonesParent;


    // Start is called before the first frame update
    void Awake()
    {
        players = SuperManager.instance.saveManager.GetPlayers();
    }

    private void Start()
    {
        currentTime = timerDuration;

        foreach (var item in SuperManager.instance.saveManager.allPlayersObject)
        {
            item.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cinematicFinished)
        {

            if (!timeFinished)
            {
                currentTime -= Time.deltaTime;
                int roundedTime = Mathf.FloorToInt(currentTime);
                timerText.text = roundedTime.ToString();
            }
            else
            {
                timerText.text = "0";
            }


            if (currentTime <= 0 && !timeFinished)
            {
                timeFinished = true;
                currentTime = 0;
                Debug.Log("Le timer est terminé!");
                TimerFinished();
            }
        }
        else
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (i < spawnPoints.Length)
                {
                    players[i].transform.position = spawnPoints[i].transform.position;
                }
            }
        }
    }

    private void TimerFinished()
    {
        foreach (var item in SuperManager.instance.saveManager.allPlayersObject)
        {
            item.GetComponent<Rigidbody>().isKinematic = true;
        }

        if (timelineDirectorStart != null)
        {
            timelineDirectorStart.SetActive(false);
        }

        if (timelineDirectorAllPlayerEnd != null)
        {
            timelineDirectorAllPlayerEnd.SetActive(true);
        }
    }

    public void Action_StartDebris()
    {
        if (withObstacle)
        {
            InvokeRepeating("SpawnObject", 0f, spawnInterval);
        }
    }


    void SpawnObject()
    {
        GameObject prefabToSpawn = SuperManager.instance.debrisManager.debris[Random.Range(0, SuperManager.instance.debrisManager.debris.Count)];

        Vector3 spawnPosition;

        spawnPosition = zonesParent.transform.GetChild(Random.Range(0, zonesParent.transform.childCount)).gameObject.transform.position;
        Vector3 thisSpawnPosition = new Vector3(Random.Range(spawnPosition.x - 4, spawnPosition.x + 4), spawnPosition.y, spawnPosition.z);
        Instantiate(prefabToSpawn, thisSpawnPosition, Quaternion.identity);
    }



    public void Action_SetCinematicFinished(bool state)
    {
        cinematicFinished = state;
    }
}
