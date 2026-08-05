using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏结束类型枚举
/// </summary>
public enum GameOverType
{
    Success,
    Timeout,
}

/// <summary>
/// 游戏结果数据
/// </summary>
public class GameResult
{
    public GameOverType resultType;
    /// <summary>
    /// 花费时间
    /// </summary>
    public float spendTime;
}
