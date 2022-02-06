using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;

    public async UniTask Attack(Unit target) {
        transform.DORotate(new Vector3(0, 0, 360), 1, RotateMode.FastBeyond360);
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
    }
}
