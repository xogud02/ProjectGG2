using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Linq;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class Monster : Unit {
    private CancellationTokenSource _source = new CancellationTokenSource();
    private bool _directionRight;
    private SpriteRenderer spriteRenderer;
    
    public new void Start() {
        base.Start();
        WaitAndMove().Forget();
        FindTarget().Forget();
        AttackRange = 2;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Die() {
        _source.Cancel();
        base.Die();
    }

    protected override void OnMoveSingle(Vector2 direction) {
        var x = direction.x;
        var nextDirectionRight = x > 0;
        if (Mathf.Abs(x) <= float.Epsilon || _directionRight == nextDirectionRight) {
            return;
        }

        _directionRight = nextDirectionRight;
        spriteRenderer.flipX = _directionRight;
    }

    private async UniTask WaitAndMove() {
        while (true) {
            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: _source.Token);
            if (_source.IsCancellationRequested) {
                return;
            }

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
