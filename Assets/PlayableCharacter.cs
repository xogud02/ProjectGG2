using Cysharp.Threading.Tasks;

public class PlayableCharacter : Unit
{

    public async UniTask AttackTarget() {
        while (true) {
            await UniTask.Yield();
            if (target == null) {
                continue;
            }

            if (IsInRange(target)) {
                await target.Hit(this);
            }
        }
    }
}
