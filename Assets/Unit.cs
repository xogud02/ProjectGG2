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

    private Animator animator;
    public void Start() {
        animator = GetComponent<Animator>();
        if (Tiles.Initialized) {
            transform.localScale *= Tiles.ScaleFactor;
        } else {
            Tiles.OnInit += () => transform.localScale *= Tiles.ScaleFactor;
        }

        Tiles.OnClick = tile => {
            if (tile.tileType == Tile.TileType.Block) {
                return;
            }
            Move(tile.transform.position);
        };

        GetComponent<SpriteRenderer>().sprite.texture.filterMode = FilterMode.Point;//TODO inspector settings not working!!!!
    }

    public void Move(Vector2 v) {
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
        DOTween.To(() => (Vector2)transform.position, vec => transform.position = vec, v, time).SetEase(Ease.Linear);
    }
}
