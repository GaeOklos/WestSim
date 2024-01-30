using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(PlayerInput))]
	public class FirstPersonController : MonoBehaviour
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 4.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 6.0f;
		[Tooltip("Rotation speed of the character")]
		public float RotationSpeed = 1.0f;
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.1f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.5f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 90.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -90.0f;

		// cinemachine
		private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

		// Walljump
		[SerializeField] private bool _isWallJumping = false;

		[SerializeField] private int _wallJumpCount = 0;
		[SerializeField] private GameObject _prefabBumperCpt;

// Capacity
		[SerializeField] private bool _capacity_isUsed = false;
		private float _capacityCooldownTimer = 0.0f;
		[SerializeField] private float _capacityDurationTimer = 5.0f;
		public float _capacityLoadValue = 0.0f;
		[SerializeField] Slider _capacitySlider;
		[SerializeField] LayerMask bumpLayer;

		public float _bumpSpeed = 0f;

// Weapon
		[SerializeField] private GameObject _prefabWeapon;
		[SerializeField] private int _damageWeapon = 10;
		[SerializeField] private float _rangeWeapon = 100.0f;
		[SerializeField] private ParticleSystem _muzzleFlash;
		[SerializeField] private GameObject _impactEffectEnemyWeapon;
		[SerializeField] private GameObject _impactEffectWallWeapon;

		// Fist
		[SerializeField] private GameObject _spawnCollider;
		[SerializeField] private int _damageFist = 10;
		[SerializeField] private float _rangeFist = 1.0f;
		[SerializeField] private ParticleSystem _muzzleFlashFist;
		[SerializeField] private GameObject _impactEffectEnemyFist;
		[SerializeField] private GameObject _impactEffectWallFist;

		[SerializeField] private bool _debugMode = false; 
		
// 

	
		private PlayerInput _playerInput;
		private CharacterController _controller;
		private StarterAssetsInputs _input;
		private GameObject _mainCamera;

		private const float _threshold = 0.01f;

		private bool IsCurrentDeviceMouse
		{
			get
			{
				return _playerInput.currentControlScheme == "KeyboardMouse";
			}
		}

		private void Awake()
		{
			// get a reference to our main camera
			if (_mainCamera == null)
			{
				_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}
		}

		private void Start()
		{
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<StarterAssetsInputs>();
			_playerInput = GetComponent<PlayerInput>();

			// reset our timeouts on start
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;
		}

		private void Update()
		{
			JumpAndGravity();
			GroundedCheck();
			Move();
			CapacityCooldown();
			CapacityAbility();
			Shoot();
			FistHit();
		}

		private void LateUpdate()
		{
			CameraRotation();
		}

		private void Shoot()
		{
			if (Input.GetKeyDown(KeyCode.Mouse0)) {
				// _muzzleFlash.Play();
				RaycastHit hit;
				if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out hit, _rangeWeapon)) {
					if(hit.collider.gameObject.GetComponent<SC_Enemy>() != null) {
                        hit.collider.gameObject.GetComponent<SC_Enemy>().TakeDamage(_damageWeapon);
                        Debug.DrawLine(_mainCamera.transform.position, hit.point, Color.red, 2.0f, true);
						// Debug.Log(hit.transform.name);
						// Instantiate(_impactEffectEnemyWeapon, hit.point, Quaternion.LookRotation(hit.normal));
					}
				}
			}
		}

		private void FistHit()
		{
			if (Input.GetKeyDown(KeyCode.Mouse1)) {
				Collider[] hitColliders = Physics.OverlapSphere(_spawnCollider.transform.position, _rangeFist);
				int i = 0;
				if (_debugMode == true) {
					GameObject _sphereTest = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), _spawnCollider.transform.position, Quaternion.identity);
					_sphereTest.transform.localScale = _sphereTest.transform.localScale * 2;
					Destroy(_sphereTest, 1);
				}
				while (i < hitColliders.Length) {
					if (hitColliders[i].gameObject.GetComponent<SC_Enemy>() != null) {
						hitColliders[i].gameObject.GetComponent<SC_Enemy>().TakeDamage(_damageFist);
						// Debug.Log(hitColliders[i].gameObject.name);
						// Instantiate(_impactEffectEnemyFist, hitColliders[i].gameObject.transform.position, Quaternion.LookRotation(hitColliders[i].gameObject.transform.position));
					}
					i++;
				}
			}
		}

		private void CapacityAbility()
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				RaycastHit hit;
				if (_capacity_isUsed == false)
				{
					if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out hit, _rangeWeapon, bumpLayer))
					{
                        Debug.Log(hit.transform.name);
                        _capacity_isUsed = true;
                        _capacityCooldownTimer = 0.0f;
                        _capacityLoadValue = 0.0f;
                        Instantiate(_prefabBumperCpt, hit.point, Quaternion.Euler(hit.normal));
                    }
				}
			}

			if (_capacityLoadValue == 0)
			{
				_capacitySlider.value = 0;
			} else
			{
                _capacitySlider.value = _capacityLoadValue;
            }
        }

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
		}

		private void CameraRotation()
		{
			// if there is an input
			if (_input.look.sqrMagnitude >= _threshold) {
				//Don't multiply mouse input by Time.deltaTime
				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
				
				_cinemachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
				_rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;

				// clamp our pitch rotation
				_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

				// Update Cinemachine camera target pitch
				CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

				// rotate the player left and right
				transform.Rotate(Vector3.up * _rotationVelocity);
			}
		}

		private void Move()
		{
			// set target speed based on move speed, sprint speed and 
			float targetSpeed;

			if (_input.sprint)
				targetSpeed = SprintSpeed;
			else
				targetSpeed = MoveSpeed;

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (_input.move == Vector2.zero)
				targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset) {
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
				_speed = targetSpeed;

			// normalise input direction
			Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (_input.move != Vector2.zero)
			{
				// move
				inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
			}

			// move the player
			_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
		}

		private void JumpAndGravity()
		{
			if (Grounded) {
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
					_verticalVelocity = -2f;

				// Jump
				if (_input.jump) {
					if (_jumpTimeoutDelta <= 0.0f) {
						// the square root of H * -2 * G = how much velocity needed to reach desired height
						_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
						_wallJumpCount = 1;
					}
				}
				else {
					_wallJumpCount = 0;
					_isWallJumping = false;
				}
				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f) {
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else {
				if (_isWallJumping == true && _input.jump && _wallJumpCount == 1) {
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
					_isWallJumping = false;
					_wallJumpCount = 2;
				}
				else {
					// reset the jump timeout timer
					_jumpTimeoutDelta = JumpTimeout;

					// fall timeout
					if (_fallTimeoutDelta >= 0.0f) {
						_fallTimeoutDelta -= Time.deltaTime;
					}

					// if we are not grounded, do not jump
					_input.jump = false;
				}
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity) 
				_verticalVelocity += Gravity * Time.deltaTime;
		}


		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded)
				Gizmos.color = transparentGreen;
			else
				Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}

		private void OnControllerColliderHit(ControllerColliderHit hit)
		{
			if (hit.gameObject.CompareTag("Wall") && !Grounded)
			{
				_isWallJumping = true;
			}
		}

        private void OnTriggerEnter(Collider other)
        {
			/*
            if (other.CompareTag("Bumper"))
			{
				Quaternion rotation = other.transform.rotation;
				Vector3 bumpForce = rotation.eulerAngles;
				_controller.Move(bumpForce * (_speed * -_bumpSpeed * Time.deltaTime));
				Debug.Log("bumper trigger");
			}
			*/
        }

        private void CapacityCooldown()
		{
			if (_capacity_isUsed == true) {
				_capacityCooldownTimer += Time.deltaTime;
				if (_capacityCooldownTimer >= _capacityDurationTimer) {
					_capacity_isUsed = false;
					_capacityCooldownTimer = 0.0f;
				}
				_capacityLoadValue = _capacityCooldownTimer / _capacityDurationTimer;
			}
		}
	}
}