using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction {
    Up,
    Down,
    Left,
    Right
}

public static class MoveExtension {
    public static Vector3 STW(this Vector3 position) => Camera.main.ScreenToWorldPoint(position);
    public static Vector3 STW(this Vector2 position) => Camera.main.ScreenToWorldPoint(position);
    public static Vector2 Convert(this Vector3 v) => v;
    public static Vector3 Convert(this Vector2 v) => v;
}

public class Unit : MonoBehaviour {
    private float speed = 5;
    private readonly int Right = Animator.StringToHash("Right");
    private readonly int Left = Animator.StringToHash("Left");
    private readonly int Up = Animator.StringToHash("Up");
    private readonly int Down = Animator.StringToHash("Down");
    private Tween moving;

    private Animator animator;
    private Queue<Vector2Int> currentPath = new Queue<Vector2Int>();
    private Vector2Int CurrentPosition;

    public void Start() {
        animator = GetComponent<Animator>();
        if (GridObject.Initialized) {
            transform.localScale *= GridObject.ScaleFactor;
        } else {
            GridObject.OnInit += () => transform.localScale *= GridObject.ScaleFactor;
        }

        GridField.OnClick = to => {
            currentPath = AStar.Find(CurrentPosition, to);
        };

        GetComponent<SpriteRenderer>().sprite.texture.filterMode = FilterMode.Point;//TODO inspector settings not working!!!!
    }

    public void Stop() {
        if (currentPath.Count == 0) {
            return;
        }

        var lastPath = new Queue<Vector2Int>();
        lastPath.Enqueue(currentPath.Dequeue());
        currentPath = lastPath;
    }

    public void Move(Vector2Int v) {
        var direction = v - transform.position.Convert();
        var angle = Vector2.SignedAngle(Vector2.right, direction);
        var absAngle = Mathf.Abs(angle);
        if (absAngle < 45f) {
            animator.SetTrigger(Right);
        } else if (absAngle > 135f) {
            animator.SetTrigger(Left);
        } else if (angle > 0) {
            animator.SetTrigger(Up);
        } else {
            animator.SetTrigger(Down);
        }

        var dist = direction.magnitude;
        var time = dist / speed;
        moving = DOTween.To(() => (Vector2)transform.position, vec => transform.position = vec, GridField.Convert(v), time).SetEase(Ease.Linear);
        moving.onComplete = () => {
            if (currentPath.Count != 0) {
                moving = null;
                var next = currentPath.Dequeue();
                Move(next);
            }
        };
    }
}
