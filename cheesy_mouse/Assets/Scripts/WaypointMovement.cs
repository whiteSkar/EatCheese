using UnityEngine;
using System.Collections;

public class WaypointMovement : MovementBase
{
    public Transform[] waypoints;
    
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
            
        Vector2 dir = waypoints[curIndex].position - transform.position;
        Move(dir);
    }
}
