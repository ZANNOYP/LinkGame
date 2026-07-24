using System.Collections;
using UnityEngine;
/// <summary>
/// 选中管理器
/// </summary>
public class SelectManager : MonoBehaviour
{
    public static SelectManager Instance;
    // 当前已经选中的格子
    private BlockView view;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 选中方块
    /// </summary>
    /// <param name="cell"></param>
    public void Select(GridCell cell)
    {
        view = cell.view;
        view.SetHightlight(true);

        view.StartAnimationScale();


        MusicManager.Instance.PlayEff(EffType.Select, 0.4f);
    }

    /// <summary>
    /// 清除选中
    /// </summary>
    public void ClearSelection()
    {
        if (view == null)
            return;

        view.SetHightlight(false);
        view = null;
    }
}
