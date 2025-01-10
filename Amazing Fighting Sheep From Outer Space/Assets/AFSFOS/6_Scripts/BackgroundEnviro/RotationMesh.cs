using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationMesh : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up;
    public float rotationSpeed = 100f;

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);

    }
}
