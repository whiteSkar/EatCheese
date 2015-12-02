using UnityEngine;
using System.Collections.Generic;

public class RandomMovement : MovementBase
{
    private static readonly List<Vector2> DIRS = new List<Vector2>() {new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1)};
    private List<Vector2> dirs;
    private Vector2 dest;
    private GameManager gameManager;
    
    
    public override void Reset()
    {
        base.Reset();
        
        dest = transform.position;

        ResetDirs();
    }
    
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        dirs = new List<Vector2>();
        
        Reset();
    }

    void OnCollisionStay2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.layer == LayerMask.NameToLayer("Blocking"))
        {
            dest = transform.position;
        }
    }

    void FixedUpdate()
    {
        if (gameManager.state != GameManager.GameState.Playing) return;
        
        Vector2 curPos = transform.position;
        if (curPos == dest)
        {
            int index = 0;
            Vector2 newDir = dirs[index];
            dest = curPos + newDir;
            
            dirs.RemoveAt(index);
            if (dirs.Count == 0)
                ResetDirs();
        }

        Move(dest - curPos);
    }
    
    void ResetDirs()
    {
        dirs.Clear();
        
        int minStep = 3;
        int maxStep = 12;
        
        List<int> indicies = new List<int>();
        for (int i = 0; i < DIRS.Count; i++)
            indicies.Add(Random.Range(0, DIRS.Count));

        for (int k = 0; k < indicies.Count; k++)
        {
            int count = Random.Range(minStep, maxStep);
            for (int i = 0; i < count; i++)
                dirs.Add(DIRS[indicies[k]]);
        }
    }
}
