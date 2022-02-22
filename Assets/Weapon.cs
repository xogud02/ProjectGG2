using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;

public class Weapon : MonoBehaviour {
    [SerializeField] private SpriteRenderer _sprite;

    private void Start() {
        var collider = gameObject.AddComponent<EdgeCollider2D>();
        gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        var center = _sprite.sprite.bounds.center;
        collider.points = new Vector2[] { center, center * 2 };
        collider.isTrigger = true;
    }

    private float _attackSpeed;
    public float AttackSpeed {
        get => _attackSpeed;
        set {
            _attackSpeed = value;
            _attackCooldown = 1 / _attackSpeed;
        }
    }

    private float _attackCooldown = 1f;

    public async UniTask Attack(Unit target) {
        transform.DORotate(new Vector3(0, 0, 360), _attackCooldown, RotateMode.FastBeyond360);
        await UniTask.Delay(TimeSpan.FromSeconds(_attackCooldown));
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log(collision);
    }

    private void OnCollisionStay2D(Collision2D collision) {
        Debug.Log(collision);
    }
}
