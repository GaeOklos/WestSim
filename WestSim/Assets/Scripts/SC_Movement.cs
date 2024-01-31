using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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



    [Header("Jump")]
    public float jumpPower = 5f;
    [SerializeField] private bool _isWallJumping = false;
    [SerializeField] private GameObject _previousWall;


    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

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
        moveDirection = (forward * movementInput.x) + (right * movementInput.y);
        


        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
            // Debug.Log(moveDirection);
        }
        else if (Input.GetButton("Jump") && canMove && _isWallJumping)
        {
            _isWallJumping = false;
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);


        if (canMove)
        {
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
        if (_Octane_isUsed == true) {
            _OctaneCooldownTimer += Time.deltaTime;
            if (_OctaneCooldownTimer >= _OctaneDurationTimer) {
                _Octane_isUsed = false;
                _OctaneCooldownTimer = 1.0f;
            }
            _OctaneLoadValue = _OctaneCooldownTimer / _OctaneDurationTimer;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("Wall"))
        {
            if (other.gameObject != _previousWall)
            {
                _previousWall = other.gameObject;
                _isWallJumping = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (collision.gameObject == _previousWall)
            {
                _previousWall = null;
                _isWallJumping = false;
            }
        }
    }

    public void Bumper()
    {
        // moveDirection += Vector de la propulsion;

        characterController.Move(moveDirection * Time.deltaTime);
    }
}
