using UnityEngine;
using System;

public class FootballPlayer : MonoBehaviour 
{
	private Vector3 m_CurrentDirection;
	private float m_CurrentSpeed;
	private Rigidbody m_RigidBody = null;
	bool mCanImpulseOnDirectionChange = true;
	float mTimeUntilCanImpulse = 0.1f;
	private bool mIsPlayerControlled = true;
	private bool mIsFriendlyTeamPlayer = true;

	void Start () 
	{
		m_CurrentDirection.x = 0;
		m_CurrentDirection.y = 0;
		m_CurrentDirection.z = 1;

		m_CurrentSpeed = 300.0f;
	}

	void Update() 
	{
		if (mIsPlayerControlled)
		{
			if (m_RigidBody) 
			{
				m_RigidBody.AddForce (m_CurrentDirection * m_CurrentSpeed);
			} 
			else
			{
				m_RigidBody = GetComponent<Rigidbody> ();
			}

			if (!mCanImpulseOnDirectionChange)
			{
				mTimeUntilCanImpulse -= Time.deltaTime;

				if (mTimeUntilCanImpulse <= 0.0f)
				{
					mCanImpulseOnDirectionChange = true;
				}
			}
		}
	}

	public void SetPlayerControlled(bool value)
	{
		mIsPlayerControlled = value;
	}

	public void SetIsFriendlyTeamPlayer (bool value)
	{
		mIsFriendlyTeamPlayer = value;
	}

	public static implicit operator FootballPlayer(Collider v)
    {
        throw new NotImplementedException();
    }

	public void SetDirection (float x, float z)
	{
		m_CurrentDirection.x = x;
		m_CurrentDirection.z = z;

		if (mCanImpulseOnDirectionChange)
		{
			m_RigidBody.AddForce (m_CurrentDirection * (m_CurrentSpeed * 0.1f), ForceMode.Impulse);
			mCanImpulseOnDirectionChange = false;
			mTimeUntilCanImpulse = 0.5f;
		}
	}
}
