using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 开始游戏按钮
/// </summary>
public class StartButton : MonoBehaviour
{
    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame()
    {
        MusicManager.Instance.PlayEff(EffType.Button, 0.4f);
        GameFlowManager.instance.StartGame();
    }
}
