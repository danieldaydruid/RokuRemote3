using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyApproachAudio : MonoBehaviour {
	public bool SaveState;
	public AudioClip MusicClip;
	public AudioSource MusicSource;
	public float detection_range;
	// Use this for initialization
	void Start () {
		detection_range = 5.0f;
		MusicSource.clip = MusicClip;
		SaveState = false;
		MusicSource.Stop();
	}
	
	// Update is called once per frame
	void Update () {
	detection_range += Time.deltaTime * 0.05f;
	FindClosestEnemy();
	}
	void FindClosestEnemy()
	{
		float distanceToClosestEnemy = Mathf.Infinity;
		EnemyStarPatrol closestEnemy = null;
		EnemyStarPatrol[] allEnemies = GameObject.FindObjectsOfType<EnemyStarPatrol>();

		foreach (EnemyStarPatrol currentEnemy in allEnemies)
		{
			float distanceToEnemy = (currentEnemy.transform.position - GameObject.FindWithTag("Player").transform.position).sqrMagnitude;
			if(distanceToEnemy < distanceToClosestEnemy)
			{
				distanceToClosestEnemy = distanceToEnemy;
				closestEnemy = currentEnemy;
			}
		}
		float dist = Vector3.Distance(GameObject.FindWithTag("Player").transform.position, closestEnemy.transform.position);
		if(dist <= detection_range) CheckDistance(true);
		else CheckDistance(false);
		//Debug.DrawLine(GameObject.FindWithTag("Player").transform.position, closestEnemy.transform.position, Color.green, 10.0f);
	}

	void CheckDistance(bool a)
	{
		if(a && !(SaveState) )
		{
			MusicSource.Play();
			SaveState = true;
			//Debug.Log("CheckDistance turned ON");
		}
		else if( !(a) && SaveState )
		{
			MusicSource.Stop();
			SaveState = false;
		}
	}
}