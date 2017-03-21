using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {

	public Text m_GoalText = null;
	public Text m_FriendlyText = null;
	public Text m_OppositionText = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_FriendlyText.text = GameManager.Instance.GetFriendlyScore ().ToString ();
		m_OppositionText.text = GameManager.Instance.GetOppositionScore ().ToString ();
		m_GoalText.enabled = GameManager.Instance.IsInGoalScoredCooldown ();
	}
}
