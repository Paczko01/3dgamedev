
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
	private int bots = 3;
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
	public GameObject[] objs;
	public float verticalVel;
    private Vector3 moveVector;
	[SerializeField] private GameObject BombPrefab;
	[SerializeField] private GameObject ExplodePrefab;
	[SerializeField] private Transform Bombtransform;
	[SerializeField] private GameObject Bot1;
	[SerializeField] private GameObject Bot2;
	[SerializeField] private GameObject Bot3;
	[SerializeField] private GameObject Me;

	// Use this for initialization
	void Start () {
		hasbomb = true;
		anim = this.GetComponent<Animator> ();
		cam = Camera.main;
		controller = this.GetComponent<CharacterController> ();
		objs = FindObjectsOfType<GameObject>();
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
			Invoke("Dropbomb", 0.5f);
			expolseRot = Quaternion.Euler(90, 0, 0);
			Invoke("setboolback", 3.5f);
			Invoke("explode", 3.5f);
		}

	}
	private void Dropbomb()
    {
		GameObject bombClone = Instantiate(BombPrefab, bombpos, Bombtransform.rotation);
		Destroy(bombClone, 3f);
	}
	private void setboolback()
    {		
		hasbomb = true;
    }
	private void explode()
    {
		Vector3 explodepos = bombpos;
		Vector3 bot1pos = Bot1.transform.position;
		Vector3 bot2pos = Bot2.transform.position;
		Vector3 bot3pos = Bot3.transform.position;
		Vector3 mypos = Me.transform.position;
		bot1pos.x = 2f * Mathf.RoundToInt(bot1pos.x / 2f);
		bot1pos.y = 1f;
		bot1pos.z = 2f * Mathf.RoundToInt(bot1pos.z / 2f);
		bot2pos.x = 2f * Mathf.RoundToInt(bot2pos.x / 2f);
		bot2pos.y = 1f;
		bot2pos.z = 2f * Mathf.RoundToInt(bot2pos.z / 2f);
		bot3pos.x = 2f * Mathf.RoundToInt(bot3pos.x / 2f);
		bot3pos.y = 1f;
		bot3pos.z = 2f * Mathf.RoundToInt(bot3pos.z / 2f);
		mypos.x = 2f * Mathf.RoundToInt(mypos.x / 2f);
		mypos.y = 1f;
		mypos.z = 2f * Mathf.RoundToInt(mypos.z / 2f);
		GameObject explodeClone = Instantiate(ExplodePrefab,explodepos, expolseRot);
		if ((Mathf.RoundToInt(mypos.x) == Mathf.RoundToInt(explodepos.x)) && (Mathf.RoundToInt(mypos.y) == Mathf.RoundToInt(explodepos.y)) && (Mathf.RoundToInt(mypos.z) == Mathf.RoundToInt(explodepos.z)))
		{
			Debug.Log("MEGHALTÁL");
			Debug.Break();
		}
		if ((Mathf.RoundToInt(bot1pos.x) == Mathf.RoundToInt(explodepos.x)) && (Mathf.RoundToInt(bot1pos.y) == Mathf.RoundToInt(explodepos.y)) && (Mathf.RoundToInt(bot1pos.z) == Mathf.RoundToInt(explodepos.z)))
		{
			Bot1.SetActive(false);
			bots--;
		}
		if ((Mathf.RoundToInt(bot2pos.x) == Mathf.RoundToInt(explodepos.x)) && (Mathf.RoundToInt(bot2pos.y) == Mathf.RoundToInt(explodepos.y)) && (Mathf.RoundToInt(bot2pos.z) == Mathf.RoundToInt(explodepos.z)))
		{
			Bot2.SetActive(false);
			bots--;
		}
		if ((Mathf.RoundToInt(bot3pos.x) == Mathf.RoundToInt(explodepos.x)) && (Mathf.RoundToInt(bot3pos.y) == Mathf.RoundToInt(explodepos.y)) && (Mathf.RoundToInt(bot3pos.z) == Mathf.RoundToInt(explodepos.z)))
		{
			Bot3.SetActive(false);
			bots--;
		}
		for (int i = 0; i < objs.Length; i++)
        {
			if ((Mathf.RoundToInt(objs[i].transform.position.x) == Mathf.RoundToInt(explodepos.x)) && (Mathf.RoundToInt(objs[i].transform.position.y) == Mathf.RoundToInt(explodepos.y)) && (Mathf.RoundToInt(objs[i].transform.position.z) == Mathf.RoundToInt(explodepos.z)))
			{
				objs[i].SetActive(false);
				break;
            }
        }		
		explodepos.x += 2f;
		GameObject explodeClone1 = Instantiate(ExplodePrefab, explodepos, expolseRot);
		if ((Mathf.RoundToInt(mypos.x) == Mathf.RoundToInt(explodepos.x)) && (Mathf.RoundToInt(mypos.y) == Mathf.RoundToInt(explodepos.y)) && (Mathf.RoundToInt(mypos.z) == Mathf.RoundToInt(explodepos.z)))
		{
			Debug.Log("MEGHALTÁL");
			Debug.Break();
		}
		if ((Mathf.RoundToInt(bot1pos.x) == Mathf.RoundToInt(explodepos.x)) && (Mathf.RoundToInt(bot1pos.y) == Mathf.RoundToInt(explodepos.y)) && (Mathf.RoundToInt(bot1pos.z) == Mathf.RoundToInt(explodepos.z)))
		{
			Bot1.SetActive(false);
			bots--;
		}
		if ((Mathf.RoundToInt(bot2pos.x) == Mathf.RoundToInt(explodepos.x)) && (Mathf.RoundToInt(bot2pos.y) == Mathf.RoundToInt(explodepos.y)) && (Mathf.RoundToInt(bot2pos.z) == Mathf.RoundToInt(explodepos.z)))
		{
			Bot2.SetActive(false);
			bots--;
		}
		if ((Mathf.RoundToInt(bot3pos.x) == Mathf.RoundToInt(explodepos.x)) && (Mathf.RoundToInt(bot3pos.y) == Mathf.RoundToInt(explodepos.y)) && (Mathf.RoundToInt(bot3pos.z) == Mathf.RoundToInt(explodepos.z)))
		{
			Bot3.SetActive(false);
			bots--;
		}
		for (int i = 0; i < objs.Length; i++)
		{
			if ((Mathf.RoundToInt(objs[i].transform.position.x) == Mathf.RoundToInt(explodepos.x)) && (Mathf.RoundToInt(objs[i].transform.position.y) == Mathf.RoundToInt(explodepos.y)) && (Mathf.RoundToInt(objs[i].transform.position.z) == Mathf.RoundToInt(explodepos.z)))
			{
				objs[i].SetActive(false);
				break;
			}
		}
		explodepos.z += 2f;
		explodepos.x -= 2f;
		GameObject explodeClone2 = Instantiate(ExplodePrefab, explodepos, expolseRot);
		if ((Mathf.RoundToInt(mypos.x) == Mathf.RoundToInt(explodepos.x)) && (Mathf.RoundToInt(mypos.y) == Mathf.RoundToInt(explodepos.y)) && (Mathf.RoundToInt(mypos.z) == Mathf.RoundToInt(explodepos.z)))
		{
			Debug.Log("MEGHALTÁL");
			Debug.Break();
		}
		if ((Mathf.RoundToInt(bot1pos.x) == Mathf.RoundToInt(explodepos.x)) && (Mathf.RoundToInt(bot1pos.y) == Mathf.RoundToInt(explodepos.y)) && (Mathf.RoundToInt(bot1pos.z) == Mathf.RoundToInt(explodepos.z)))
		{
			Bot1.SetActive(false);
			bots--;
		}
		if ((Mathf.RoundToInt(bot2pos.x) == Mathf.RoundToInt(explodepos.x)) && (Mathf.RoundToInt(bot2pos.y) == Mathf.RoundToInt(explodepos.y)) && (Mathf.RoundToInt(bot2pos.z) == Mathf.RoundToInt(explodepos.z)))
		{
			Bot2.SetActive(false);
			bots--;
		}
		if ((Mathf.RoundToInt(bot3pos.x) == Mathf.RoundToInt(explodepos.x)) && (Mathf.RoundToInt(bot3pos.y) == Mathf.RoundToInt(explodepos.y)) && (Mathf.RoundToInt(bot3pos.z) == Mathf.RoundToInt(explodepos.z)))
		{
			Bot3.SetActive(false);
			bots--;
		}
		for (int i = 0; i < objs.Length; i++)
        {
			if ((Mathf.RoundToInt(objs[i].transform.position.x) == Mathf.RoundToInt(explodepos.x))&&(Mathf.RoundToInt(objs[i].transform.position.y) == Mathf.RoundToInt(explodepos.y))&&(Mathf.RoundToInt(objs[i].transform.position.z) == Mathf.RoundToInt(explodepos.z)))
            {
				objs[i].SetActive(false);
				break;
            }
        }
		explodepos.x -= 2f;
		explodepos.z -= 2f;
		GameObject explodeClone3 = Instantiate(ExplodePrefab, explodepos, expolseRot);
		if ((Mathf.RoundToInt(mypos.x) == Mathf.RoundToInt(explodepos.x)) && (Mathf.RoundToInt(mypos.y) == Mathf.RoundToInt(explodepos.y)) && (Mathf.RoundToInt(mypos.z) == Mathf.RoundToInt(explodepos.z)))
		{
			Debug.Log("MEGHALTÁL");
			Debug.Break();
		}
		if ((Mathf.RoundToInt(bot1pos.x) == Mathf.RoundToInt(explodepos.x)) && (Mathf.RoundToInt(bot1pos.y) == Mathf.RoundToInt(explodepos.y)) && (Mathf.RoundToInt(bot1pos.z) == Mathf.RoundToInt(explodepos.z)))
		{
			Bot1.SetActive(false);
			bots--;
		}
		if ((Mathf.RoundToInt(bot2pos.x) == Mathf.RoundToInt(explodepos.x)) && (Mathf.RoundToInt(bot2pos.y) == Mathf.RoundToInt(explodepos.y)) && (Mathf.RoundToInt(bot2pos.z) == Mathf.RoundToInt(explodepos.z)))
		{
			Bot2.SetActive(false);
			bots--;
		}
		if ((Mathf.RoundToInt(bot3pos.x) == Mathf.RoundToInt(explodepos.x)) && (Mathf.RoundToInt(bot3pos.y) == Mathf.RoundToInt(explodepos.y)) && (Mathf.RoundToInt(bot3pos.z) == Mathf.RoundToInt(explodepos.z)))
		{
			Bot3.SetActive(false);
			bots--;
		}
		for (int i = 0; i < objs.Length; i++)
        {
			if ((Mathf.RoundToInt(objs[i].transform.position.x) == Mathf.RoundToInt(explodepos.x))&&(Mathf.RoundToInt(objs[i].transform.position.y) == Mathf.RoundToInt(explodepos.y))&&(Mathf.RoundToInt(objs[i].transform.position.z) == Mathf.RoundToInt(explodepos.z)))
            {
				objs[i].SetActive(false);
				break;
            }
        }
		explodepos.z -= 2f;
		explodepos.x += 2f;
		GameObject explodeClone4 = Instantiate(ExplodePrefab, explodepos, expolseRot);
		if ((Mathf.RoundToInt(mypos.x) == Mathf.RoundToInt(explodepos.x)) && (Mathf.RoundToInt(mypos.y) == Mathf.RoundToInt(explodepos.y)) && (Mathf.RoundToInt(mypos.z) == Mathf.RoundToInt(explodepos.z)))
		{
			Debug.Log("MEGHALTÁL");
			Debug.Break();
		}
		if ((Mathf.RoundToInt(bot1pos.x) == Mathf.RoundToInt(explodepos.x)) && (Mathf.RoundToInt(bot1pos.y) == Mathf.RoundToInt(explodepos.y)) && (Mathf.RoundToInt(bot1pos.z) == Mathf.RoundToInt(explodepos.z)))
		{
			Bot1.SetActive(false);
			bots--;
		}
		if ((Mathf.RoundToInt(bot2pos.x) == Mathf.RoundToInt(explodepos.x)) && (Mathf.RoundToInt(bot2pos.y) == Mathf.RoundToInt(explodepos.y)) && (Mathf.RoundToInt(bot2pos.z) == Mathf.RoundToInt(explodepos.z)))
		{
			Bot2.SetActive(false);
			bots--;
		}
		if ((Mathf.RoundToInt(bot3pos.x) == Mathf.RoundToInt(explodepos.x)) && (Mathf.RoundToInt(bot3pos.y) == Mathf.RoundToInt(explodepos.y)) && (Mathf.RoundToInt(bot3pos.z) == Mathf.RoundToInt(explodepos.z)))
		{
			Bot3.SetActive(false);
			bots--;
		}
		for (int i = 0; i < objs.Length; i++)
        {
			if ((Mathf.RoundToInt(objs[i].transform.position.x) == Mathf.RoundToInt(explodepos.x))&&(Mathf.RoundToInt(objs[i].transform.position.y) == Mathf.RoundToInt(explodepos.y))&&(Mathf.RoundToInt(objs[i].transform.position.z) == Mathf.RoundToInt(explodepos.z)))
            {;
				objs[i].SetActive(false);
				break;
            }
        }
        if (bots==0)
        {
			Debug.Log("NYERTÉL");
			Debug.Break();
        }
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
