using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStarPatrol : MonoBehaviour {
	public float speed;
	public float detection_range;
	private float waitTime;
	public float startWaitTime;

	public Transform[] moveSpots;
	private int randomSpot;

	void Start() {
		randomSpot = Random.Range(0, moveSpots.Length);
		detection_range = 5.0f;
	}

	void Update() {
		detection_range += Time.deltaTime * 0.05f;
		float dist = Vector3.Distance(transform.position, GameObject.Find("Player").transform.position);
		if(dist > detection_range) {
			transform.position = Vector3.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);
		}
		else {
			transform.position = Vector3.MoveTowards(transform.position, GameObject.Find("Player").transform.position, speed * Time.deltaTime);
			speed += Time.deltaTime * 0.1f;
		}
		if(Vector3.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f){
			if(waitTime <= 0) {
				randomSpot = Random.Range(0, moveSpots.Length);
				waitTime = startWaitTime;
			}
			else {
				waitTime -= Time.deltaTime;
			}
		}
	}
}
