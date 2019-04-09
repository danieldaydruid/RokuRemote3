using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetect : MonoBehaviour {
	public GameObject _MMOCharacterController;
	// Use this for initialization              
	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("FuelCell")) {
			other.gameObject.SetActive(false);
			_MMOCharacterController.GetComponent<MMOCharacterController>().UpdateFuel();
		}
	}
	public void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Enemy")) {
			_MMOCharacterController.GetComponent<MMOCharacterController>().DrainFuel();
		}
	}
}
