using UnityEngine;
using System.Collections.Generic;

// refactor with PlayerController
public class RandomMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    
    
    private static readonly List<Vector2> DIRS = new List<Vector2>() {new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1)};
    private List<Vector2> dirs;
    private Vector2 dest;
    
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
        return hit.collider.gameObject.layer != LayerMask.NameToLayer("Blocking");
    }

    void Start()
    {
        dest = transform.position;
        
        dirs = new List<Vector2>();
        ResetDirs();
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
