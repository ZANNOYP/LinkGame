using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 胜利面板
/// </summary>
public class VictoryPanel : BasePanel
{
    // 花费时间文本
    public TextMeshProUGUI timeText;
    // 标题文本
    public TextMeshProUGUI titleText;
    /// <summary>
    /// 更新文本
    /// </summary>
    /// <param name="spendTime"></param>
    /// <param name="isVictory"></param>
    public void UpdateText(GameResult result)
    {
        // 失败
        if (result.resultType == GameOverType.Timeout) 
        {
            titleText.text = "  失 败~";
            timeText.text = "时间耗尽";
        }
        // 成功
        else
        {
            titleText.text = "  成 功 !!";
            int spendTimeInt = Mathf.RoundToInt(result.spendTime);
            timeText.text = "用时 " + spendTimeInt + " 秒";
        }
    }
}
