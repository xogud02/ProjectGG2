using System;
using System.Collections.Generic;
using UnityEngine;

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

    public class StatValue
    {
        private readonly ValueType _valueType;
        private readonly QuantityType _quantityType;
        private int _value;
    }

    private readonly Dictionary<(ValueType, QuantityType), int> _statValues = new Dictionary<(ValueType, QuantityType), int>();


    private readonly Unit _owner;
    public int MaxHp => Level * 10;
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

    private int attack;
    protected int AttackRange { get; set; }

    private float speed = 5;

    public UnitStatus(Unit owner){
        _owner = owner;
    }
}
