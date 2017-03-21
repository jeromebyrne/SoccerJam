using UnityEngine;
using System;

public class FootballPlayer : MonoBehaviour 
{
	private Vector3 m_DesiredDirection;
	private float m_CurrentSpeed;
	private Rigidbody m_RigidBody = null;
	bool mCanImpulseOnDirectionChange = true;
	float mTimeUntilCanImpulse = 0.1f;
	private bool mIsPlayerControlled = false;
	private bool mIsAIControlled = false;
	private bool mIsFriendlyTeamPlayer = true;
	private Vector3 m_InitialPosition;

	private const float kPlayerSpeed = 475.0f;
	private const float kOppositionSpeed = 1.0f;

	void Start () 
	{
		m_DesiredDirection.x = 0;
		m_DesiredDirection.y = 0;
		m_DesiredDirection.z = 1;

		m_CurrentSpeed = kPlayerSpeed;
	}

	void Update() 
	{
		if (mIsPlayerControlled || mIsAIControlled)
		{
			if (m_RigidBody) 
			{
				m_RigidBody.transform.forward = Vector3.RotateTowards (m_RigidBody.transform.forward, m_DesiredDirection, 0.1f, 0.75f);

				if (m_RigidBody.velocity.sqrMagnitude < (mIsFriendlyTeamPlayer ? 90.0f : 20.0f))
				{
					m_RigidBody.AddForce (m_RigidBody.transform.forward * m_CurrentSpeed);
				}
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


	public void SetInitialPosition (Vector3 initialPos)
	{
		m_InitialPosition = initialPos;
	}

	public Vector3 GetInitialPosition()
	{
		return m_InitialPosition;
	}

	public void SetPlayerControlled(bool value)
	{
		mIsPlayerControlled = value;
	}

	public void SetIsAIControlled (bool value)
	{
		mIsAIControlled = value;
	}

	public void SetIsFriendlyTeamPlayer (bool value)
	{
		mIsFriendlyTeamPlayer = value;

		if (mIsFriendlyTeamPlayer) {
			m_CurrentSpeed = kPlayerSpeed;
		} else {
			m_CurrentSpeed = kOppositionSpeed;

			GetComponent<Rigidbody> ().mass = 80.0f;
		}
	}

	public bool IsFriendlyTeamPlayer()
	{
		return mIsFriendlyTeamPlayer;
	}

	public static implicit operator FootballPlayer(Collider v)
    {
        throw new NotImplementedException();
    }

	public void SetDirection (float x, float z)
	{
		m_DesiredDirection.x = x;
		m_DesiredDirection.y = 0.0f;
		m_DesiredDirection.z = z;

		// m_RigidBody.transform.forward = new Vector3(m_DesiredDirection.x, m_DesiredDirection.y, m_DesiredDirection.z);

		if (mCanImpulseOnDirectionChange)
		{
			// m_RigidBody.AddForce (m_DesiredDirection * (m_CurrentSpeed * 0.1f), ForceMode.Impulse);
			mCanImpulseOnDirectionChange = false;
			mTimeUntilCanImpulse = 0.5f;
		}
	}
}
