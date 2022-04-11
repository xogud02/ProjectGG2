using System;
using UnityEngine;

public class UnitStatus
{
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
            if (_level > before)
            {
                Hp = MaxHp;
                //hpBar.Init(Hp, hpBar.ShowFrame);
                Debug.Log("level up to " + _level);
            }
        }
    }

    protected void KillLogic(Unit other)
    {
        if (other == _owner)
        {
            return;
        }
        Debug.Log($"{other} killed by {this} , reward : {other.RewardExp}");
        Exp += other.RewardExp;
    }

    private int attack;
    protected int AttackRange { get; set; }

    //private GUIBar hpBar;

    private float speed = 5;

    public UnitStatus(Unit owner){
        _owner = owner;
    }
}
