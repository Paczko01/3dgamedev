
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script requires you to have setup your animator with 3 parameters, "InputMagnitude", "InputX", "InputZ"
//With a blend tree to control the inputmagnitude and allow blending between animations.
[RequireComponent(typeof(CharacterController))]
public class MovementInput : MonoBehaviour {

    public float Velocity;
    [Space]
	public float InputX;
	public float InputZ;
	public Vector3 desiredMoveDirection;
	public bool blockRotationPlayer;
	public float desiredRotationSpeed = 0.1f;
	public Animator anim;
	public float Speed;
	public float allowPlayerRotation = 0.1f;
	public Camera cam;
	public CharacterController controller;
	public bool isGrounded;
	public bool hasbomb;
	public float interval = 3f;
    [Header("Animation Smoothing")]
    [Range(0, 1f)]
    public float HorizontalAnimSmoothTime = 0.2f;
    [Range(0, 1f)]
    public float VerticalAnimTime = 0.2f;
    [Range(0,1f)]
    public float StartAnimTime = 0.3f;
    [Range(0, 1f)]
    public float StopAnimTime = 0.15f;
	public Vector3 bombpos;
	public Quaternion expolseRot;

	public float verticalVel;
    private Vector3 moveVector;
	[SerializeField] private GameObject BombPrefab;
	[SerializeField] private GameObject ExplodePrefab;
	[SerializeField] private Transform Bombtransform;

	// Use this for initialization
	void Start () {
		hasbomb = true;
		anim = this.GetComponent<Animator> ();
		cam = Camera.main;
		controller = this.GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		InputMagnitude ();

        isGrounded = controller.isGrounded;
        if (isGrounded)
        {
            verticalVel -= 0;
        }
        else
        {
            verticalVel -= 1;
        }
        
        moveVector = new Vector3(0, verticalVel * .2f * Time.deltaTime, 0);
		controller.Move(moveVector);
		if (Input.GetKeyDown(KeyCode.Space)&& hasbomb)
		{
			bombpos = Bombtransform.position;
			hasbomb = false;
            bombpos.x =2f*Mathf.RoundToInt(bombpos.x/2f);
            bombpos.z =2f*Mathf.RoundToInt(bombpos.z/2f);
            if (bombpos.x>20f) bombpos.x = 20f;
			if (bombpos.z > 20f) bombpos.z = 20f;
			if (bombpos.x < -20f) bombpos.x = -20f;
			if (bombpos.z < -20f) bombpos.z = -20f;
			GameObject bombClone = Instantiate(BombPrefab, bombpos, Bombtransform.rotation);
			expolseRot = Quaternion.Euler(90, 0, 0);
			Invoke("setboolback", 3f);
			Invoke("explode", 3.01f);
			Destroy(bombClone,3f);
		}

	}

	private void setboolback()
    {		
		hasbomb = true;
    }
	private void explode()
    {
		Vector3 explodepos = bombpos;
		GameObject explodeClone = Instantiate(ExplodePrefab,explodepos, expolseRot);
		explodepos.x += 2f;
		GameObject explodeClone1 = Instantiate(ExplodePrefab, explodepos, expolseRot);
		explodepos.z += 2f;
		explodepos.x -= 2f;
		GameObject explodeClone2 = Instantiate(ExplodePrefab, explodepos, expolseRot);
		explodepos.x -= 2f;
		explodepos.z -= 2f;
		GameObject explodeClone3 = Instantiate(ExplodePrefab, explodepos, expolseRot);
		explodepos.z -= 2f;
		explodepos.x += 2f;
		GameObject explodeClone4 = Instantiate(ExplodePrefab, explodepos, expolseRot);
		Destroy(explodeClone, 2f);
		Destroy(explodeClone1, 2f);
		Destroy(explodeClone2, 2f);
		Destroy(explodeClone3, 2f);
		Destroy(explodeClone4, 2f);
	}

	void PlayerMoveAndRotation() {
		InputX = Input.GetAxis ("Horizontal");
		InputZ = Input.GetAxis ("Vertical");

		var camera = Camera.main;
		var forward = cam.transform.forward;
		var right = cam.transform.right;

		forward.y = 0f;
		right.y = 0f;

		forward.Normalize ();
		right.Normalize ();

		desiredMoveDirection = forward * InputZ + right * InputX;

		if (blockRotationPlayer == false) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (desiredMoveDirection), desiredRotationSpeed);
            controller.Move(desiredMoveDirection * Time.deltaTime * Velocity);
		}
	}

    public void LookAt(Vector3 pos)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(pos), desiredRotationSpeed);
    }

    public void RotateToCamera(Transform t)
    {

        var camera = Camera.main;
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        desiredMoveDirection = forward;

        t.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
    }

	void InputMagnitude() {
		//Calculate Input Vectors
		InputX = Input.GetAxis ("Horizontal");
		InputZ = Input.GetAxis ("Vertical");

		//anim.SetFloat ("InputZ", InputZ, VerticalAnimTime, Time.deltaTime * 2f);
		//anim.SetFloat ("InputX", InputX, HorizontalAnimSmoothTime, Time.deltaTime * 2f);

		//Calculate the Input Magnitude
		Speed = new Vector2(InputX, InputZ).sqrMagnitude;

        //Physically move player

		if (Speed > allowPlayerRotation) {
			anim.SetFloat ("Blend", Speed, StartAnimTime, Time.deltaTime);
			PlayerMoveAndRotation ();
		} else if (Speed < allowPlayerRotation) {
			anim.SetFloat ("Blend", Speed, StopAnimTime, Time.deltaTime);
		}
	}
}
