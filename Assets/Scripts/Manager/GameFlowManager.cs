using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameState
{
    Menu,
    Playing,
    Over,
}

/// <summary>
/// 游戏流程管理器
/// </summary>
public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager instance;

    private GameResult currentResult;

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame()
    {
        ChangeState(GameState.Playing);
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    public void OverGame(GameResult result)
    {
        currentResult = result;
        ChangeState(GameState.Over);
    }

    /// <summary>
    /// 准备游戏
    /// </summary>
    public void ReadyGame()
    {
        ChangeState(GameState.Menu);
    }

    private void ChangeState(GameState state)
    {
        switch (state)
        {
            case GameState.Menu:
                EnterReady();
                break;
            case GameState.Playing:
                EnterPlaying();
                break;
            case GameState.Over:
                EnterOver();
                break;
        }
    }

    /// <summary>
    /// 进入主菜单
    /// </summary>
    private void EnterReady()
    {
        UIManager.instance.HidePanel<VictoryPanel>();
        UIManager.instance.ShowPanel<BeginPanel>();
        TimelineManager.instance.ResetSpendTime();
    }

    /// <summary>
    /// 进入游戏中
    /// </summary>
    private void EnterPlaying()
    {
        UIManager.instance.HidePanel<BeginPanel>();
        UIManager.instance.ShowPanel<GamePanel>();
        MapManager.Instance.Init();
        TimelineManager.instance.StartTimer();
    }

    /// <summary>
    /// 进入游戏结束
    /// </summary>
    private void EnterOver()
    {
        UIManager.instance.HidePanel<GamePanel>();
        UIManager.instance.ShowPanel<VictoryPanel>();
        UIManager.instance.GetPanel<VictoryPanel>().UpdateText(currentResult);
        MapManager.Instance.ClearView();
    }
}
