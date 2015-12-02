using UnityEngine;
using System.Collections;

public class PlayerMovement : MovementBase
{
    public float moveSpeed = 10f;
    
    private GameManager gameManager;
    private AudioSource audioSrc;
    
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
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Cat"))
        {
            audioSrc.Play();
            transform.position = new Vector3(-100, -100, -100);

            gameManager.state = GameManager.GameState.Lost;
        }
    }
    
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioSrc = GetComponent<AudioSource>();
    }
    
    void FixedUpdate()
    {
        if (gameManager.state != GameManager.GameState.Playing) return;
        
        int horizontal = (int) Input.GetAxisRaw("Horizontal");
        int vertical = (int) Input.GetAxisRaw("Vertical");
        
        if (horizontal != 0)
            vertical = 0;
            
        Vector2 dir = new Vector2(horizontal, vertical);
        
        if (CanMove(dir))
            Move(dir);
    }
}
