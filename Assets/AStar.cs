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
        return new Queue<Vector2Int>();
    }
}
