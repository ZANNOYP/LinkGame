using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 退出游戏按钮
/// </summary>
public class ExitButton : MonoBehaviour
{
    /// <summary>
    /// 退出游戏
    /// </summary>
    public void ExitGame()
    {
        MusicManager.Instance.PlayEff(EffType.Button);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
