using System.Collections;
using System.Linq;
using UnityEngine;

public class Monster : Unit {
    [SerializeField] private Unit target;
    private Coroutine waitForMove;
    private Coroutine findTarget;

    public new void Start() {
        base.Start();
        waitForMove = StartCoroutine(WaitAndMove());
        AttackRange = 2;
    }

    private IEnumerator WaitAndMove() {
        while (true) {
            yield return new WaitForSeconds(1);
            if (target == null && findTarget == null) {
                findTarget = StartCoroutine(FindTarget());
            }

            var nextPosition = GetNextPosition();

            if (GridField.IsMovable(nextPosition)) {
                SetPath(nextPosition);
            }

            if (target != null && IsInRange(target) && target.Hit(this)) {
                target = null;
            }

            while (IsMoving) {
                yield return null;
            }
        }
    }

    private Vector2Int GetNextPosition() {
        var nextPosition = CurrentPosition;

        if (target == null) {
            var nextIndex = Random.Range(0, AStar.dx.Length);
            nextPosition.x += AStar.dx[nextIndex];
            nextPosition.y += AStar.dy[nextIndex];
            return nextPosition;
        }

        var path = AStar.Find(CurrentPosition, target.CurrentPosition);
        if (path.Count == 0) {
            return CurrentPosition;
        }

        return path.Peek();
    }

    private IEnumerator FindTarget() {
        while (true) {
            yield return null;
            if (target != null) {
                continue;
            }
            target = GridField.GetOccupied(CurrentPosition, 2).Where(unit => unit != this).FirstOrDefault();
        }
    }

}
