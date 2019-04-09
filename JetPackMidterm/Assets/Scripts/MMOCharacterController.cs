using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MMOCharacterController : MonoBehaviour {
	public Transform playerCam, character, centerPoint;

	private float vertVelocity;
	public float jumpForce = 4.0f;
	private float mouseX, mouseY;
	public float mouseSensitivity = 10f;
	public float mouseYPosition = 1f;

	private float moveFB, moveLR;
	public float moveSpeed = 2f;

	private float zoom;
	public float zoomSpeed = 2;

	public float zoomMin = -2f;
	public float zoomMax = -10f;

	public float rotationSpeed = 5f;
	private bool hasJumped;
	public float Current_Fuel = 100f;
	public bool Fuel_Warning_Fifty = false;
	public bool Fuel_Warning_Zero = false;
	public int Total_Pilots_Rescued = 0;
	public int WinCount = 0;
	// Use this for initialization
	void Start () {
		zoom = -3;
		StartCoroutine(ShowMessage("Your fleet has been irreparably damaged by rogue shooting stars. Rescue the three pilots from the wreckage of their ships before the stars notice your presence. (Look for the pilot's white distress beacons on each wrecked ship.)", 15));
	}
	
	// Update is called once per frame
	void Update () {
		CheckFuel();
		CheckPlayerPosition();
		if(Total_Pilots_Rescued < 3) UpdateFuelAndPilotsText(true);
		else UpdateFuelAndPilotsText(false);

		if(Current_Fuel < 50 && !(Fuel_Warning_Fifty))
		{
			StartCoroutine(ShowMessage("You are running low on fuel! Look for glowing yellow rods on the platforms around you: Those are fuel cells!", 15));
			Fuel_Warning_Fifty = true;
		}
		if(Current_Fuel <= 0 && !(Fuel_Warning_Zero))
		{
			StartCoroutine(ShowMessage("You are out of fuel! Find a fuel cell, fast!", 15));
			Fuel_Warning_Zero = true;
		}

		zoom += Input.GetAxis ("Mouse ScrollWheel") * zoomSpeed;

		if (zoom > zoomMin)
			zoom = zoomMin;

		if (zoom < zoomMax)
			zoom = zoomMax;

		playerCam.transform.localPosition = new Vector3 (0, 0, zoom);

		if (Input.GetMouseButton (1)) {
			mouseX += Input.GetAxis ("Mouse X");
			mouseY -= Input.GetAxis ("Mouse Y");
		}
		if (Input.GetKey(KeyCode.Space)) {
			if(Current_Fuel > 0f) {
				ApplyGravity(false);
				Jump();
				Current_Fuel -= Time.deltaTime * 2;
			}
		}
		else 
		{
			if(Current_Fuel > 0f && Current_Fuel <= 100.0f) Current_Fuel += Time.deltaTime * 0.1f;
		}
		ApplyGravity(true);
		
		mouseY = Mathf.Clamp (mouseY, -60f, 60f);
		playerCam.LookAt (centerPoint);
		centerPoint.localRotation = Quaternion.Euler (mouseY, mouseX, 0);

		moveFB = Input.GetAxis ("Vertical") * moveSpeed;
		moveLR = Input.GetAxis ("Horizontal") * moveSpeed;

		Vector3 movement = new Vector3 (moveLR, vertVelocity, moveFB);
		movement = character.rotation * movement;
		character.GetComponent<CharacterController> ().Move (movement * Time.deltaTime);
		centerPoint.position = new Vector3 (character.position.x, character.position.y + mouseYPosition, character.position.z);
		if (Input.GetAxis ("Vertical") > 0 | Input.GetAxis ("Vertical") < 0)
		{

			Quaternion turnAngle = Quaternion.Euler (0, centerPoint.eulerAngles.y, 0);

			character.rotation = Quaternion.Slerp (character.rotation, turnAngle, Time.deltaTime * rotationSpeed);

		}
	
	}
	public void CheckPlayerPosition()
	{
		if(this.transform.position.y < -1000) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
	}
	public void UpdateFuelAndPilotsText(bool a)
	{
		if(a)
		{
			//Fuel Value HUD
			Text myTXT = GameObject.Find("Canvas/Jetpack_Fuel").GetComponent<Text>();
			myTXT.text = "Fuel: " + Current_Fuel.ToString();
			//Pilot Count HUD
			Text PilotCount = GameObject.Find("Canvas/Pilot_Count").GetComponent<Text>();
			PilotCount.text = "Pilots: " + Total_Pilots_Rescued.ToString() + " / 3";
		}
		else 
		{
			Text myTXT = GameObject.Find("Canvas/Jetpack_Fuel").GetComponent<Text>();
			myTXT.text = "";
			Text PilotCount = GameObject.Find("Canvas/Pilot_Count").GetComponent<Text>();
			PilotCount.text = "";
			Text WinTxt = GameObject.Find("Canvas/WinText").GetComponent<Text>();
			WinTxt.text = "You win!!!";
			WinCount++;
		}
		if(WinCount > 200)
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
		}
	}
	public void UpdatePilotsRescued()
	{
		GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Pilot");
		for(int i = 0; i < taggedObjects.Length; i++)
         {
            if(Vector3.Distance(this.transform.position, 
                     taggedObjects[i].transform.position) <= 8.0f)
            {
                taggedObjects[i].SetActive(false);
				Total_Pilots_Rescued++;
            }
         }
	}

	public bool CheckFuel()
	{
		if (Current_Fuel > 0f) return true;
		else return false;
	}
	public void UpdateFuel()
	{
		if(Current_Fuel > 80f) Current_Fuel = 100f;
		else Current_Fuel += 20f;
	}
	public void DrainFuel()
	{
		if(Current_Fuel > 1f) Current_Fuel -= Time.deltaTime * 10;
	}


	private void Jump()
	{
		vertVelocity += 20.0f * Time.deltaTime;
		//if (Input.GetKey(KeyCode.Q)) {vertVelocity += 100.0f * Time.deltaTime;}
		//if (Input.GetKey(KeyCode.E)) {vertVelocity -= 20.0f * Time.deltaTime;}
		//vertVelocity += 10.0f * Time.deltaTime;

		hasJumped = true;
		
	}

	private void ApplyGravity(bool Apply)
	{
		if(Apply)
		{
			if (character.GetComponent<CharacterController> ().isGrounded == true)
			//if (Current_Fuel > 0f)
			{
				if (hasJumped == false) vertVelocity = Physics.gravity.y;
				else vertVelocity = jumpForce;
			}
			else
			{
				vertVelocity += Physics.gravity.y * Time.deltaTime;
				//vertVelocity = Mathf.Clamp(vertVelocity, -50f, 20f);
				hasJumped = false;
			}
		}
	}


	IEnumerator ShowMessage(string message, float delay)
	{
		Text guiText = GameObject.Find("Canvas/GUI_Text").GetComponent<Text>();
		guiText.text = message;
		guiText.enabled = true;
		yield return new WaitForSeconds(delay);
		guiText.enabled = false;
	}
}