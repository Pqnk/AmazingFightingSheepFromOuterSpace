using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    private string nameCurrentLevel;
    private static float GRAVITY_FORCE = 2800;
    private Rigidbody _rigidbody;
    private List<GravityArea> _gravityAreas;
    private PlayerControllerGravity _playerController;

    public Vector3 GravityDirection
    {
        get
        {
            if (nameCurrentLevel != SceneManager.GetActiveScene().name)
            {
                _gravityAreas.Clear();
                nameCurrentLevel = SceneManager.GetActiveScene().name;
            }

            if (_gravityAreas.Count == 0)
            {
                return Vector3.down;
            }
            else
            {
                _gravityAreas.Sort((area1, area2) => area1.Priority.CompareTo(area2.Priority));
                return _gravityAreas.Last().GetGravityDirection(this).normalized;
            }
        }
    }


    void Start()
    {
        _rigidbody = transform.GetComponent<Rigidbody>();
        _gravityAreas = new List<GravityArea>();

        nameCurrentLevel = SceneManager.GetActiveScene().name;

        _playerController = GetComponent<PlayerControllerGravity>();
    }

    void FixedUpdate()
    {
        _rigidbody.AddForce(GravityDirection * (GRAVITY_FORCE * Time.fixedDeltaTime), ForceMode.Acceleration);

        Quaternion upRotation = Quaternion.FromToRotation(transform.up, -GravityDirection);
        Quaternion newRotation = Quaternion.Slerp(_rigidbody.rotation, upRotation * _rigidbody.rotation, Time.fixedDeltaTime * 10f);

        _rigidbody.MoveRotation(newRotation);

        Vector3 currentRotation = _rigidbody.rotation.eulerAngles;

        if (_playerController.yRotation == 0.0f)
        {
            currentRotation.y = 0.0f;
        }
        else
        {
            currentRotation.y = 180.0f;
        }
        _rigidbody.MoveRotation(Quaternion.Euler(currentRotation));
        //_rigidbody.MoveRotation(newRotation*_playerController.yRotation);
    }

    public void AddGravityArea(GravityArea gravityArea)
    {
        _gravityAreas.Add(gravityArea);
    }

    public void RemoveGravityArea(GravityArea gravityArea)
    {
        _gravityAreas.Remove(gravityArea);
    }
}