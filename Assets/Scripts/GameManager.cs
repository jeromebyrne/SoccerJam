using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	public Camera m_Camera = null;
	public FootballPlayer m_TestPlayer = null;
	private static GameManager m_Instance;						// Damien : Making GameManager a singleton

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
		if (m_TestPlayer == null) 
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
				Vector3 diff =  hit.point - m_TestPlayer.transform.position;
				diff.Normalize ();

				m_TestPlayer.SetDirection (diff.x, diff.z);
			}
		}
	}

	void UpdateCamera ()
	{
		if (m_Camera == null)
		{
			return;
		}

		if (m_TestPlayer == null)
		{
			return;
		}

		Vector3 position = m_Camera.transform.position;
		position.x = m_TestPlayer.transform.position.x;
		position.y = m_CameraYOffset;
		position.z = m_TestPlayer.transform.position.z + m_CameraZOffset;

		m_Camera.transform.position = position;
				
		/*
	private void FindAveragePosition()
	{
		Vector3 averagePos = new Vector3();
		int numTargets = 0;

		for (int i = 0; i < m_Targets.Length; i++)
		{
			if (!m_Targets[i].gameObject.activeSelf)
				continue;

			averagePos += m_Targets[i].position;
			numTargets++;
		}

		if (numTargets > 0)
			averagePos /= numTargets;

		averagePos.y = transform.position.y;

		m_DesiredPosition = averagePos;
	}


	private void Zoom()
	{
		float requiredSize = FindRequiredSize();
		m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
	}


	private float FindRequiredSize()
	{
		Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);

		float size = 0f;

		for (int i = 0; i < m_Targets.Length; i++)
		{
			if (!m_Targets[i].gameObject.activeSelf)
				continue;

			Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].position);

			Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

			size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.y));

			size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.x) / m_Camera.aspect);
		}

		size += m_ScreenEdgeBuffer;

		size = Mathf.Max(size, m_MinSize);

		return size;
	}
	*/
	}
}
