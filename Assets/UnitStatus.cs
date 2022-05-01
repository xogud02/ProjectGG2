using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ũ�� rgb 3�������� �����ϸ� �����۰� ���������� ����, �����µ����� ��ü�� ����
/// ���� 3������ ��ġ�� ���ӵ� ���ȵ� ����
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

    public class StatType : IEquatable<StatType>
    {
        private readonly ValueType _valueType;
        private readonly QuantityType _quantityType;

        public StatType(ValueType valueType, QuantityType quantityType)
        {
            _valueType = valueType;
            _quantityType = quantityType;
        }

        public bool Equals(StatType other)
        {
            return other != null && _valueType == other._valueType && _quantityType == other._quantityType;
        }
    }

    private readonly IDictionary<StatType, int> statValues = new Dictionary<StatType, int>();
    private int GetOrInitValue(ValueType valueType, QuantityType quantityType)
    {
        var key = new StatType(valueType, quantityType);
        if (statValues.TryGetValue(key, out var ret))
        {
            return ret;
        }

        return statValues[key] = 0;
    }


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

    public UnitStatus(Unit owner)
    {
        _owner = owner;
    }
}
