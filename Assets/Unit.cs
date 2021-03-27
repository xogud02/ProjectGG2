using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction {
    Up,
    Down,
    Left,
    Right
}

public class Unit : MonoBehaviour
{
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
    }
}
