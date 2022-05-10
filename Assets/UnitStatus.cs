using System;
using System.Collections.Generic;
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
        Level,
    }

    public enum QuantityType
    {
        None,
        Decreased,
        Increased,
        Remain,
        Max,
    }

    public class StatKeyType : IEquatable<StatKeyType>
    {
        private ValueType _valueType;

        public StatKeyType(ValueType valueType) => _valueType = valueType;

        public bool Equals(StatKeyType other) => other != null && other._valueType == _valueType;

        public override int GetHashCode() => ((int)_valueType).GetHashCode();
    }

    public class StatChangeEvent
    {
        public readonly StatKeyType _statKeyType;
        public readonly int Before;
        public readonly int After;
        public StatChangeEvent(StatKeyType statKeyType, int before, int after)
        {
            _statKeyType = statKeyType;
            Before = before;
            After = after;
        }
    }

    public delegate void OnStatChange(StatChangeEvent _);

    private Dictionary<StatKeyType, OnStatChange> _statChangeEvents = new Dictionary<StatKeyType, OnStatChange>();

    public void AddListener(StatKeyType key, OnStatChange onStatChange)
    {
        if (_statChangeEvents.ContainsKey(key))
        {
            _statChangeEvents[key] += onStatChange;
            return;
        }

        _statChangeEvents[key] = onStatChange;
    }

    public void RemoveListener(StatKeyType key, OnStatChange onStatChange)
    {
        if (_statChangeEvents.ContainsKey(key))
        {
            _statChangeEvents[key] -= onStatChange;
        }
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
            var key = new StatKeyType(ValueType.Hp);
            if (_statChangeEvents.TryGetValue(key, out var onChange))
            {
                onChange?.Invoke(new StatChangeEvent(key, before, hp));
            }
        }
    }

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
            var key = new StatKeyType(ValueType.Level);
            if(_statChangeEvents.TryGetValue(key, out var onStatChange))
            {
                onStatChange?.Invoke(new StatChangeEvent(key, before, _level));
            }
        }
    }

    public int Attack => Red;
    protected int AttackRange { get; set; }

    public float Speed => Green;

    public UnitStatus(Unit owner)
    {
    }
}
