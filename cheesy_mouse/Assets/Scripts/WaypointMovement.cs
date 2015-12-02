using UnityEngine;
using System.Collections;

public class WaypointMovement : MovementBase
{
    public Transform[] waypoints;
    public float moveSpeed = 10f;
    
    
    private int curIndex;
    private GameManager gameManager;
    
    public override void Reset()
    {
        base.Reset();
        
        curIndex = 0;
    }
    
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        Reset();
    }
    
    void FixedUpdate()
    {
        if (gameManager.state != GameManager.GameState.Playing) return;
        
        if (transform.position == waypoints[curIndex].position)
            curIndex = (curIndex + 1) % waypoints.Length;
            
        Vector2 p = Vector2.MoveTowards(transform.position, waypoints[curIndex].position, moveSpeed * Time.deltaTime);
        GetComponent<Rigidbody2D>().MovePosition(p);

        Vector2 dir = waypoints[curIndex].position - transform.position;
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);
    }
}
