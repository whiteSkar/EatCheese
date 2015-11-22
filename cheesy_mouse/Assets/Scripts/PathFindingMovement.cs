using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class PathFindingMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public GameObject map;
    public Transform player;
    
    
    private static readonly List<Vector2> DIRS = new List<Vector2>() {new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1)};
    // static possibly
    private IDictionary<Vector2, int> validNodes = new Dictionary<Vector2, int>();  // int is for heuristic value
    private Vector2 dest;
    
    [System.Serializable]
    private class AStarScore
    {
        public int cost;
        public int h;
        
        public AStarScore(int cost, int h)
        {
            this.cost = cost;
            this.h = h;
        }
    }
    
    
    void Move(Vector2 dir)
    {
        Vector2 curP = transform.position;
        Vector2 newP = Vector2.MoveTowards(curP, curP + dir, moveSpeed * Time.deltaTime);
        
        GetComponent<Rigidbody2D>().MovePosition(newP);
        
        Animator animator = GetComponent<Animator>();
        animator.SetFloat("DirX", dir.x);
        animator.SetFloat("DirY", dir.y);
    }


    void FixedUpdate()
    {
        Vector2 curPos = transform.position;
        print(curPos);

        if (curPos == dest)
        {
            ComputeAstarH();
            
            var visitedNodes = new HashSet<Vector2>();
            var willVisitNodes = new Dictionary<Vector2, AStarScore>();
            willVisitNodes.Add(curPos, new AStarScore(0, validNodes[new Vector2((int) curPos.x, (int) curPos.y)])); // starting pos is maybe a float
            
            var path = new Stack<Vector2>();
            while (willVisitNodes.Count != 0)
            {
                // I want a dictionary-like data structure where I can access by the key, the position, but is sorted automatically like a heap by value, the AStarScore.
                var node = willVisitNodes.OrderBy(n => n.Value.cost + n.Value.h).First();
                var pos = node.Key;
                var score = node.Value;
                
                visitedNodes.Add(curPos);
                willVisitNodes.Remove(curPos);
                
                if (node.Key == (Vector2) player.position)
                    break;
                
                var adjacentNodesPos = GetAdjacentValidNodes(curPos);
                for (int i = 0; i < adjacentNodesPos.Count; i++)
                {
                    var adjacentNodePos = adjacentNodesPos[i];
                    if (visitedNodes.Contains(adjacentNodePos))
                        continue;
                    
                    int newCost = score.cost + 1;
                    if (!willVisitNodes.ContainsKey(adjacentNodePos))
                    {
                        willVisitNodes.Add(adjacentNodePos, new AStarScore(newCost, validNodes[adjacentNodePos]));
                        path.Push(adjacentNodePos);
                    }
                    else
                    {
                        var adjacentNode = willVisitNodes[adjacentNodePos];
                        if (newCost < adjacentNode.cost)
                        {
                            adjacentNode.cost = newCost;
                            path.Push(adjacentNodePos);
                        }
                    }
                }
                
                if (path.Count > 0)
                    path.Pop();
            }
            
            dest = path.FirstOrDefault();
        }

        Move(dest - curPos);
    }
    
    void ComputeAstarH()
    {
        Vector2 targetPos = player.position;
        
        foreach (var nodePos in validNodes.Keys.ToList())    // check if this works without ToList
        {
            var deltaDist = targetPos - nodePos;
            validNodes[nodePos] = (int) Mathf.Abs(deltaDist.x) + (int) Mathf.Abs(deltaDist.y);
        }
    }
    
    IList<Vector2> GetAdjacentValidNodes(Vector2 pos)
    {
        List<Vector2> nodes = new List<Vector2>();
        
        for (int i = 0; i < DIRS.Count; i++)
        {
            var adjacentNode = pos + DIRS[i];
            if (validNodes.ContainsKey(adjacentNode))
            {
                nodes.Add(adjacentNode);
            }
        }
        
        return nodes;
    }
    
    void OnCollisionStay2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.layer == LayerMask.NameToLayer("Blocking"))
        {
            dest = transform.position;
        }
    }
    
    // Refactor with CheeseManager code.
    void Start()
    {
        var mapBlocks = map.GetComponents<BoxCollider2D>();
        
        // 2, 31, 28 are all hacky way for getting map size and offset.
        int mapOffset = 2;
        for (int i = (int) map.transform.position.x + mapOffset; i < 31; i++)
        {
            for (int j = (int) map.transform.position.y + mapOffset; j < 28; j++)
            {
                bool isOnBlock = false;
                for (int k = 0; k < mapBlocks.Length; k++)
                {
                    var point = new Vector2(j, i);
                    if (IsIntersect(mapBlocks[k], point))
                    {
                        isOnBlock = true;
                        break;
                    }
                }

                if (!isOnBlock)
                    validNodes.Add(new Vector2(j, i), 0);
            }
        }
        
        dest = transform.position;
    }
    
    bool IsIntersect(BoxCollider2D collider, Vector2 point)
    {
        var offset = collider.offset;
        var halfX = collider.size.x / 2.0f;
        var halfY = collider.size.y / 2.0f;
        
        if (point.x >= offset.x - halfX && 
            point.x <= offset.x + halfX &&
            point.y >= offset.y - halfY && 
            point.y <= offset.y + halfY)
        {
            return true;
        }
        return false;
    }
}
