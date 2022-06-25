using System;
using UnityEngine;

public class ClickManager
{
    public static ClickManager Instance = new ClickManager();
    public delegate void OnClickCallback(GridPositionHandle position);
    private ClickManager()
    { 

    }

    public event OnClickCallback OnClick;
    public void Click(GridPositionHandle position) => OnClick?.Invoke(position);
}
