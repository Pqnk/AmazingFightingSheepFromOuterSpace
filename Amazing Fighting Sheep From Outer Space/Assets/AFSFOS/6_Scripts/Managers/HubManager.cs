using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;
using UnityEngine.Windows;

public class HubManager : MonoBehaviour
{
    public GameObject prefabCam;
    public GameObject spawnPoint;
    public Transform spawnPointCamera;

    private PlayerInputManager inputManager;

    [Header("UI")]
    public GameObject canvasHub_slider;
    public GameObject canvasHub_pressbutton;

    [Header("Script")]
    public TriggerScript triggerScript;

    [Header("Animation")]
    public Animator animator;

    private int _currentNumberPlayer = 0;

    private Slider _slider;
    private bool isStarted = false;
    private float elapsedTime = 0;
    private bool p1Ok = false;
    private bool playersFreezed = false;

    private void Awake()
    {
        inputManager = GetComponent<PlayerInputManager>();
        if (SuperManager.instance.saveManager.allPlayersObject.Count == 2)
        {
            GetComponent<PlayerInputManager>().enabled = false;
            _currentNumberPlayer = SuperManager.instance.saveManager.allPlayersObject.Count;

            foreach (var item in SuperManager.instance.saveManager.allPlayersObject)
            {
                item.GetComponent<Rigidbody>().isKinematic = false;
                item.transform.position = spawnPoint.transform.position;
                item.transform.GetChild(0).gameObject.SetActive(true);
            }

            SuperManager.instance.saveManager.RestartScore();
        }
    }

    private void Start()
    {
        SuperManager.instance.soundManager.SpawnGameMusic01();
        Cursor.visible = false;

        _slider = canvasHub_slider.transform.GetChild(0).GetComponent<Slider>();

    }

    private void OnEnable()
    {
        inputManager.onPlayerJoined += OnPlayerJoined;
    }

    private void OnDisable()
    {
        inputManager.onPlayerJoined -= OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        if (SuperManager.instance.saveManager.allPlayersObject.Count < 2)
        {
            if (SuperManager.instance.saveManager.GetCamera() == null)
            {
                GameObject cam = Instantiate(prefabCam, spawnPointCamera.position, spawnPointCamera.rotation);
                SuperManager.instance.saveManager.SetCamera(cam);
            }

            if (!p1Ok)
            {
                SuperManager.instance.saveManager.AddPlayer(playerNames[0], 0);
                playerInput.gameObject.name = playerNames[0];
                playerInput.gameObject.layer = 12;
                playerInput.gameObject.GetComponent<PlayerPhysic>().ChangeColorPlayer(Color.white);
                p1Ok = true;
            }
            else
            {
                SuperManager.instance.saveManager.AddPlayer(playerNames[1], 0);
                playerInput.gameObject.name = playerNames[1];
                playerInput.gameObject.layer = 13;
                playerInput.gameObject.GetComponent<PlayerPhysic>().ChangeColorPlayer(Color.yellow);
            }

            SuperManager.instance.saveManager.allPlayersObject.Add(playerInput.gameObject);
            SuperManager.instance.saveManager.GetCamera().gameObject.GetComponent<CameraManager>().playersTarget.Add(playerInput.gameObject.transform);


            playerInput.gameObject.transform.position = spawnPoint.transform.position;


            playerInput.gameObject.AddComponent<PersistOnLoad>();

            _currentNumberPlayer++;
        }
    }

    private List<string> playerNames = new List<string>
    {
        "Player 1", "Player 2", "Taylor", "Morgan", "Riley", "Casey", "Jamie", "Rabin", "Cameron", "Drew", "Player_1", "Player_2"
    };


    private void Update()
    {
        if (triggerScript.elapsedTime >= triggerScript.requiredTimeInZone)
        {
            if (!isStarted)
            {
                animator.Play("AnimStop");
                isStarted = true;
            }
            if (!playersFreezed)
            {
                foreach (var item in SuperManager.instance.saveManager.allPlayersObject)
                {
                    item.GetComponent<Rigidbody>().isKinematic = true;
                }
                playersFreezed = true;
            }

            elapsedTime += Time.deltaTime;

            if (elapsedTime >= 3)
            {
                SuperManager.instance.levelManager.LoadRandomScene();
            }
        }

        if (triggerScript.elapsedTime > 0)
        {
            canvasHub_slider.SetActive(true);
            _slider.maxValue = triggerScript.requiredTimeInZone;
            _slider.value = triggerScript.elapsedTime;

        }
        else
        {
            canvasHub_slider.SetActive(false);
            _slider.value = 0;
        }

        if (_currentNumberPlayer > 0)
        {
            canvasHub_pressbutton.SetActive(false);
        }
    }
}
