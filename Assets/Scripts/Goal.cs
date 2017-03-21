using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

	public bool m_IsOppositionGoal = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "FootballCollider")
		{
			GetComponent<AudioSource> ().Play ();

			GameManager.Instance.ScoreGoal (!m_IsOppositionGoal);
		}
	}

	void OnTriggerStay(Collider other)
	{
	}

	void OnTriggerExit(Collider other)
	{
	}
}
