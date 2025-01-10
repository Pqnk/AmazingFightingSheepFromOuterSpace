using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperManager : MonoBehaviour
{
    public static SuperManager instance { get; private set; }

    public SaveManager saveManager { get; private set; }
    public LangageManager langageManager { get; private set; }
    public LevelManager levelManager { get; private set; }
    public MalusManager malusManager { get; private set; }
    public DebrisManager debrisManager { get; private set; }
    public SoundManager soundManager{ get; private set; }
    public ParticlesManager particlesManager { get; private set; }
    public VibratorManager vibratorManager { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;

        saveManager = GetComponent<SaveManager>();
        langageManager = GetComponent<LangageManager>();
        levelManager = GetComponent<LevelManager>();
        malusManager = GetComponent<MalusManager>();
        debrisManager = GetComponent<DebrisManager>();
        soundManager = GetComponent<SoundManager>();
        particlesManager = GetComponent<ParticlesManager>();
        vibratorManager = GetComponent<VibratorManager>();
    }
}
