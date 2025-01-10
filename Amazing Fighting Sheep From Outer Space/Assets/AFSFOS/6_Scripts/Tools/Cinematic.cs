using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class Cinematic : MonoBehaviour
{
    [Header("Screen GameObject")]
    public GameObject first;

    [Header("UI")]
    public TMP_Text nameMap;
    public TMP_Text nameP1;
    public TMP_Text scoreP1;
    public TMP_Text nameP2;
    public TMP_Text scoreP2;

    [Header("GameObject Camera")]
    public GameObject cam;
    public GameObject startPoint;
    public GameObject endPoint;
    public GameObject globalPoint;
    public float translateTime;
    public float translateTimeToPlayer;

    private Animator firstAnim;
    private SaveManager saveManager;
    [SerializeField] private TriggerScript triggerScript;

    // Start is called before the first frame update
    void Start()
    {
        firstAnim = first.GetComponent<Animator>();
        saveManager = SuperManager.instance.saveManager;

        nameMap.text = SceneManager.GetActiveScene().name;

        string nameText_P1 = SuperManager.instance.saveManager.allPlayersObject[0].ToString();
        nameP1.text = nameText_P1.Replace(" (UnityEngine.GameObject)", "");
        scoreP1.text = SuperManager.instance.saveManager.GetPlayer(nameP1.text).Score.ToString();

        if (SuperManager.instance.saveManager.allPlayersObject.Count >= 2)
        {
            string nameText_P2 = SuperManager.instance.saveManager.allPlayersObject[1].ToString();
            nameP2.text = nameText_P2.Replace(" (UnityEngine.GameObject)", "");
            scoreP2.text = SuperManager.instance.saveManager.GetPlayer(nameP2.text).Score.ToString();
        }

        foreach (GameObject player in GetComponent<GameLevelManager>().players)
        {
            player.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void Action_MoveCam()
    {
        // GameObject cam
        // GameObject startPoint => position de départ
        // GameObject endPoint => position d'arrivé

        StartCoroutine(MoveCameraOverTime(cam, startPoint.transform.position, endPoint.transform.position, translateTime));
    }

    public void Action_MoveToAnotherPlayer()
    {
        if (saveManager.GetOtherPlayerObject(triggerScript.playerData) == null)
        {
            Debug.Log("Arrive dans other : null");
        }
        
        GameObject otherPlayer = saveManager.GetOtherPlayerObject(triggerScript.playerData);
        
        StartCoroutine(MoveCameraOverTimeToPlayer(cam, startPoint.transform.position, otherPlayer.transform.position, translateTimeToPlayer, otherPlayer));
    }

    public void Action_NextPosCam()
    {
        cam.transform.position = startPoint.transform.position;
    }

    public void Action_MoveToAllPlayers()
    {
        StartCoroutine(MoveCameraOverTimeToAllPlayer(cam, startPoint.transform.position, globalPoint.transform.position, translateTime));
    }

    private IEnumerator MoveCameraOverTime(GameObject cam, Vector3 startPos, Vector3 endPos, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            cam.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);

            yield return null;
        }

        cam.transform.position = endPos;
    }

    private IEnumerator MoveCameraOverTimeToPlayer(GameObject cam, Vector3 startPos, Vector3 endPos, float duration, GameObject otherPlayer)
    {
        float elapsedTime = 0f;

        Vector3 newEndPos = new Vector3(endPos.x, endPos.y, -14.13f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            cam.transform.position = Vector3.Lerp(startPos, newEndPos, elapsedTime / duration);

            yield return null;
        }

        cam.transform.position = newEndPos;

        yield return new WaitForSeconds(2);

        Instantiate(SuperManager.instance.particlesManager.spawnPlayerParticle, otherPlayer.transform);

        yield return new WaitForSeconds(1);

        otherPlayer.transform.GetChild(0).gameObject.SetActive(false);

        Debug.Log("Finish MoveCameraOverTimeToPlayer");
    }

    private IEnumerator MoveCameraOverTimeToAllPlayer(GameObject cam, Vector3 startPos, Vector3 endPos, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            cam.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);

            yield return null;
        }

        cam.transform.position = endPos;

        yield return new WaitForSeconds(2);

        foreach (GameObject player in GetComponent<GameLevelManager>().players)
        {
            Instantiate(SuperManager.instance.particlesManager.spawnPlayerParticle, player.transform);
        }

        yield return new WaitForSeconds(1);

        foreach (GameObject player in GetComponent<GameLevelManager>().players)
        {
            player.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void Action_SpawnParticulePlayer()
    {
        foreach (GameObject player in GetComponent<GameLevelManager>().players)
        {
            Instantiate(SuperManager.instance.particlesManager.spawnPlayerParticle, player.transform);
        }
    }

    public void Action_SpawnPlayer()
    {
        foreach (GameObject player in GetComponent<GameLevelManager>().players)
        {
            player.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

}
