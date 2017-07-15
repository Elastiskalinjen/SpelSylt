using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    [SerializeField]
    private float Speed = 2;

    private Vector3 A;
    private Vector3 B;

    private Vector3 Target;

    private Rigidbody Body;

    // Use this for initialization
    void Start () {
        this.A = transform.GetChild(0).gameObject.transform.position;
        this.B = transform.GetChild(1).gameObject.transform.position;

        Target = A;

        Body = GetComponent<Rigidbody>();
        Body.MovePosition(Target);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Body.MovePosition(Vector3.MoveTowards(transform.position, Target, Time.deltaTime * Speed ));

        if (Vector3.Distance(transform.position, Target) < 0.1f)
        {
            if (Target == A)
                Target = B;
            else
                Target = A;
        }
	}
}
