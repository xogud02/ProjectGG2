using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;

    public async UniTask Attack(Unit target) {
        await UniTask.Delay(TimeSpan.FromSeconds(0.5));
    }
}
