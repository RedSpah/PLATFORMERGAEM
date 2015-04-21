using UnityEngine;
using System.Collections;

public class S_Rocket : MonoBehaviour {

    public Transform Target;
    private float acceleration = 30;
    private float initialvelocity;
    private float TurnSpeed = 0.01f;
    [SerializeField]
    private Rigidbody2D selfrigid;

	void Start () {
	
	}
	
	
	void FixedUpdate () {
        Vector3 lookDirection = Target.transform.position - gameObject.transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, targetRotation, TurnSpeed);
        selfrigid.AddForce(new Vector2(Mathf.Sin(gameObject.transform.rotation.eulerAngles.z) * acceleration, Mathf.Cos(gameObject.transform.rotation.eulerAngles.z) * acceleration));
	
	}

    void SetTarget(Transform g)
    {
        Target = g;
    }
}
