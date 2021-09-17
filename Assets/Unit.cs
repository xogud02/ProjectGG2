﻿using DG.Tweening;
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
    private int hp;
    private int maxHp;
    private int attack;

    private float speed = 5;
    private readonly int Right = Animator.StringToHash("Right");
    private readonly int Left = Animator.StringToHash("Left");
    private readonly int Up = Animator.StringToHash("Up");
    private readonly int Down = Animator.StringToHash("Down");
    private int currentDirection;
    private Tween moving;

    private Animator animator;
    private Queue<Vector2Int> currentPath = new Queue<Vector2Int>();
    private Vector2Int CurrentPosition;

    public void Start() {
        animator = GetComponent<Animator>();
        currentDirection = Down;
        if (GridObject.Initialized) {
            transform.localScale *= GridObject.ScaleFactor;
        } else {
            GridObject.OnInit += () => transform.localScale *= GridObject.ScaleFactor;
        }

        GetComponent<SpriteRenderer>().sprite.texture.filterMode = FilterMode.Point;//TODO inspector settings not working!!!!
    }

    public void Stop() {
        if (IsMoving == false) {
            return;
        }

        EmptyPath();
    }

    public void SetPath(Vector2Int v) {
        EmptyPath();
        var newPath = AStar.Find(currentPath.Count > 0 ? currentPath.Peek() : CurrentPosition, v);
        var wasMoving = IsMoving;
        while (newPath.Count > 0) {
            currentPath.Enqueue(newPath.Pop());
        }

        if (wasMoving == false && currentPath.Count > 0) {
            Move(currentPath.Peek());
        }
    }

    private void EmptyPath() {
        var lastPath = new Queue<Vector2Int>();
        if (IsMoving && currentPath.Count > 0) {
            lastPath.Enqueue(currentPath.Dequeue());
        }
        currentPath = lastPath;
    }

    public bool IsMoving => currentPath.Count > 0 || (moving?.active ?? false);

    private int GetPreferDirection(float angle) {
        var absAngle = Mathf.Abs(angle);
        if (absAngle < 45f) {
            return Right;
        } else if (absAngle > 135f) {
            return Left;
        } else if (angle > 0) {
            return Up;
        } else {
            return Down;
        }
    }

    public void MoveImmidiately(Vector2Int v) {
        transform.position = GridField.Convert(v);
        CurrentPosition = v;
    }

    public void Move(Vector2Int v) {
        var dest = GridField.Convert(v);
        var direction = dest - transform.position.Convert();
        var preferDirection = GetPreferDirection(Vector2.SignedAngle(Vector2.right, direction));
        if(preferDirection != currentDirection) {
            animator.SetTrigger(preferDirection);
            currentDirection = preferDirection;
        }

        var dist = direction.magnitude;
        var time = dist / speed;
        moving = DOTween.To(() => (Vector2)transform.position, vec => transform.position = vec, dest, time).SetEase(Ease.Linear);

        moving.onComplete = () => {
            moving = null;

            if (currentPath.Count == 0) {
                return;
            }

            CurrentPosition = currentPath.Dequeue();

            if (currentPath.Count != 0) {
                Move(currentPath.Peek());
            }
        };
    }
}
