using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction {
    Up,
    Down,
    Left,
    Right
}

public static class Extension {
    public static void Foo(this Vector2 position) => Debug.Log(position);
    public static void Foo(this Vector3 position) => Debug.Log(position);
}

public class Unit : MonoBehaviour {
    private Animator animator;
    public void Start() {
        animator = GetComponent<Animator>();
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
            Input.touches[0].position.Foo();
        } else if (Input.GetMouseButtonUp(0)) {
            Input.mousePosition.Foo();
        }
    }

}
