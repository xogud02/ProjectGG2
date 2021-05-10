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
        if (Tiles.Initialized) {
            transform.localScale *= Tiles.ScaleFactor;
        } else {
            Tiles.OnInit += () => transform.localScale *= Tiles.ScaleFactor;
        }

        Tiles.OnClick = tile => {
            currentPath = FindPath(tile.GridPosition);
            if (currentPath.Count != 0) {
                Move(currentPath.Dequeue());
            }
        };

        GetComponent<SpriteRenderer>().sprite.texture.filterMode = FilterMode.Point;//TODO inspector settings not working!!!!
    }

    private Queue<Vector2Int> FindPath(Vector2Int dest) {
        return new Queue<Vector2Int>();//TODO Find
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
        moving = DOTween.To(() => (Vector2)transform.position, vec => transform.position = vec, v, time).SetEase(Ease.Linear);
        moving.onComplete = () => {
            if (currentPath.Count != 0 && moving == null) {
                var next = currentPath.Dequeue();
                Move(next);
            }
        };
    }
}
