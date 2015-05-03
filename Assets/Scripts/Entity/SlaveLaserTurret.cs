using UnityEngine;
using System.Collections;
using RedHelp;
/*
public class SlaveLaserTurret : MonoBehaviour {

	private SubTurret MyAI; 
	private LineRenderer line;
	private Transform hit;
	private int lasertime = -1;
	public LayerMask collision;
	public LayerMask noplayercollision;

	public void SetVisibility(bool v)
	{
		MyAI.Visible = v;
	}

	void Start()
	{
		line.SetPosition(0, MyAI.FirePosition.position);
	}

	public void SetAI(SubTurret ai)
	{
		MyAI = ai;
	}

	void FixedUpdate()
	{
		// Rotating
		if (MyAI.Tracking == TrackingType.Turn)
		{
			gameObject.transform.rotation = Quaternion.Euler(0, 0, gameObject.transform.rotation.eulerAngles.z + MyAI.TurnSpeed);
		}
			else if (!((MyAI.Tracking == TrackingType.Aim || MyAI.Tracking == TrackingType.Fire) && MyAI.Visible == false))
		{
			Vector3 lookDirection = MyAI.Target.transform.position - gameObject.transform.position;
			float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
			Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
			transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, targetRotation, MyAI.TurnSpeed);
		}

		// Time Seen
		MyAI.TimeSeen = (MyAI.Visible) ? ++MyAI.TimeSeen : 0;
		
		if (MyAI.TimeSeen != 0 && ((MyAI.TimeSeen <= MyAI.InitDelay) ? MyAI.TimeSeen % MyAI.InitDelay  == 0 : (MyAI.TimeSeen - MyAI.InitDelay) % MyAI.ContDelay == 0))
		{
			if (lasertime == -1)
			{
				lasertime = MyAI.ProjectileVelocity;
			}

			if (lasertime > 0)
			{
				lasertime--;
				MyAI.TimeSeen--;
				gameObject.transform.rotation = Quaternion.Euler(0, 0, gameObject.transform.rotation.eulerAngles.z + MyAI.AngularOffset);
				RaycastHit2D KillBeam = Physics2D.Raycast(MyAI.FirePosition.position, transform.right, Mathf.Infinity, collision);
				RaycastHit2D AimBeam = Physics2D.Raycast(MyAI.FirePosition.position, transform.right, Mathf.Infinity, noplayercollision);
				if (AimBeam.collider != null)
				{
					hit.position = AimBeam.point;
				}
				else
				{
					hit.position = new Vector2(Mathf.Cos(Mathf.Deg2Rad * gameObject.transform.rotation.eulerAngles.z) * 200 + gameObject.transform.position.x, Mathf.Sin(Mathf.Deg2Rad * gameObject.transform.rotation.eulerAngles.z) * 200 + gameObject.transform.position.y);
				}
				line.SetPosition(1, hit.position);

				gameObject.transform.rotation = Quaternion.Euler(0, 0, gameObject.transform.rotation.eulerAngles.z - MyAI.AngularOffset);
			} 
			else if (lasertime == 0)
			{
				lasertime = -1;
			}

		} 
		else
		{
			lasertime = -1;
		}

	}
}*/
