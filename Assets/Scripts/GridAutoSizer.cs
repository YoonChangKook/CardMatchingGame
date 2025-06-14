using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(GridLayoutGroup))]
public class GridAutoSizer : MonoBehaviour
{
    [Header("Layout Settings")]
    public int columns = 4;
    public Vector2 spacing = new Vector2(20f, 20f);
    public RectOffset padding = new RectOffset(30, 30, 30, 30);

    private GridLayoutGroup grid;
    private RectTransform rectTransform;

    void Awake()
    {
        grid = GetComponent<GridLayoutGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        StartCoroutine(DelayAndApply());
    }

    IEnumerator DelayAndApply()
    {
        // Wait 1 frame to ensure layout has calculated its dimensions
        yield return null;
        ApplyGridSizing();
    }

    public void ApplyGridSizing()
    {
        // 1. 총 카드 개수
        int childCount = transform.childCount;

        if (childCount == 0 || columns == 0)
            return;

        // 2. 줄 수 계산
        int rows = Mathf.CeilToInt(childCount / (float)columns);

        // 3. 가용 영역 계산 (부모 영역 - padding - spacing)
        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float usableWidth = parentWidth - padding.left - padding.right - spacing.x * (columns - 1);
        float usableHeight = parentHeight - padding.top - padding.bottom - spacing.y * (rows - 1);

        if (usableWidth <= 0 || usableHeight <= 0)
        {
            Debug.LogWarning("Not enough space for layout: check padding and spacing values.");
            return;
        }

        // 4. 셀 크기 계산
        float cellWidth = usableWidth / columns;
        float cellHeight = usableHeight / rows;

        // 5. 적용
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;
        grid.spacing = spacing;
        grid.padding = padding;
        grid.cellSize = new Vector2(cellWidth, cellHeight);
    }
}