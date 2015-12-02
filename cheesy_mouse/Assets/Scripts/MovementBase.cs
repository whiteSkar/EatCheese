using UnityEngine;
using System.Collections;

public abstract class MovementBase : MonoBehaviour
{
    public float moveSpeed;
    
    protected Vector3 initialPos;
    protected Quaternion initialRotation;
    
    public virtual void Reset()
    {
        transform.position = initialPos;
        transform.rotation = initialRotation;
        gameObject.GetComponent<Animator>().Play("PlayerRight");
    }
    
    protected virtual void Move(Vector2 dir)
    {
        Vector2 curP = transform.position;
        Vector2 newP = Vector2.MoveTowards(curP, curP + dir, moveSpeed * Time.deltaTime);
        
        GetComponent<Rigidbody2D>().MovePosition(newP);
        
        Animator animator = GetComponent<Animator>();
        animator.SetFloat("DirX", dir.x);
        animator.SetFloat("DirY", dir.y);
    }
    
    void Awake()
    {
        initialPos = transform.position;
        initialRotation = transform.rotation;
    }
}
