using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class SC_Movement : MonoBehaviour
{
    [Header("Camera")]
    public Camera playerCamera;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    [Header("Basic")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float gravity = 10f;

    private float curSpeedX;
    private float curSpeedY;
    
    [Header("MaxSprintOctane")]
    public float _sprintOctane = 15f;
    [SerializeField] private bool _Octane_isUsed = false;
    private float _OctaneCooldownTimer = 0.0f;
    [SerializeField] private float _OctaneDurationTimer = 5.0f;
    public float _OctaneLoadValue = 0.0f;
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
        if (_Octane_isUsed == false)
        {
            _Octane_isUsed = true;
            _OctaneCooldownTimer = 0.0f;
            _OctaneLoadValue = 0.0f;
        }
    }
    private void OctaneCapacityCooldown()
    {
        if (_sliderUICD != null) {
            _sliderUICD.value = _OctaneLoadValue;
        }
        else
            Debug.Log("Error, slider pas int�gr�");

        if (_Octane_isUsed == true) {
            _OctaneCooldownTimer += Time.deltaTime;
            if (_OctaneCooldownTimer >= _OctaneDurationTimer) {
                _Octane_isUsed = false;
                _OctaneCooldownTimer = 1.0f;
            }
            _OctaneLoadValue = _OctaneCooldownTimer / _OctaneDurationTimer;
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
        Debug.Log(_bumpVectorSpeedCurrent);

        _isBumped = true;
        // bumpVector = ;
    }
}
