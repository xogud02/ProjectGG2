using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Monster : Unit {

    public new void Start() {
        base.Start();
        WaitAndMove().Forget();
        FindTarget().Forget();
        AttackRange = 2;
    }

    public override void Die() {
        base.Die();
    }

    private async UniTask WaitAndMove() {
        while (true) {
            await UniTask.Delay(TimeSpan.FromSeconds(1));

            var nextPosition = GetNextPosition();

            if (GridField.IsMovable(nextPosition)) {
                SetPath(nextPosition);
            }

            if (target != null && IsInRange(target) && await target.Hit(this)) {
                target = null;
            }

            while (IsMoving) {
                await UniTask.Yield();
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

    private async UniTask FindTarget() {
        while (true) {
            await UniTask.Yield();
            if (target != null) {
                continue;
            }
            target = GridField.GetOccupied(CurrentPosition, 2).Where(unit => unit != this).FirstOrDefault();
        }
    }

}
