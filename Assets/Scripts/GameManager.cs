using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	public Camera m_Camera = null;
	private static GameManager m_Instance;
	public Football m_Football = null;

	private float m_CameraZOffset = -7.0f;
	private float m_CameraYOffset = 10.0f;
	private float m_TimeLeftUntilCanSwitchPlayer = 3.0f;

	private bool m_IsSetup = false;
	private FootballPlayer m_CurrentSelectedPlayer = null;

	List<FootballPlayer> mFriendlyTeam = new List<FootballPlayer>();
	List<FootballPlayer> mOppositionTeam = new List<FootballPlayer>();

	public static GameManager Instance
	{
		get 
		{
			if (m_Instance == null)
			{
				m_Instance = new GameManager();
			}
			return m_Instance;
		}
	}

	void RebuildTeams()
	{
		mFriendlyTeam.Clear();
		mOppositionTeam.Clear();

		FootballPlayer[] players = FindObjectsOfType<FootballPlayer> ();

		foreach(FootballPlayer p in players)
		{
			if (p.IsFriendlyTeamPlayer ()) 
			{
				mFriendlyTeam.Add (p);
			} 
			else 
			{
				mOppositionTeam.Add (p);
			}
		}
	}

	void Start () 
	{
		m_Instance = this;	
	}

	void DoSetup()
	{
		
	}

	void Update () 
	{
		// Doing setup in Update because scene not fully created in Start()
		if (!m_IsSetup)
		{	
			DoSetup ();
			m_IsSetup = true;
			return;
		}

		if (mFriendlyTeam.Count == 0 || mOppositionTeam.Count == 0)
		{
			// intitial build
			RebuildTeams ();

			m_CurrentSelectedPlayer = GetClosestFriendlyPlayerToBall ();

			if (m_CurrentSelectedPlayer)
			{
				m_CurrentSelectedPlayer.SetPlayerControlled (true);
			}
		}

		TakeInput ();

		UpdateCamera ();

		ApplyBallControlAid ();

		UpdateCurrentPlayer ();

		UpdateAI ();
	}

	void TakeInput()
	{
		if (m_CurrentSelectedPlayer == null) 
		{
			return;
		}
			
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch (0);

			if (touch.phase == TouchPhase.Ended)
			{
				/*
				RaycastHit hit;

				Ray rr = m_Camera.ScreenPointToRay (touch.position);

				if (Physics.Raycast(rr, out hit) )
				{
					NavNode nearestNode = m_NavGrid.GetNearestNode (hit.point.x, hit.point.z);

					if (nearestNode != null)
					{
						var path = m_NavGrid.GetPath (m_LocalPlayer.m_CurrentNavGridRow, m_LocalPlayer.m_CurrentNavGridColumn, nearestNode.m_RowIndex, nearestNode.m_ColumnIndex);

						m_LocalPlayer.SetPathAndMove (path);
					}
				}
				*/
			}
		}
			
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;

			Ray rr = m_Camera.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast(rr, out hit) )
			{
				Vector3 diff =  hit.point - m_CurrentSelectedPlayer.transform.position;
				diff.Normalize ();

				m_CurrentSelectedPlayer.SetDirection (diff.x, diff.z);
			}
		}
	}

	void UpdateCamera ()
	{
		if (m_Camera == null)
		{
			return;
		}

		if (m_CurrentSelectedPlayer == null)
		{
			return;
		}

		Vector3 desiredPosition = m_Camera.transform.position;
		desiredPosition.x = m_CurrentSelectedPlayer.transform.position.x;
		desiredPosition.y = m_CameraYOffset;
		desiredPosition.z = m_CurrentSelectedPlayer.transform.position.z + m_CameraZOffset;

		Vector3 pos = Vector3.MoveTowards (m_Camera.transform.position, desiredPosition, 1.0f);

		m_Camera.transform.position = pos;
	}

	FootballPlayer GetClosestFriendlyPlayerToBall()
	{
		if (m_Football == null)
		{
			return null;
		}

		float smallestDistance = -1.0f;
		FootballPlayer selectedPlayer = null;

		foreach (FootballPlayer p in mFriendlyTeam)
		{
			Vector3 diff = m_Football.transform.position - p.transform.position;

			float magSqrAbs = Mathf.Abs (diff.sqrMagnitude);

			if (smallestDistance == -1 ||
				magSqrAbs < smallestDistance)
			{
				smallestDistance = magSqrAbs;
				selectedPlayer = p;
			}
		}

		return selectedPlayer;
	}

	void ApplyBallControlAid()
	{
		if (m_CurrentSelectedPlayer == null)
		{
			return;
		}

		if (m_Football == null)
		{
			return;
		}

		if (m_Football.transform.position.z - 8 > m_CurrentSelectedPlayer.transform.position.z)
		{
			// the ball is on front of the player so don't suck the ball in (we want it to go forward)
			return;
		}

		Vector3 diff = m_Football.transform.position - m_CurrentSelectedPlayer.transform.position;

		float magSqrAbs = Mathf.Abs (diff.sqrMagnitude);

		if (magSqrAbs < 100)
		{
			// suck the ball gently towards the player
			Rigidbody rb = m_Football.GetComponent<Rigidbody>();

			if (rb)
			{
				diff.Normalize ();
				rb.AddForce (diff * -0.5f);

				// rb.transform.forward = Vector3.RotateTowards (rb.transform.forward, m_CurrentSelectedPlayer.transform.forward, 1.0f, 1.0f);
			}
		}
	}

	void UpdateCurrentPlayer()
	{
		if (m_TimeLeftUntilCanSwitchPlayer > 0.0f)
		{
			m_TimeLeftUntilCanSwitchPlayer -= Time.deltaTime;
		}

		if (!m_CurrentSelectedPlayer) {
			m_CurrentSelectedPlayer = GetClosestFriendlyPlayerToBall ();

			if (m_CurrentSelectedPlayer) {
				m_CurrentSelectedPlayer.SetPlayerControlled (true);
			}
		} 
		else
		{
			if (m_TimeLeftUntilCanSwitchPlayer > 0.0f)
			{
				return;
			}

			// if the current player is too far away from the ball then select a new player
			Vector3 diff = m_Football.transform.position - m_CurrentSelectedPlayer.transform.position;

			float magSqrAbs = Mathf.Abs (diff.sqrMagnitude);

			if (magSqrAbs > 300) 
			{
				// pick new player
				float smallestDistance = -1.0f;

				foreach (FootballPlayer p in mFriendlyTeam)
				{
					if (p == m_CurrentSelectedPlayer)
					{
						continue;
					}

					Vector3 diffP = m_Football.transform.position - p.transform.position;

					float magSqrAbsP = Mathf.Abs (diffP.sqrMagnitude);

					if (smallestDistance == -1 ||
						magSqrAbsP < smallestDistance)
					{
						m_CurrentSelectedPlayer.SetPlayerControlled (false);
						smallestDistance = magSqrAbsP;
						m_CurrentSelectedPlayer = p;
						m_CurrentSelectedPlayer.SetPlayerControlled (true);

						m_TimeLeftUntilCanSwitchPlayer = 3.0f;
					}
				}
			}
		}	
	}

	void UpdateAI()
	{
		foreach (FootballPlayer p in mFriendlyTeam) 
		{
			if (p != m_CurrentSelectedPlayer)
			{
				// move towards initial position
				Vector3 moveToPos = p.GetInitialPosition();
				moveToPos.y = p.transform.position.y;
				Vector3 pos = Vector3.MoveTowards(p.transform.position, moveToPos, 0.1f);

				p.transform.position = pos;
			}
		}

		foreach (FootballPlayer p in mOppositionTeam) 
		{
			// move towards initial position
			Vector3 moveToPos = p.GetInitialPosition();
			moveToPos.y = p.transform.position.y;
			Vector3 pos = Vector3.MoveTowards(p.transform.position, moveToPos, 0.1f);

			p.transform.position = pos;
		}
	}
}
