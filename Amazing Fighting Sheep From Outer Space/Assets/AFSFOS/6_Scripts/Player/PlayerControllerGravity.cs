using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerGravity : MonoBehaviour
{
    private Vector2 _moveInput;
    public Vector3 _move;
    private Vector3 _stickDirection;
    private Vector3 _stickTargetNormal;
    private Vector3 _stickTargetDirection;
    private Vector3 _surfaceNormal;
    private Vector3 _targetPosition;
    private Vector3 _lastNonZeroMove;

    private Animator _animator;
    private Rigidbody _rbPlayer;

    private float _baseGravity = -9.81f;
    private float _baseMoveForce = 2.0f;
    private float _baseJumpForce = 2.0f;
    private int _numberOfJump = 0;

    private float alarmAttack = -1;
    private float alarmStun = -1;

    private bool _canStick = false;
    private bool _isJumping = false;
    private bool _isAttacking = false;
    private bool _isStun = false;
    private bool _isGrounded = true;
    private bool _hasMoved = false;
    private bool _hasStick = false;
    private bool _isSoundAlreadyPlaying = false;
    private bool _hasAlreadyAttacked = false;

    private Ray _stickRay;
    private Ray _downRay;

    private int _layerToIgnore = 1 << 12;
    private int _layerMask;

    GravityBody _gravityBody;
    private float _massGoat;

    ParticleSystem particleInstance;

    [Header("Player Controller Settings")]
    [Space(5)]
    [Header("Controls")]
    [Space(2)]
    [Header("Moving")]
    [SerializeField] private float moveMultiplier = 1.0f;
    [SerializeField] private float moveResistance = 10.0f;
    [SerializeField] private float moveWhileJumpingMultiplier = 10.0f;
    [SerializeField] private float maxMoveSpeed = 20.0f;

    [Space(2)]
    [Header("Jumping")]
    [SerializeField] private float jumpMultiplier = 10.0f;
    [SerializeField] private int maxNumberOfJumps = 2;

    [Space(2)]
    [Header("Gravity")]
    [SerializeField] private float baseGravityMultiplier = 5.0f;
    [SerializeField] private float surfaceGravityMultiplier = 3.0f;

    [Space(2)]
    [Header("Sticking to Surfaces")]
    //[SerializeField] private float stickMultiplier = 5.0f;
    [SerializeField] private float maxDistanceToStick = 5.0f;

    [Space(2)]
    [Header("Attack")]
    [SerializeField] private float forceOfAttack = 1000.0f;
    [SerializeField] private float attackDistance = 1.0f;
    [SerializeField] private float timeOfAttack = 1.0f;
    [SerializeField] private float timeOfStun = 1.0f;
    [SerializeField] private GameObject startPointAttack;

    [Space(2)]
    [Header("Detection")]
    [SerializeField] private LayerMask WhatIsStickSurface;

    [Space(2)]
    [Header("Particles")]
    [SerializeField] private ParticleSystem detectSurfaceParticlePrefab;
    [SerializeField] private GameObject hitPointParticle;
    [SerializeField] private GameObject positionPointParticle;

    private ParticlesManager _particlesManager;
    private SoundManager _soundManager;
    private VibratorManager _vibratorManager;

    // Camera
    private GameObject cam;

    private LayerMask layerMaskPlayer;

    public Quaternion targetRotation;
    public Quaternion yRotationQuaternion;
    private Quaternion originalRotation;
    public float yRotation;


    //  ################################################################
    //  Awake
    //  ################################################################
    private void Awake()
    {
        _animator = transform.GetChild(0).GetComponent<Animator>();
        _rbPlayer = GetComponent<Rigidbody>();
        _rbPlayer.drag = moveResistance;
        _massGoat = _rbPlayer.mass;
        _gravityBody = GetComponent<GravityBody>();

        _layerMask = ~_layerToIgnore;
        _surfaceNormal = Vector3.up;

        _particlesManager = SuperManager.instance.particlesManager;
        _soundManager = SuperManager.instance.soundManager;
        _vibratorManager = SuperManager.instance.vibratorManager;

        layerMaskPlayer = LayerMask.GetMask("Player1", "Player2");

        cam = SuperManager.instance.saveManager.GetCamera();
        originalRotation = cam.transform.localRotation;
    }

    //  ################################################################
    //  Controls
    //  ################################################################
    public void Move(InputAction.CallbackContext context)
    {
        if(_isStun) return;

        _moveInput = context.ReadValue<Vector2>();
        _hasMoved = true;

        //Debug.Log("Input : " +_moveInput.ToString());
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!_isGrounded && _numberOfJump >= maxNumberOfJumps || _isStun ) return;

        _rbPlayer.AddForce(transform.up * _massGoat * _baseJumpForce * jumpMultiplier, ForceMode.Impulse);
        _numberOfJump++;
        _soundManager.SpawnJumpSound();
    }
    public void Attack(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if (_isStun || _isAttacking) return;

        _isAttacking = true;
        _particlesManager.SpawnAttackParticle(positionPointParticle.transform.position, this.transform);
        _soundManager.SpawnSlurpSound();
        alarmAttack = Time.time + timeOfAttack;
    }
    public void StickOrientation(InputAction.CallbackContext context)
    {
        _stickDirection = new Vector3(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y, 0.0f).normalized;
        _stickRay = new Ray(transform.position, _stickDirection * 50);
        //Debug.DrawRay(transform.position, _stickDirection, Color.red, 0.5f);

        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(_stickRay, out hit, maxDistanceToStick, _layerMask))
        {
            if (hit.transform.gameObject.layer == 14)
            {
                _canStick = true;
                _targetPosition = new Vector3(hit.point.x, hit.point.y, 0.0f);
                _stickTargetDirection = _targetPosition - transform.position;
                _stickTargetNormal = hit.normal.normalized;

                if (!(_surfaceNormal == hit.normal.normalized))
                {
                    //detectSurfaceParticlePrefab.gameObject.SetActive(true);
                    //hitPointParticle.gameObject.transform.position = _targetPosition;
                }
            }
            else
            {
                _canStick = false;
            }
        }
        else
        {
            //detectSurfaceParticlePrefab.gameObject.SetActive(false);
        }


    }
    public void Stick(InputAction.CallbackContext context)
    {
        // Debug.Log("Stick Hold " + context.duration + " " + context.interaction + " " + context.performed);
        if (context.performed)
        {
            Debug.Log("Stick Hold !!!");
        }

        if (context.started && _canStick && transform.up != _stickTargetNormal)
        {
            transform.position = _targetPosition;
            transform.up = _stickTargetNormal;
            _surfaceNormal = _stickTargetNormal;
            _hasStick = true;
        }
    }

    //  ################################################################
    //  Apply Controls
    //  ################################################################
    private void ApplyMovement()
    {
        Vector3 directionMove = new Vector3(_moveInput.x, _moveInput.y, 0.0f);
        _surfaceNormal = _gravityBody.GravityDirection;
        _move = Vector3.ProjectOnPlane(directionMove, _surfaceNormal);

        if (_rbPlayer.velocity.magnitude < maxMoveSpeed && !_isStun)
        {
            _rbPlayer.AddForce(_move * _massGoat * _baseMoveForce * moveMultiplier, ForceMode.Impulse);
        }

        if (_move.x + _move.y != 0)
        {
            _lastNonZeroMove = _move;
        }
        else
        {
            _hasMoved = false;
        }

        //Debug.DrawRay(transform.position, _move, Color.red, 0.1f);
    }
    private void ApplyGravity()
    {
        if (_isGrounded)
        {
            _rbPlayer.AddForce(_surfaceNormal * _baseGravity * baseGravityMultiplier, ForceMode.Acceleration);
        }
        else
        {
            _rbPlayer.AddForce((Vector3.up + _surfaceNormal * surfaceGravityMultiplier) * _baseGravity, ForceMode.Acceleration);
        }
    }
    private void ApplyRotation()
    {
        //if (_hasStick && !_hasMoved)
        //{
        //    _lastNonZeroMove = transform.right;
        //    _hasStick = false;
        //}
        //Quaternion targetRotation = Quaternion.LookRotation(Vector3.Cross(_lastNonZeroMove, _surfaceNormal), _surfaceNormal);
        //_rbPlayer.MoveRotation(targetRotation);

        //targetRotation = Quaternion.LookRotation(Vector3.Cross(_lastNonZeroMove, -_gravityBody.GravityDirection), -_gravityBody.GravityDirection);

        Vector3 newww = Vector3.Cross(_lastNonZeroMove, -_gravityBody.GravityDirection) - transform.position;
       // Quaternion joystickRotation = Quaternion.AngleAxis(horizontalInput * rotationSpeed * Time.fixedDeltaTime, transform.up);

        float currentYORotation = _rbPlayer.rotation.eulerAngles.y;

        if (newww.z > 0)
        {
            yRotation = 0.0f;
        }
        else
        {
            yRotation = 180.0f;
        }

        //_rbPlayer.MoveRotation(targetRotation);
    }
    private void ApplyAnimation()
    {
        if (!_isStun)
        {
            _animator.SetBool("isStun", false);

            if (_isJumping)
            {
                _animator.SetBool("isJumping", true);
            }
            else
            {
                _animator.SetBool("isJumping", false);
                _animator.SetFloat("BlendMove", Mathf.Abs(_move.x + _move.y));
            }

            if (_isAttacking)
            {
                _animator.SetBool("isAttacking", true);
            }
            else
            {
                _animator.SetBool("isAttacking", false);
            }
        }
        else
        {
            _animator.SetBool("isStun", true);
        }

    }
    private void ApplyAttack()
    {
        if (_isAttacking && !_hasAlreadyAttacked)
        {
            RayCastToAttack();
            _hasAlreadyAttacked = true;
        }

        if(alarmAttack < Time.time)
        {
            _hasAlreadyAttacked = false;
            _isAttacking = false;
        }
    }
    private void ApplyStun()
    {
        if (_isStun)
        {        
            if (alarmStun <= Time.time)
            {
                _isStun = false;
            }
        }
    }

    //  ################################################################
    //  Other behaviors
    //  ################################################################
    private void RayCastToAttack()
    {
        Ray attackRay = new Ray(startPointAttack.transform.position, startPointAttack.transform.right);
        RaycastHit other = new RaycastHit();
        //Debug.DrawRay(attackRay.origin, attackRay.direction * attackDistance, Color.red, 2f);

        if (Physics.Raycast(attackRay, out other, attackDistance, layerMaskPlayer))
        {
            if (other.rigidbody.gameObject.TryGetComponent(out PlayerControllerGravity playerControllerGravity) && other.rigidbody.gameObject.name != this.gameObject.name)
            {
                //Debug.Log("Attacker : " + this.gameObject.name + "\n" + "Attacked : " + other.rigidbody.gameObject.name);

                if (!playerControllerGravity._isStun)
                {
                    _vibratorManager.RumblePulse(0.25f, 0.75f, 0.2f);
                    _particlesManager.SpawnHitParticle(other.rigidbody.gameObject.transform.position);
                    _particlesManager.SpawnStunParticle(other.transform.GetComponent<PlayerControllerGravity>().positionPointParticle.transform.position, other.rigidbody.gameObject.transform);
                    _soundManager.SpawnHitSound();
                    _soundManager.SpawnSheepSound();
                                        
                    cam.GetComponent<CameraShake>().StartShake();

                    playerControllerGravity._isStun = true;
                    playerControllerGravity.alarmStun = Time.time + timeOfStun;

                    Vector3 _expulseDirection = (other.rigidbody.gameObject.transform.position - transform.position).normalized;
                    playerControllerGravity._rbPlayer.AddForce(_expulseDirection * forceOfAttack, ForceMode.Impulse);
                }
            }
            else
            {
                Debug.LogError("no PlayerControllerPhysic found!");
            }

        }
    }
    private void DetectGround()
    {
        _downRay = new Ray(transform.position, -transform.up);
        RaycastHit downHit;

        if (Physics.Raycast(_downRay, out downHit, 0.1f, _layerMask))
        {
            _isGrounded = true;
            _isJumping = false;
            //Debug.DrawRay(transform.position, -transform.up, Color.magenta, 0.05f);
        }
        else
        {
            _isGrounded = false;
            _isJumping = true;
        }
    }
    private void ResetZposition()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
    }

    //  ################################################################
    //  FixedUpdate for Physics
    //  ################################################################
    private void FixedUpdate()
    {
        ApplyMovement();
        //ApplyGravity();
        DetectGround();
        ApplyAnimation();
        ApplyRotation();
        ApplyAttack();
        ApplyStun();

        ResetZposition();
    }

    
}
