using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour
{
    public List<Transform> playersTarget;
    public Vector3 offset;
    public float smoothTime = 0.5f;

    [Tooltip("distance minimale de sécurité des deux côtés")]
    public float minDistance = 5f;

    private Vector3 velocity;
    private Camera cam;

    public Vector3 player1ToPlayer2;

    public float initialZposition;
    public float uneValeur = 10;
    public float maxFOV = 20;
    public float distanceThreshold = 15f; // Seuil pour l'espacement entre les joueurs sur l'axe X

    public float currentZPosition;

    public float xmultiplier = 0.1f;
    public float zcameraMax = -325.0f;

    private void Start()
    {
        cam = GetComponent<Camera>();
        initialZposition = transform.position.z;
        uneValeur = 10.0f;
    }

    private void LateUpdate()
    {
        if (playersTarget.Count == 0) return;

        Move();
        AdjustZoom();
    }

    void Move()
    {
        // Obtenir le point central des joueurs
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;

        if (playersTarget.Count == 1)
        {
            Transform singlePlayer = playersTarget[0].transform;

            Vector3 targetPosition = new Vector3(singlePlayer.position.x, singlePlayer.position.y, singlePlayer.position.z + offset.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 20, Time.deltaTime);
        }
        else if (playersTarget.Count > 1)
        {
            DistanceYBetweenPlayer();

            if (Mathf.Abs(player1ToPlayer2.y) > uneValeur)
            {
                offset.z = -75 * (Mathf.Abs(player1ToPlayer2.y) * xmultiplier);
            }
            else
            {
                offset.z = Mathf.Lerp(offset.z, -75, Time.deltaTime);
            }

            offset.z = Mathf.Clamp(offset.z, zcameraMax, -75);

            if (offset.z <= zcameraMax)
            {
                cam.fieldOfView = Mathf.Min(cam.fieldOfView, 13);
            }

            transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
        }

        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 0, maxFOV);
    }

    void AdjustZoom()
    {
        // Calculer la plus grande distance entre les joueurs sur l'axe X ou Z
        float greatestDistance = GetGreatestDistance();
        greatestDistance = Mathf.Max(greatestDistance, minDistance);

        
        if (greatestDistance > distanceThreshold)
        {
            maxFOV = 25;
        }
        else
        {
            maxFOV = 20;
        }

        // Ajuster le field of view (FOV) en fonction de la distance, sans dépasser le maxFOV défini
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, greatestDistance, Time.deltaTime);
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 0, maxFOV); 
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(playersTarget[0].position, Vector3.zero);
        for (int i = 0; i < playersTarget.Count; i++)
            bounds.Encapsulate(playersTarget[i].position);

        return Mathf.Max(bounds.size.x, bounds.size.z);
    }

    Vector3 GetCenterPoint()
    {
        if (playersTarget.Count == 1)
            return playersTarget[0].position;

        var bounds = new Bounds(playersTarget[0].position, Vector3.zero);

        for (int i = 0; i < playersTarget.Count; i++)
            bounds.Encapsulate(playersTarget[i].position);

        return bounds.center;
    }

    private void DistanceYBetweenPlayer()
    {
        player1ToPlayer2 = playersTarget[0].gameObject.transform.position - playersTarget[1].gameObject.transform.position;
    }
}
