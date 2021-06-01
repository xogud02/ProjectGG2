using System.Collections.Generic;
using UnityEngine;

public static class AStar {
    private struct Node {
        public readonly float distance;
        public float sum;
        public Node(float distance, float sum = 0) {
            this.distance = distance;
            this.sum = sum;
        }
    }


    public static Queue<Vector2Int> Find(Vector2Int from, Vector2Int to) {
        var start = new Node((to - from).magnitude);
        var open = new SortedSet<Node>(Comparer<Node>.Create((node1, node2) => (int)(node1.sum - node2.sum))) { start };

        var ret = new Queue<Vector2Int>();
        while (open.Count > 0) {

        }
        return ret;
    }
}
