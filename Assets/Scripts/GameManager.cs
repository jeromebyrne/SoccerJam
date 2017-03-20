using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	public Camera m_Camera = null;
	private static GameManager m_Instance;
	public Football m_Football = null;
	public FootballPlayer m_CurrentSelectedPlayer = null;

	private float m_CameraZOffset = -7.0f;
	private float m_CameraYOffset = 10.0f;

	private bool m_IsSetup = false;

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

	void Start () 
	{
		m_Instance = this;	// Hacky singleton

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

		TakeInput ();

		UpdateCamera ();
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

		Vector3 position = m_Camera.transform.position;
		position.x = m_CurrentSelectedPlayer.transform.position.x;
		position.y = m_CameraYOffset;
		position.z = m_CurrentSelectedPlayer.transform.position.z + m_CameraZOffset;

		m_Camera.transform.position = position;
	}
}
