using System.Collections;
using UnityEngine;

public class Monster : Unit
{
    private Coroutine waitForMove;

    public new void Start() {
        base.Start();
        waitForMove = StartCoroutine(WaitAndMove());
    }

    private IEnumerator WaitAndMove() {
        while (true) {
            yield return new WaitForSeconds(1);
            var nextPosition = CurrentPosition;
            var nextIndex = Random.Range(0, AStar.dx.Length);
            nextPosition.x += AStar.dx[nextIndex];
            nextPosition.y += AStar.dy[nextIndex];

            if (GridField.IsInRange(nextPosition)) {
                SetPath(nextPosition);
            }

            while (IsMoving) {
                yield return null;
            }
        }
    }


}
