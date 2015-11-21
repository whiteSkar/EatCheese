using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;

    void Move(Vector2 dir)
    {
        Vector2 curP = transform.position;
        Vector2 newP = Vector2.MoveTowards(curP, curP + dir, moveSpeed * Time.deltaTime);
        
        GetComponent<Rigidbody2D>().MovePosition(newP);
        
        Animator animator = GetComponent<Animator>();
        animator.SetFloat("DirX", dir.x);
        animator.SetFloat("DirY", dir.y);
    }
    
    bool CanMove(Vector2 dir)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        
        var hitLayer = hit.collider.gameObject.layer;
        var blockingLayer = LayerMask.NameToLayer("Blocking");
        var playerBlockingLayer = LayerMask.NameToLayer("PlayerBlocking");
        return hitLayer != blockingLayer && hitLayer != playerBlockingLayer;
    }

    void Start()
    {

    }


    void FixedUpdate()
    {
        int horizontal = (int) Input.GetAxis("Horizontal");
        int vertical = (int) Input.GetAxis("Vertical");
        
        if (horizontal != 0)
            vertical = 0;
            
        Vector2 dir = new Vector2(horizontal, vertical);
        
        if (CanMove(dir))
            Move(dir);
    }
}
