using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class SC_Movement : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float lookSpeed = 2f;
    [SerializeField] private float lookXLimit = 45f;

    [Header("Basic")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float gravity = 10f;

    private float curSpeedX;
    private float curSpeedY;
    
    [Header("MaxSprintOctane")]
    [SerializeField] private float _sprintOctane = 15f;
    [SerializeField] private bool _OctaneTimerOn = false;
    [SerializeField] private bool _Octane_isUsed = false;
    private float _OctaneTimeUsing = 0.0f;
    private float _OctaneCooldownTimer = 0.0f;
    [SerializeField] private float _OctaneDurationCooldown = 10.0f;
    [SerializeField] private float _OctaneDuration = 5.0f;
    private float _OctaneLoadValue = 1.0f;
    public Slider _sliderUICD;



    [Header("Jump")]
    public float jumpPower = 5f;
    [SerializeField] private bool _isWallJumping = false;
    [SerializeField] private GameObject _previousWall;

    private bool debugWall = false;


    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [Header("Bumper")]
    private float _bumpForce = 10.0f;
    [SerializeField] private bool _isBumped = false;
    private Vector3 _bumpVectorDirector;
    private Vector3 _bumpVectorSpeedCurrent;
    [SerializeField] private float _ForceYBumperInGround = 20f;


    [Header("Debug")]
    public bool canMove = true;
    // [SerializeField] private SphereCollider _sphereCollider;

    
    CharacterController characterController;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        FindWall();

        // OctaneCapacity
        if (Input.GetKeyDown(KeyCode.C)) {
            LaunchOctaneCapacity();
        }
        OctaneCapacityCooldown();
        OctaneCapacityTimeUse();


        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);


        float movementDirectionY = moveDirection.y;
        // Appel de fonction movement Walk or Sprint or Octane
        Vector2 movementInput = SprintOrWalk();

        DecreaseSpeedBumper();
        if (_isBumped == false) {
            moveDirection = (forward * movementInput.x) + (right * movementInput.y);
        }
        else {
            moveDirection = (forward * movementInput.x) + (right * movementInput.y) + _bumpVectorSpeedCurrent;
        }

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded) {
            moveDirection.y = jumpPower;
        }
        else if (Input.GetButton("Jump") && canMove && _isWallJumping) {
            _isWallJumping = false;
            moveDirection.y = jumpPower;
        }
        else {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded) {
            moveDirection.y -= gravity * Time.deltaTime;
        }

/////////////////////////////////////////////////////////////////////////////////////////////////
        characterController.Move(moveDirection * Time.deltaTime);
/////////////////////////////////////////////////////////////////////////////////////////////////
        
        if (characterController.isGrounded) {
            _isBumped = false;
        }

        if (canMove) {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        if (Input.anyKey == false){
            moveDirection.x = 0;
            moveDirection.z = 0;
        }
    }

    private void DecreaseSpeedBumper()
    {
        if (_isBumped == true) {
            _bumpVectorSpeedCurrent -= _bumpVectorSpeedCurrent * Time.deltaTime;
        }
    }

    private Vector2 SprintOrWalk()
    {
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        if (isRunning == true && _Octane_isUsed == true && canMove == true) {
            curSpeedX = _sprintOctane * Input.GetAxis("Vertical");
            curSpeedY = _sprintOctane * Input.GetAxis("Horizontal");
        }
        else if (isRunning == true && canMove == true){
            curSpeedX = runSpeed * Input.GetAxis("Vertical");
            curSpeedY = runSpeed * Input.GetAxis("Horizontal");
        }
        else if (isRunning == false && canMove == true) {
            curSpeedX = walkSpeed * Input.GetAxis("Vertical");
            curSpeedY = walkSpeed * Input.GetAxis("Horizontal");
        }
        return new Vector2(curSpeedX, curSpeedY);
    }
    private void LaunchOctaneCapacity()
    {
        if (_OctaneTimerOn == false)
        {
            _Octane_isUsed = true;
            _OctaneTimerOn = true;
            _OctaneCooldownTimer = 0.0f;
            _OctaneLoadValue = 0.0f;
        }
    }
    private void OctaneCapacityCooldown()
    {
        if (_sliderUICD != null)
            _sliderUICD.value = _OctaneLoadValue;
        else
            Debug.LogError("Error, slider pas int�gr�");

        if (_OctaneTimerOn == true)
        {
            _OctaneCooldownTimer += Time.deltaTime;
            _OctaneLoadValue = _OctaneCooldownTimer / _OctaneDurationCooldown;
            if (_OctaneCooldownTimer >= _OctaneDurationCooldown)
            {
                _OctaneTimerOn = false;
                _OctaneCooldownTimer = 0.0f;
                _OctaneLoadValue = 1f;
            }
        }
    }
    private void OctaneCapacityTimeUse()
    {
        if (_Octane_isUsed == true)
        {
            _OctaneTimeUsing += Time.deltaTime;
            if (_OctaneTimeUsing >= _OctaneDuration)
            {
                _Octane_isUsed = false;
                _OctaneTimeUsing = 0.0f;
            }
        }
    }

    public void FindWall()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.6f);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].gameObject.CompareTag("Wall")) {
                if (hitColliders[i].gameObject != _previousWall) {
                    _previousWall = hitColliders[i].gameObject;
                    _isWallJumping = true;
                    debugWall = true;
                }
            }
            i++;
        }
        if (debugWall == false) {
            _isWallJumping = false;
            _previousWall = null;
        }
        debugWall = false;
    }
    

    public void Bumper(Vector3 _vectorDirector)
    {
        _bumpVectorDirector = _vectorDirector;
        _bumpVectorSpeedCurrent = _bumpVectorDirector * _bumpForce;
        if (_bumpVectorSpeedCurrent.y <= 0) {
            _bumpVectorSpeedCurrent.y = 7;
        }
        else if (_bumpVectorSpeedCurrent.y < _ForceYBumperInGround) {
            _bumpVectorSpeedCurrent.y = _ForceYBumperInGround;
        }
        moveDirection += _bumpVectorSpeedCurrent;
        // Debug.Log(_bumpVectorSpeedCurrent);

        _isBumped = true;
        // bumpVector = ;
    }
}
