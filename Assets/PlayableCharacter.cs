using Cysharp.Threading.Tasks;

public class PlayableCharacter : Unit {

    public async UniTask AttackTarget() {
        AttackRange = 2;//TODO
        while (true) {
            await UniTask.Yield();
            if (target == null) {
                continue;
            }

            if (target != this && IsInRange(target)) {
                await _weapon.Attack(target);
                target.Hit(this);
            }
        }
    }
}
