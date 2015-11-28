using UnityEngine;
using System.Collections;

public class WaypointMovement : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 10f;
    
    
    private int curIndex;
    private GameManager gameManager;
    
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
