using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEngine;
/// <summary>
/// 地图管理器
/// </summary>
public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    // 地图宽度（包括黑色边界）
    public int width = 10;
    // 地图高度（包括黑色边界）
    public int height = 10;
    // 格子信息数组
    public GridCell[,] grids;
    // 格子表现层预设体
    public GameObject gridPrefab;
    // 格子表现层所有颜色
    public UnityEngine.Color[] colors;

    // 第一选中
    public GridCell firstSelected;
    // 第二选中
    public GridCell secondSelected;
    // 连线的材质
    public Material lineMaterial;
    // 隐藏线段用时
    public float hideLineInterval = 0.5f;
    // 转换坐标
    public Vector2 conversion = new Vector2(-5, -4);
    // 延长时间需要消除次数
    public int addTimeMatchCount = 5;
    // 延长的时间
    public float addTime = 3f;
    // 连线
    private LineRenderer lr;
    // 消除次数
    private int matchCount;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 初始化所有格子
    /// </summary>
    public void Init()
    {
        matchCount = 0;
        HintManager.Instance.ResetHintCount();
        HintManager.Instance.StopTipCoroutinue();
        // 第一次生成方块 需要初始化grids
        if (grids == null)
        {
            grids = new GridCell[width, height];
        }

        List<int> colorList = CreateColorList();

        bool success = false;

        for (int i = 0; i < 100; i++)
        {
            ShuffleColorList(colorList);

            InitGridData(colorList);

            if (GetAnyMatch() != (null, null))
            {
                success = true;
                break;
            }
        }

        if (!success)
        {
            return;
        }

        GenerateBoard(colorList);
    }

    /// <summary>
    /// 清除表现
    /// </summary>
    public void ClearView()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grids[j, i].view != null)
                {
                    grids[j, i].item = null;
                    grids[j, i].view.Refresh(grids[j, i]);
                }
            }
        }
    }

    /// <summary>
    /// 制作配对颜色列表
    /// </summary>
    public List<int> CreateColorList()
    {
        int colorCount = (width - 2) * (height - 2);
        List<int> colorList = new List<int>();
        for (int i = 0; i < colorCount / 2; i++)
        {
            int value = Random.Range(0, colors.Length);
            colorList.Add(value);
            colorList.Add(value);
        }

        return colorList;
    }

    /// <summary>
    /// 打乱颜色列表
    /// </summary>
    /// <param name="colorList"></param>
    public void ShuffleColorList(List<int> colorList)
    {
        for (int i = 0; i < colorList.Count; i++)
        {
            int index = Random.Range(i, colorList.Count);
            int temp = colorList[index];
            colorList[index] = colorList[i];
            colorList[i] = temp;
        }
    }

    /// <summary>
    /// 初始化格子数据层
    /// </summary>
    /// <param name="colorList"></param>
    public void InitGridData(List<int> colorList)
    {
        int w = 0;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                // 最下面和最上面一行 不需要表现层，仅初始化格子坐标
                if (i < 1 || i > height - 2)
                {
                    if (grids[j, i] == null)
                    {
                        grids[j, i] = new GridCell();
                        Vector2Int pos1 = new Vector2Int(j, i);
                        grids[j, i].pos = pos1;
                    }
                }
                // 最左边和最右边一列 不需要表现层，仅初始化格子坐标
                else if (j < 1 || j > width - 2)
                {
                    if (grids[j, i] == null)
                    {
                        grids[j, i] = new GridCell();
                        Vector2Int pos2 = new Vector2Int(j, i);
                        grids[j, i].pos = pos2;
                    }
                }
                // 中间实际可操作格子 初始化坐标以及数据层的颜色
                else
                {
                    if (grids[j, i] == null)
                    {
                        grids[j, i] = new GridCell();
                        Vector2Int pos3 = new Vector2Int(j, i);
                        grids[j, i].pos = pos3;
                    }
                    grids[j, i].item = new ItemData();

                    switch(colorList[w])
                    {
                        case 0:
                            grids[j, i].item.type = ItemType.G1;
                            break;
                        case 1:
                            grids[j, i].item.type = ItemType.R;
                            break;
                        case 2:
                            grids[j, i].item.type = ItemType.P1;
                            break;
                        case 3:
                            grids[j, i].item.type = ItemType.B1;
                            break;
                        case 4:
                            grids[j, i].item.type = ItemType.B2;
                            break;
                        case 5:
                            grids[j, i].item.type = ItemType.O;
                            break;
                        case 6:
                            grids[j, i].item.type = ItemType.P2;
                            break;
                        case 7:
                            grids[j, i].item.type = ItemType.G2;
                            break;
                        case 8:
                            grids[j, i].item.type = ItemType.B3;
                            break;
                        case 9:
                            grids[j, i].item.type = ItemType.G3;
                            break;
                        case 10:
                            grids[j, i].item.type = ItemType.B4;
                            break;
                        case 11:
                            grids[j, i].item.type = ItemType.P3;
                            break;
                        case 12:
                            grids[j, i].item.type = ItemType.P4;
                            break;
                    }


                    w++;
                }
            }
        }
    }

    /// <summary>
    /// 格子坐标切换世界坐标
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private Vector2 CellToWorld(Vector2 pos)
    {
        Vector2 worldPos = conversion + pos;
        return worldPos;
    }

    /// <summary>
    /// 得到任意一组可消除格子对
    /// </summary>
    /// <returns></returns>
    public (GridCell, GridCell) GetAnyMatch()
    {
        for (int y1 = 1; y1 < height - 1; y1++)
        {
            for (int x1 = 1; x1 < width - 1; x1++)
            {
                GridCell a = grids[x1, y1];
                if (a.IsEmpty) continue;

                for (int y2 = y1; y2 < height - 1; y2++)
                {
                    for (int x2 = x1; x2 < width - 1; x2++)
                    {
                        if (x1 == x2 && y1 == y2) continue;
                        GridCell b = grids[x2, y2];
                        if (b.IsEmpty) continue;

                        if (CanMatch(a, b))
                        {
                            return (a, b);
                        }
                    }
                }
            }
        }
        return (null, null);
    }

    /// <summary>
    /// 生成格子表现层
    /// </summary>
    private void GenerateBoard(List<int> colorList)
    {
        for (int i = 1; i < height - 1; i++)
        {
            for (int j = 1; j < width - 1; j++)
            {
                // 第一次生成 创建Gameobject 初始化格子表现层
                if (grids[j, i].view == null)
                {
                    GameObject obj = GameObject.Instantiate(gridPrefab);
                    obj.transform.position = CellToWorld(new Vector2(j, i));

                    BlockView bv = obj.GetComponent<BlockView>();
                    bv.worldPos = obj.transform.position;
                    bv.Refresh(grids[j, i]);
                    grids[j, i].view = bv;

                    DestroyEffect de = obj.GetComponentInChildren<DestroyEffect>(true);
                    grids[j, i].desEff = de;
                }
                // 不是第一次生成 则只更新表现层
                else
                {
                    grids[j, i].view.Refresh(grids[j, i]);
                }
            }
        }
    }

    /// <summary>
    /// 打乱
    /// </summary>
    public void Shuffle()
    {
        if (grids == null)
        {
            Debug.Log("格子未初始化，请按Init按钮！！");
            return;
        }

        //HintManager.Instance.ResetBeforeTimer();
        HintManager.Instance.StopTipCoroutinue();
        // 打乱数据层直到有可消除方块
        bool success = false;

        for (int i = 0; i < 100; i++)
        {
            ShuffleData();

            if (GetAnyMatch() != (null, null))
            {
                success = true;
                break;
            }
        }

        if (!success)
        {
            Debug.Log("打乱失败");
            return;
        }

        DoShuffle();
    }

    /// <summary>
    /// 打乱格子的数据层
    /// </summary>
    private void ShuffleData()
    {
        for (int y1 = 1; y1 < height - 1; y1++)
        {
            for (int x1 = 1; x1 < width - 1; x1++)
            {
                GridCell a = grids[x1, y1];

                int y2 = Random.Range(1, height - 1);
                int x2 = Random.Range(1, width - 1);

                if (x1 == x2 && y1 == y2) continue;

                GridCell b = grids[x2, y2];

                (a.item, b.item) = (b.item, a.item);
            }
        }
    }

    /// <summary>
    /// 打乱后更新表现层
    /// </summary>
    private void DoShuffle()
    {
        for (int y1 = 1; y1 < height - 1; y1++)
        {
            for (int x1 = 1; x1 < width - 1; x1++)
            {
                GridCell a = grids[x1, y1];
                a.view.Refresh(a);
            }
        }
    }

    /// <summary>
    /// 世界坐标转格子坐标
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public Vector2 WorldToCell(Vector2 worldPos)
    {
        Vector2 cellPos = worldPos - conversion;
        return cellPos;
    }

    /// <summary>
    /// 点击方块逻辑
    /// </summary>
    /// <param name="pos"></param>
    public void OnCellClicked(Vector2 pos)
    {
        HintManager.Instance.StopTipCoroutinue();

        GridCell cell = grids[(int)pos.x, (int)pos.y];
        // 格子为空白，无法点击
        if (cell.IsEmpty) return;

        // 第一次选择
        if (firstSelected == null)
        {
            firstSelected = cell;
            // 选中提示
            SelectManager.Instance.Select(firstSelected);
        }
        // 第二次选择
        else
        {
            // 如果点了同个方块两次 隐藏选中提示
            if (firstSelected == cell)
            {
                SelectManager.Instance.ClearSelection();
                firstSelected = null;
            }
            // 点了两个不同方块
            else
            {
                secondSelected = cell;
                // 满足消除条件则消除
                if (CanMatch(firstSelected, secondSelected, true)) 
                {
                    MusicManager.Instance.PlayEff(EffType.Match);

                    firstSelected.item = null;
                    secondSelected.item = null;

                    firstSelected.view.Refresh(firstSelected, true);
                    secondSelected.view.Refresh(secondSelected, true);

                    firstSelected.desEff.Play();
                    secondSelected.desEff.Play();

                    firstSelected = null;
                    secondSelected = null;

                    matchCount++;
                    // 满足消除数量 延长剩余时间
                    if (matchCount >= addTimeMatchCount)
                    {
                        matchCount = 0;
                        TimelineManager.instance.AddTime(addTime);
                    }

                    //HintManager.Instance.ResetBeforeTimer();
                    // 游戏结束
                    if (EmptyGrids())
                    {
                        GameResult result = new GameResult();
                        float spendTime = TimelineManager.instance.StopTimer();
                        result.resultType = GameOverType.Success;
                        result.spendTime = spendTime;
                        GameFlowManager.instance.OverGame(result);
                    }
                }
                // 不满足消除条件则取消选中提示
                else
                {
                    SelectManager.Instance.ClearSelection();
                    firstSelected = null;
                    secondSelected = null;
                }
            }
        }
    }

    /// <summary>
    /// 检测是否全部消除完毕
    /// </summary>
    /// <returns></returns>
    public bool EmptyGrids()
    {
        GridCell cell;
        for (int y1 = 1; y1 < height - 1; y1++)
        {
            for (int x1 = 1; x1 < width - 1; x1++)
            {
                cell = grids[x1, y1];
                if (!cell.IsEmpty)
                {
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// 消除条件
    /// </summary>
    /// <returns></returns>
    public bool CanMatch(GridCell a, GridCell b, bool drawLine = false)
    {
        if (a == b) return false;
        if (a.item.type != b.item.type) return false;
        if (CheckStraight(a, b)) 
        {
            if (drawLine)
            {
                List<Vector2> path = new List<Vector2>();
                Vector2 pos1 = CellToWorld(a.pos);
                Vector2 pos2 = CellToWorld(b.pos);
                path.Add(pos1);
                path.Add(pos2);
                DrawLine(path);
            }
                
            return true;
        }
        if (CheckOneCorner(a, b, out List<Vector2> path2))
        {
            if (drawLine)
                DrawLine(path2);
            return true;
        }
        if (CheckTwoCorner(a, b, out List<Vector2> path3))
        {
            if (drawLine)
                DrawLine(path3);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 检查直线
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public bool CheckStraight(GridCell a, GridCell b)
    {
        if (a.pos.x == b.pos.x)
        {
            int minY = Mathf.Min(a.pos.y, b.pos.y);
            int maxY = Mathf.Max(a.pos.y, b.pos.y);

            for (int i = minY + 1; i < maxY; i++)
            {
                if (!grids[a.pos.x, i].IsEmpty)
                {
                    return false;
                }
            }
            return true;
        }
        else if (a.pos.y == b.pos.y)
        {
            int minX = Mathf.Min(a.pos.x, b.pos.x);
            int maxX = Mathf.Max(a.pos.x, b.pos.x);

            for (int i = minX + 1; i < maxX; i++)
            {
                if (!grids[i, a.pos.y].IsEmpty)
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// 连线
    /// </summary>
    public void DrawLine(List<Vector2> path)
    {
        if (lr == null)
        {
            GameObject obj = new GameObject();
            obj.name = "line";
            lr = obj.AddComponent<LineRenderer>();
            lr.material = lineMaterial;
            lr.startWidth = 0.01f;
            lr.endWidth = 0.01f;
            lr.sortingOrder = -3;
        }
        lr.gameObject.SetActive(true);
        StartCoroutine(DrawLineCoroutine(path));
    }
    
    /// <summary>
    /// 绘制连线协程
    /// </summary>
    /// <param name="lr"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    private IEnumerator DrawLineCoroutine(List<Vector2> path)
    {
        lr.positionCount = 2;
        // 当线条点数小于等于路径点数时 逐渐移动线条的点位
        while (lr.positionCount <= path.Count) 
        {
            // 第一次进入 初始化所有点位
            if (lr.positionCount == 2)
            {
                for (int i = 0; i < lr.positionCount; i++)
                {
                    lr.SetPosition(i, path[0]);
                }
            }
            // 点位移动速度
            float speed = 10f;

            float t = 0;
            // 将点位逐渐移动到目标点位
            while (t < 1f)
            {
                t += Time.deltaTime * speed;
                Vector2 current = Vector2.Lerp(path[lr.positionCount - 2], path[lr.positionCount - 1], t);
                lr.SetPosition(lr.positionCount - 1, current);
                yield return null;
            }
            // 增加线段的点数
            lr.positionCount++;
            lr.SetPosition(lr.positionCount - 1, path[lr.positionCount - 2]);
        }
        // 删掉最后增加的点位
        lr.positionCount--;
        // 开启隐藏线段协程
        StartCoroutine(HideLineCoroutine(lr));
    }

    /// <summary>
    /// 隐藏线条协程
    /// </summary>
    /// <param name="lr"></param>
    /// <returns></returns>
    private IEnumerator HideLineCoroutine(LineRenderer lr)
    {
        yield return new WaitForSeconds(hideLineInterval);
        lr.gameObject.SetActive(false);
    }

    /// <summary>
    /// 检查一个拐角
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public bool CheckOneCorner(GridCell a, GridCell b, out List<Vector2> path)
    {
        path = new List<Vector2>();
        GridCell p1 = grids[a.pos.x, b.pos.y];
        GridCell p2 = grids[b.pos.x, a.pos.y];

        if (p1.IsEmpty && CheckStraight(a, p1) && CheckStraight(p1, b)) 
        {
            Vector2 pos1 = CellToWorld(a.pos);
            Vector2 pos2 = CellToWorld(p1.pos);
            Vector2 pos3 = CellToWorld(b.pos);
            path.Add(pos1);
            path.Add(pos2);
            path.Add(pos3);
            return true;
        }
        path.Clear();

        if (p2.IsEmpty && CheckStraight(a, p2) && CheckStraight(p2, b))
        {
            Vector2 pos1 = CellToWorld(a.pos);
            Vector2 pos2 = CellToWorld(p2.pos);
            Vector2 pos3 = CellToWorld(b.pos);
            path.Add(pos1);
            path.Add(pos2);
            path.Add(pos3);
            return true;
        }
        path.Clear();
        return false;
    }

    /// <summary>
    /// 检查两个拐角
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public bool CheckTwoCorner(GridCell a, GridCell b, out List<Vector2> path)
    {
        path = new List<Vector2>();
        GridCell[,] grids = MapManager.Instance.grids;
        for (int i = a.pos.x - 1; i >= 0; i--)
        {
            GridCell p2 = grids[i, a.pos.y];
            if (!p2.IsEmpty) break;

            if (CheckOneCorner(p2, b, out path)) 
            {
                Vector2 pos1 = CellToWorld(a.pos);
                path.Insert(0, pos1);
                return true;
            }
        }

        for (int i = a.pos.x + 1; i < MapManager.Instance.width; i++)
        {
            GridCell p1 = grids[i, a.pos.y];
            if (!p1.IsEmpty) break;

            if (CheckOneCorner(p1, b, out path))
            {
                Vector2 pos1 = CellToWorld(a.pos);
                path.Insert(0, pos1);
                return true;
            }
                
        }

        for (int i = a.pos.y - 1; i >= 0; i--)
        {
            GridCell p4 = grids[a.pos.x, i];
            if (!p4.IsEmpty) break;

            if (CheckOneCorner(p4, b, out path))
            {
                Vector2 pos1 = CellToWorld(a.pos);
                path.Insert(0, pos1);
                return true;
            }
        }

        for (int i = a.pos.y + 1; i < MapManager.Instance.height; i++)
        {
            GridCell p3 = grids[a.pos.x, i];
            if (!p3.IsEmpty) break;

            if (CheckOneCorner(p3, b, out path))
            {
                Vector2 pos1 = CellToWorld(a.pos);
                path.Insert(0, pos1);
                return true;
            }
        }
        return false;
    }

    
}
