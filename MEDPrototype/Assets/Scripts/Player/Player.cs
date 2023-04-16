using UnityEngine;
using System.Collections;

//Credit: Sebastian Lague
//Edits by Chris W

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour {

	public static Player Instance;

	[Header("Jump modifiers")]
	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float earlyJumpEndGravMod = 3;
	public float fallClamp = -30f;
	public float timeToJumpApex = .4f;

	[Header("Horizontal Movement modifiers")]
	public float accelerationTimeAirborne = .2f;
	public float accelerationTimeGrounded = .1f;
	public float moveSpeed = 6;
	[HideInInspector] public bool receivingInput = true;
	[HideInInspector] public bool frozen = false;

	[Header("Wall jumping")]
	[Tooltip("Velocity if jumping when input towards wall")]
	public Vector2 wallJumpClimb;
	[Tooltip("Velocity if jumping when input away from wall")]
	public Vector2 wallJumpOff;
	[Tooltip("Velocity if jumping when input is none")]
	public Vector2 wallLeap;

	[Header("Wall sliding")]
	public float wallSlideSpeedMax = 3;
	public float wallStickTime = .25f;
	float timeTillUnstuckFromWall = 0;

	float gravity;
	float maxGravity;
	float minGravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;

	Controller2D controller;
	public new AudioManager audio;

	Vector2 directionalInput;
	bool wallSliding;
	int wallDirX;

	private SpriteRenderer mySpriteRenderer;

	public static Player instance;

	public void HaltVelocity()
    {
		velocity.x = 0;
		velocity.y = 0;
    }

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
		{
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
		mySpriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Start()
	{
		if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

		// Dont destory Player code ^^^^^

		controller = GetComponent<Controller2D>();
		audio = FindObjectOfType<AudioManager>();

		gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		minGravity = gravity / 1.25f;
		maxGravity = gravity;
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
	}

	void Update()
	{
		if (!frozen)
		{
			CalculateVelocity();
			HandleWallSliding();
			CalculateJumpApex();
			CalculateJump();

			bool prevGrounded = controller.collisions.below;
			controller.Move(velocity * Time.deltaTime, directionalInput);
			if (!controller.collisions.below && prevGrounded) lastGroundTime = Time.time;
			else if (controller.collisions.below && !prevGrounded)
            {
				coyoteUsable = true;
				//landing = true;
            }
		}

		if (velocity.x < 0) // flip sprite
			mySpriteRenderer.flipX = true;
			//transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		else if (velocity.x > 0)
			mySpriteRenderer.flipX = false;

		if (controller.collisions.above || controller.collisions.below)
		{
			if (controller.collisions.slidingDownMaxSlope)
			{
				velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
			}
			else
			{
				velocity.y = 0;
			}
		}
	}

	public void SetDirectionalInput(Vector2 input)
	{
		directionalInput = input;
	}

	private bool endedJumpEarly;
	private bool jumpPressed = false;
	private float lastGroundTime;
	private float lastJumpTime;
	private bool coyoteUsable;

	[Header("Jump forgiveness effects")]
	[SerializeField] private float jumpBuffer = 0.1f;
	[SerializeField] private float coyoteBuffer = 0.1f;

	public void OnJumpInputDown()
	{
		lastJumpTime = Time.time;
		jumpPressed = true;
	}

	private bool CanUseCoyote => coyoteUsable && !controller.collisions.below && lastGroundTime + coyoteBuffer > Time.time;
	private bool HasBufferedJump => controller.collisions.below && lastJumpTime + jumpBuffer > Time.time;

	void CalculateJump()
    {
		if (wallSliding && lastJumpTime + jumpBuffer > Time.time)
		{
			if (wallDirX == directionalInput.x)
			{
				velocity.x = -wallDirX * wallJumpClimb.x;
				velocity.y = wallJumpClimb.y;
			}
			else if (directionalInput.x == 0)
			{
				velocity.x = -wallDirX * wallJumpOff.x;
				velocity.y = wallJumpOff.y;
			}
			else
			{
				velocity.x = -wallDirX * wallLeap.x;
				velocity.y = wallLeap.y;
			}
			endedJumpEarly = false;
			audio.Play("jump");
		}
		if (jumpPressed && CanUseCoyote || HasBufferedJump)
		{
			coyoteUsable = false;
			endedJumpEarly = false;
			if (controller.collisions.slidingDownMaxSlope)
			{
				if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x))
				{ // not jumping against max slope
					velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
					velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
				}
			}
			else
			{
				velocity.y = maxJumpVelocity;
			}
			audio.Play("jump");
		}
		jumpPressed = false;
	}

	public void OnJumpInputUp()
	{
		if(!controller.collisions.below && !endedJumpEarly && velocity.y > 0)
        {
			endedJumpEarly = true;
		}
	}

	[Header("Effects at jump Apex")]
	private float apexPoint;
	[SerializeField] private float apexBonusAmount = 2f;
	[SerializeField] private float apexThresh = 10f;

	private void CalculateJumpApex()
	{
		if (!controller.collisions.below)
		{
			// Gets stronger the closer to the top of the jump
			apexPoint = Mathf.InverseLerp(apexThresh, 0, Mathf.Abs(velocity.y));
			gravity = Mathf.Lerp(minGravity, maxGravity, apexPoint);
		}
		else
		{
			apexPoint = 0;
		}
	}



	void HandleWallSliding()
	{
		wallDirX = (controller.collisions.left) ? -1 : 1;
		wallSliding = false;
		if (((controller.collisions.left && (directionalInput.x < 0 || timeTillUnstuckFromWall > 0)) 
			|| (controller.collisions.right && (directionalInput.x > 0 || timeTillUnstuckFromWall > 0))) 
			&& !controller.collisions.below 
			&& velocity.y < 0)
		{
			wallSliding = true;

			if (velocity.y < -wallSlideSpeedMax)
			{
				velocity.y = -wallSlideSpeedMax;
			}

			if (timeTillUnstuckFromWall == wallStickTime)
				audio.Play("wall");

			if (timeTillUnstuckFromWall > 0)
			{
				velocityXSmoothing = 0;
				velocity.x = 0;

				if (directionalInput.x != wallDirX)
					timeTillUnstuckFromWall -= Time.deltaTime;
				else
					timeTillUnstuckFromWall = wallStickTime;
			}
		}
		if(!wallSliding && controller.collisions.below)
			timeTillUnstuckFromWall = wallStickTime;
	}

	void CalculateVelocity()
	{
		float targetVelocityX = directionalInput.x * moveSpeed;
		float apexBonus = directionalInput.x * apexBonusAmount * apexPoint;
		targetVelocityX += apexBonus;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
		var fallSpeed = endedJumpEarly && velocity.y > 0 ? gravity * earlyJumpEndGravMod : gravity;
		velocity.y += fallSpeed * Time.deltaTime;
		if (velocity.y < fallClamp) velocity.y = fallClamp;
	}
}