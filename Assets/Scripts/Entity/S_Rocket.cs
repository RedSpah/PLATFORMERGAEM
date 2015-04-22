using UnityEngine;
using System.Collections;

public class S_Rocket : MonoBehaviour {

    public Transform Target;
    [SerializeField]
    private float acceleration = 300;
    private float initialvelocity;
    [SerializeField]
    private float topvelx = 24;
    [SerializeField]
    private float topvely = 24;
    [SerializeField]
    private float TurnSpeed = 0.66f;
    [SerializeField]
    private Rigidbody2D selfrigid;
    [SerializeField]
    private BoxCollider2D col;
	void Start () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
    }
	
	void FixedUpdate () {
        Vector3 lookDirection = Target.transform.position - gameObject.transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, targetRotation, TurnSpeed);
        float anDif = Vector2.Angle(gameObject.transform.position, Target.transform.position);
        selfrigid.AddForce(new Vector2(Mathf.Cos(Mathf.Deg2Rad * gameObject.transform.rotation.eulerAngles.z) * acceleration, Mathf.Sin(Mathf.Deg2Rad * gameObject.transform.rotation.eulerAngles.z) * acceleration));
        float x = (selfrigid.velocity.x > 0) ? (topvelx < selfrigid.velocity.x) ? topvelx : selfrigid.velocity.x : (-topvelx > selfrigid.velocity.x) ? -topvelx : selfrigid.velocity.x ;
        float y = (selfrigid.velocity.y > 0) ? (topvely < selfrigid.velocity.y) ? topvely : selfrigid.velocity.y : (-topvely > selfrigid.velocity.y) ? -topvely : selfrigid.velocity.y ;

        selfrigid.velocity = new Vector2(x,y);
	}

    void SetTarget(Transform g)
    {
        Target = g;
    }
}
