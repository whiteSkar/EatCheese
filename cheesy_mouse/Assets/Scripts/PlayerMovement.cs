using UnityEngine;
using System.Collections;
using CnControls;

public class PlayerMovement : MovementBase
{    
    private GameManager gameManager;
    private AudioSource audioSrc;
    
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

        int horizontal = (int) CnInputManager.GetAxisRaw("Horizontal");
        int vertical = (int) CnInputManager.GetAxisRaw("Vertical");
        
        if (horizontal != 0)
            vertical = 0;
            
        Vector2 dir = new Vector2(horizontal, vertical);
        
        if (CanMove(dir))
            Move(dir);
    }
}
