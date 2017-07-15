using UnityEngine;

public class MovingObject : MonoBehaviour {

    public Vector3 DeltaPostion { get; private set; }

    private Vector3 basePos;
    private Vector3 target;
    private Rigidbody body;

    [SerializeField]
    private Vector3 VectorToMove;

    [SerializeField]
    private float Speed = 2;

    [SerializeField]
    private bool RelativePosition = true;

    void Start () {
        basePos = transform.position;
        if (RelativePosition)
            VectorToMove += basePos;

        //target = basePos;
        target = VectorToMove;
        body = GetComponent<Rigidbody>();
    }
    
    void FixedUpdate () {
        var newPos = Vector3.MoveTowards(transform.position, target, Time.deltaTime * Speed);
        DeltaPostion = newPos - transform.position;

        if (transform.position != target)
        {
            if (body != null)
                body.MovePosition(newPos);
            else
                transform.position = newPos;
        }
        else
        {
            if (target == basePos)
                target = VectorToMove;
            else
                target = basePos;
            //MovingSound.Stop();
        }
    }

    public Vector3 getBasePos() { return basePos; }
    public Vector3 getMoveToPos() { return VectorToMove; }
}
