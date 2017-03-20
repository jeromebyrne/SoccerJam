using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootballPlayerSpawner : MonoBehaviour {

	public FootballPlayer m_FootballPlayerPrefab;

	public bool m_IsFriendlyTeamPlayerSpawner = true;

	private FootballPlayer m_SpawnedPlayer = null;

	public Material m_OppositionMaterial = null;

	private bool mHasDoneSetup = false;

	// Use this for initialization
	void Start () 
	{
		SpawnPlayer ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	public void SpawnPlayer()
	{
		m_SpawnedPlayer = null;
		m_SpawnedPlayer = Instantiate (m_FootballPlayerPrefab) as FootballPlayer;

		if (m_SpawnedPlayer)
		{
			m_SpawnedPlayer.transform.position = transform.position;
			m_SpawnedPlayer.SetPlayerControlled (false);
			m_SpawnedPlayer.SetIsFriendlyTeamPlayer (m_IsFriendlyTeamPlayerSpawner);

			if (!m_IsFriendlyTeamPlayerSpawner)
			{
				GameObject cubeChild = m_SpawnedPlayer.transform.GetChild (0).gameObject;

				if (cubeChild)
				{
					Renderer rc = cubeChild.GetComponent<Renderer> ();
					 
					if (rc)
					{
						rc.material = m_OppositionMaterial;
					}

				}
			}
		}
	}
}
