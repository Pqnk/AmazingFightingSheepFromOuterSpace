using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingColor : MonoBehaviour
{
    public Material mat1;
    public Material mat2;

    public GameObject objectToChangeMat;

    private SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        objectToChangeMat.gameObject.GetComponent<MeshRenderer>().material = mat1;

        soundManager = SuperManager.instance.soundManager;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 12 || other.gameObject.layer == 13)
        {   
            objectToChangeMat.gameObject.GetComponent<MeshRenderer>().material = mat2;
            soundManager.SpawnSphereSound();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 12 || other.gameObject.layer == 13)
        {
            objectToChangeMat.gameObject.GetComponent<MeshRenderer>().material = mat2;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 12 || other.gameObject.layer == 13)
        {
            objectToChangeMat.gameObject.GetComponent<MeshRenderer>().material = mat1;
        }
    }
}
