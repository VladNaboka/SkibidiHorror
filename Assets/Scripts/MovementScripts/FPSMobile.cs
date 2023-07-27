using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMobile : MonoBehaviour
{
    public bool CanMove { get; private set; } = true;
    private bool _isSprinting => _canSprint && _sprintButton;
    private bool _shouldJump => _characterController.isGrounded;

    [Header("Functional Options")]
    [SerializeField] private bool _canSprint = true;
    [SerializeField] private bool _useStamina = true;
    [SerializeField] private bool _canJump = true;
    [SerializeField] private bool _canUseHeadBob = true;
    [SerializeField] private bool _useFootsteps = true;

    [Header("Controls")]
    [SerializeField] private bool _sprintButton;

    [Header("Joystick Controls")]
    [SerializeField] private FixedJoystick _joystick;
    private float _horizontalMovement;
    private float _verticalMovement;

    [Header("Movement Parametrs")]
    [SerializeField] private float _movementSpeed = 3.0f;
    [SerializeField] private float _sprintSpeed = 6.0f;

    [Header("Jumping Parametrs")]
    [SerializeField] private float _jumpForce = 8.0f;
    [SerializeField] private float _gravity = 30.0f;

    [Header("Headbob Parametrs")]
    [SerializeField] private float _walkBobSpeed = 14f;
    [SerializeField] private float _walkBobAmount = 0.05f;
    [SerializeField] private float _sprintBobSpeed = 18f;
    [SerializeField] private float _sprintBobAmount = 0.11f;
    private float _defaultYPos = 0;
    private float _timer;

    [Header("Stamina Parameters")]
    [SerializeField] private float _maxStamina = 100.0f;
    [SerializeField] private float _staminaUseMultiplier = 5.0f;
    [SerializeField] private float _timeBeforeRegen = 3.0f;
    [SerializeField] private float _staminaValueIncrement = 2.0f;
    [SerializeField] private float _staminaTimeIncrement = 0.1f;
    private float _currentStamina;
    private Coroutine _regenStamina;
    public static Action<float> OnStaminaChange;

    [Header("Camera Parametrs")]
    [SerializeField] private Transform _playerCamera;
    [SerializeField] private float _cameraSensitivity;
    private int _rightFingerID;
    private float _halfScreenWidth;
    private Vector2 _lookInput;
    private float _cameraPitch;

    [Header("Footsteps Parametrs")]
    [SerializeField] private float _baseStepSpeed = 0.5f;
    [SerializeField] private float _sprintStepMultipler = 0.6f;
    [SerializeField] private AudioSource _footstepAudioSource = default;
    [SerializeField] private AudioClip[] _stepClips = default;
    private float _footstepTimer = 0f;
    private float _currentOffset => _isSprinting ? _baseStepSpeed * _sprintStepMultipler : _baseStepSpeed;


    private CharacterController _characterController;

    private Vector3 _moveDirection;
    private Vector2 _currentInput;

    private float _rotationX = 0;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        _rightFingerID = -1;
        _halfScreenWidth = Screen.width / 2;
        _currentStamina = _maxStamina;
    }
    private void Update()
    {
        if (CanMove)
        {
            HandleMovementInput();
            //HandleMouseLook();

            //if (_canJump)
            //    HandleJump();
            GetTouchInput();
            if(_rightFingerID != 1)
            {
                LookAround();
            }
            if (_canUseHeadBob)
                HandleHeadBob();

            if (_useFootsteps)
                HandleFootsteps();

            ApplyFinalMovements();
        }

        if (_useStamina)
        {
            HandleStamina();
        }
    }

    private void HandleMovementInput()
    {
        _horizontalMovement = Mathf.Lerp(_horizontalMovement, _joystick.Horizontal, Time.deltaTime * 5);
        _verticalMovement = Mathf.Lerp(_verticalMovement, _joystick.Vertical, Time.deltaTime * 5);

        _currentInput = new Vector2((_isSprinting ? _sprintSpeed : _movementSpeed) * _verticalMovement,
            (_isSprinting ? _sprintSpeed : _movementSpeed) * _horizontalMovement);

        float moveDirectionY = _moveDirection.y;
        _moveDirection = (transform.TransformDirection(Vector3.forward) * _currentInput.x)
            + (transform.TransformDirection(Vector3.right) * _currentInput.y);
        _moveDirection.y = moveDirectionY;
    }

    void GetTouchInput()
    {
        // Iterate through all the detected touches
        for (int i = 0; i < Input.touchCount; i++)
        {

            Touch t = Input.GetTouch(i);

            // Check each touch's phase
            switch (t.phase)
            {
                case TouchPhase.Began:
                    if (t.position.x > _halfScreenWidth && _rightFingerID == -1)
                    {
                        // Start tracking the rightfinger if it was not previously being tracked
                        _rightFingerID = t.fingerId;
                    }

                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (t.fingerId == _rightFingerID)
                    {
                        // Stop tracking the right finger
                        _rightFingerID = -1;
                        Debug.Log("Stopped tracking right finger");
                    }

                    break;
                case TouchPhase.Moved:

                    // Get input for looking around
                    if (t.fingerId == _rightFingerID)
                    {
                        _lookInput = t.deltaPosition * _cameraSensitivity * Time.deltaTime;
                    }
                    break;
                case TouchPhase.Stationary:
                    // Set the look input to zero if the finger is still
                    if (t.fingerId == _rightFingerID)
                    {
                        _lookInput = Vector2.zero;
                    }
                    break;
            }
        }
    }
    private void LookAround()
    {
        _cameraPitch = Mathf.Clamp(_cameraPitch - _lookInput.y, -80f, 80f);
        _playerCamera.localRotation = Quaternion.Euler(_cameraPitch, 0, 0);

        transform.Rotate(transform.up, _lookInput.x);
    }
    //private void HandleMouseLook()
    //{
    //    _rotationX -= Input.GetAxis("Mouse Y") * _cameraSpeedY;
    //    _rotationX = Mathf.Clamp(_rotationX, -_upCameraLimit, _downCameraLimit);
    //    _playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
    //    transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * _cameraSpeedX, 0);
    //}
    private void ApplyFinalMovements()
    {
        if (!_characterController.isGrounded)
        {
            _moveDirection.y -= _gravity * Time.deltaTime;
        }

        _characterController.Move(_moveDirection * Time.deltaTime);
    }
    private void HandleJump()
    {
        if (_shouldJump)
        {
            _moveDirection.y = _jumpForce;
        }
    }
    private void HandleHeadBob()
    {
        if (!_characterController.isGrounded) return;

        if (Mathf.Abs(_moveDirection.x) > 0.1f || Mathf.Abs(_moveDirection.z) > 0.1f)
        {
            _timer += Time.deltaTime * (_isSprinting ? _sprintBobSpeed : _walkBobSpeed);
            _playerCamera.transform.localPosition = new Vector3(
                _playerCamera.transform.localPosition.x,
                _defaultYPos + Mathf.Sin(_timer) * (_isSprinting ? _sprintBobAmount : _walkBobAmount),
                _playerCamera.transform.localPosition.z);
        }
    }
    private void HandleStamina()
    {
        if (_isSprinting && _currentInput != Vector2.zero)
        {
            if (_regenStamina != null)
            {
                StopCoroutine(_regenStamina);
                _regenStamina = null;
            }
            _currentStamina -= _staminaUseMultiplier * Time.deltaTime;

            if (_currentStamina < 0)
            {
                _currentStamina = 0;
            }

            OnStaminaChange?.Invoke(_currentStamina);

            if (_currentStamina <= 0)
            {
                _canSprint = false;
            }
        }

        if (!_isSprinting && _currentStamina < _maxStamina && _regenStamina == null)
        {
            _regenStamina = StartCoroutine(RegenerateStamina());
        }
    }
    private IEnumerator RegenerateStamina()
    {
        yield return new WaitForSeconds(_timeBeforeRegen);
        WaitForSeconds timeToWait = new WaitForSeconds(_staminaTimeIncrement);
        while (_currentStamina < _maxStamina)
        {
            if (_currentStamina > 0)
            {
                _canSprint = true;
            }
            if (_currentStamina > _maxStamina)
            {
                _currentStamina = _maxStamina;
            }
            _currentStamina += _staminaValueIncrement;

            OnStaminaChange?.Invoke(_currentStamina);

            yield return timeToWait;

            _regenStamina = null;
        }
    }
    private void HandleFootsteps()
    {
        if (!_characterController.isGrounded) return;
        if (_currentInput == Vector2.zero) return;

        _footstepTimer -= Time.deltaTime;

        if (_footstepTimer <= 0)
        {
            _footstepAudioSource.PlayOneShot(_stepClips[UnityEngine.Random.Range(0, _stepClips.Length - 1)]);
            _footstepTimer = _currentOffset;
        }
    }
    public void JumpButton()
    {
        HandleJump();
    }
    public void SprintButton(bool _isPressed)
    {
        _sprintButton = _isPressed;
    }
}
