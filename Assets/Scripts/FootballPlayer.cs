using UnityEngine;
using System;

public class FootballPlayer : MonoBehaviour 
{
	private Vector3 m_CurrentDirection;
	private float m_CurrentSpeed;
	private Rigidbody m_RigidBody = null;
	bool mCanImpulseOnDirectionChange = true;
	float mTimeUntilCanImpulse = 0.1f;
	bool mIsPlayerControlled = true;

	void Start () 
	{
		m_CurrentDirection.x = 0;
		m_CurrentDirection.y = 0;
		m_CurrentDirection.z = 1;

		m_CurrentSpeed = 800.0f;
	}

	void Update() 
	{
		if (m_RigidBody) 
		{
			m_RigidBody.AddForce (m_CurrentDirection * m_CurrentSpeed);

			// if (m_RigidBody.velocity.sqrMagnitude < 200)
			{
			}
		} 
		else
		{
			m_RigidBody = GetComponent<Rigidbody> ();
		}

		if (mIsPlayerControlled)
		{
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
			m_RigidBody.AddForce (m_CurrentDirection * (m_CurrentSpeed * 0.75f), ForceMode.Impulse);
			mCanImpulseOnDirectionChange = false;
			mTimeUntilCanImpulse = 2.0f;
		}
	}
}
