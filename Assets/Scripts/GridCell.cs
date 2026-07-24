using UnityEngine;
/// <summary>
/// 格子信息
/// </summary>
public class GridCell
{
    // 坐标
    public Vector2Int pos;
    // 数据
    public ItemData item;
    // 表现
    public BlockView view;
    // 消除特效
    public DestroyEffect desEff;
    // 是否空白
    public bool IsEmpty => item == null;
}
