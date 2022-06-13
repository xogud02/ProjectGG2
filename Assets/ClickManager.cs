using System;
using UnityEngine;

public class ClickManager
{
    public static ClickManager Instance = new ClickManager();
    public delegate void OnClickCallback(Vector2Int position);
    private ClickManager()
    { 

    }

    public event OnClickCallback OnClick;
    public void Click(Vector2Int position) => OnClick?.Invoke(position);//TODO to many v2i?
}
