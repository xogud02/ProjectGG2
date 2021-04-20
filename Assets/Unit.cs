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

public static class Move {
    public static Action<Vector2> OnMove;
    public static void Foo(this Vector2 position) => OnMove?.Invoke(position);
    public static void Foo(this Vector3 position) => OnMove?.Invoke(position);
    public static Vector3 STW(this Vector3 position) => Camera.main.ScreenToWorldPoint(position);
    public static Vector3 STW(this Vector2 position) => Camera.main.ScreenToWorldPoint(position);
}

public class Unit : MonoBehaviour {

    private Animator animator;
    public void Start() {
        animator = GetComponent<Animator>();
        Move.OnMove = v => DOTween.To(() => (Vector2)transform.position, vec => transform.position = vec, v, 10);
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            animator.SetTrigger("Up");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            animator.SetTrigger("Down");
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            animator.SetTrigger("Left");
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            animator.SetTrigger("Right");
        }
        if (Input.touchCount != 0) {
            Input.touches[0].position.STW().Foo();
        } else if (Input.GetMouseButtonUp(0)) {
            Input.mousePosition.STW().Foo();
        }
    }

}
