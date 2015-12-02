using UnityEngine;
using System.Collections;

public abstract class MovementBase : MonoBehaviour
{
    protected Vector3 initialPos;
    protected Quaternion initialRotation;
    
    public virtual void Reset()
    {
        transform.position = initialPos;
        transform.rotation = initialRotation;
        gameObject.GetComponent<Animator>().Play("PlayerRight");
    }
    
    void Awake()
    {
        initialPos = transform.position;
        initialRotation = transform.rotation;
    }
}
