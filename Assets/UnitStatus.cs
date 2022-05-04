using System;
using UnityEngine;
/// <summary>
/// 크게 rgb 3개스탯이 존재하며 아이템과 레벨에따라 증가, 오르는데에는 개체값 존재
/// 각각 3개스탯 수치에 종속된 스탯들 있음
/// </summary>
public class UnitStatus
{
    public enum ColorType
    {
        None,
        Red,
        Green,
        Blue,
    }

    public enum ValueType
    {
        None,
        Hp,
        Exp,
        Attack,
        AttackRange,
    }

    public enum QuantityType
    {
        None,
        Decreased,
        Increased,
        Remain,
        Max,
    }

    public int MaxHp => Red * 10;
    private int hp = 10;
    public int Hp
    {
        get => hp;
        set
        {
            var before = hp;
            hp = Mathf.Clamp(value, 0, MaxHp);
            onHpChange?.Invoke(before, hp);
        }
    }

    public Action<int, int> onHpChange;

    public int Red => Level;
    public int Green => Level;
    public int Blue => Level;

    public int RewardExp => 15;
    private int _exp;
    public int Exp
    {
        get => _exp;
        set
        {
            _exp += value;
            Debug.Log($"exp increased {value} => current : {_exp}/{MaxExp}");
            while (_exp >= MaxExp)
            {
                _exp -= MaxExp;
                ++Level;
            }
        }
    }
    public int MaxExp => Level * 10;
    private int _level = 1;
    public int Level
    {
        get => _level;
        set
        {
            var before = _level;
            _level = value;
            onLevelChange?.Invoke(before, _level);
        }
    }

    public Action<int, int> onLevelChange;

    public int Attack => Red;
    protected int AttackRange { get; set; }

    public float Speed => Green;

    public UnitStatus(Unit owner)
    {
    }
}
