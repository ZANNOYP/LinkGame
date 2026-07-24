using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 格子表现层
/// </summary>
public class BlockView : MonoBehaviour
{
    // 边界（高亮白框）
    public GameObject border;
    // 提示（黄色边框）
    public GameObject tipBorder;
    public Vector2 worldPos;

    private SpriteRenderer sr;

    private Animator anim;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        SetHightlight(false);
        SetTip(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 显隐选中边界
    /// </summary>
    /// <param name="active"></param>
    public void SetHightlight(bool active)
    {
        border.SetActive(active);
    }

    private void OnMouseDown()
    {
        Vector2 cellPos = MapManager.Instance.WorldToCell(worldPos);
        MapManager.Instance.OnCellClicked(cellPos);
    }

    /// <summary>
    /// 更新表现颜色
    /// </summary>
    public void Refresh(GridCell grid, bool playFade = false)
    {
        ItemData item = grid.item;
        Color c = sr.color;
        c.a = 1f;
        sr.color = c;

        if (item == null)
        {
            SetHightlight(false);
            if (playFade)
            {
                PlayFade();
            }
            else
            {
                EndFade();
            }

            return;
        }
        gameObject.SetActive(true);
        if (grid.desEff != null) 
            grid.desEff.Stop();
        ItemType itemType = item.type;
        Color color = MapManager.Instance.colors[(int)itemType];
        sr.color = color;
    }

    /// <summary>
    /// 显隐提示边框
    /// </summary>
    /// <param name="active"></param>
    public void SetTip(bool active)
    {
        tipBorder.SetActive(active);
    }

    /// <summary>
    /// 开始缩放动画
    /// </summary>
    public void StartAnimationScale()
    {
        anim.SetBool("isScale", true);
    }

    /// <summary>
    /// 重置缩放动画状态
    /// </summary>
    public void ResetAnimationScale()
    {
        anim.SetBool("isScale", false);
    }

    /// <summary>
    /// 播放消除渐隐动画
    /// </summary>
    public void PlayFade()
    {
        anim.SetTrigger("isFade");
    }

    /// <summary>
    /// 结束渐隐调用，失活与缩放恢复
    /// </summary>
    public void EndFade()
    {
        gameObject.SetActive(false);
        transform.localScale = new Vector3(1, 1, transform.localScale.z);
    }

}
