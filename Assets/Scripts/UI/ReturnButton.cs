using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 返回按钮
/// </summary>
public class ReturnButton : MonoBehaviour
{
    public void ReturnMain()
    {
        MusicManager.Instance.PlayEff(EffType.Button, 0.4f);
        GameFlowManager.instance.ReadyGame();
    }
}
