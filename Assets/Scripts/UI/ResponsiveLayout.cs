using UnityEngine;
using UnityEngine.UI;

// Lays out the level UI for any aspect ratio:
//  landscape - top bar, board on the left, quest column on the right
//  portrait  - top bar, horizontal quest strip, board below
// Runs in edit mode too, so the scene view matches the game view.
[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class ResponsiveLayout : MonoBehaviour
{
    [SerializeField] private RectTransform _safeArea;

    [SerializeField] private RectTransform _topBar;

    [SerializeField] private RectTransform _board;

    [SerializeField] private GridLayoutGroup _boardGrid;

    [SerializeField] private RectTransform _questPanel;

    [SerializeField] private RectTransform _questHeader;

    [SerializeField] private ScrollRect _questScroll;

    [SerializeField] private GridLayoutGroup _questGrid;

    [Header("Sizes (canvas units)")]
    [SerializeField]
    private float _topBarHeight = 130f;

    [SerializeField] private float _margin = 24f;

    [SerializeField] private float _boardPadding = 44f;

    [SerializeField] private float _boardSpacing = 10f;

    [SerializeField] private float _maxCellSize = 170f;

    [SerializeField] [Range(0.5f, 1f)] private float _itemSizeInSlot = 0.8f;

    [SerializeField] private Vector2 _questColumnWidth = new(400f, 560f);

    [SerializeField] private Vector2 _questStripHeight = new(340f, 420f);

    [SerializeField] private float _questCardHeight = 210f;

    [SerializeField] private float _questCardWidthPortrait = 400f;

    [SerializeField] private float _questHeaderHeight = 56f;

    [SerializeField] private float _questPanelPadding = 20f;

    [SerializeField] private float _headerInset = 48f;

    [SerializeField] private float _headerTop = 26f;

    private bool dirty = true;
    private Rect lastSafeArea;

    public bool IsPortrait { get; private set; }

    private void LateUpdate()
    {
        if (Application.isPlaying && Screen.safeArea != lastSafeArea)
            dirty = true;

        if (dirty)
            Apply();
    }

    private void OnEnable()
    {
        dirty = true;
    }

    private void OnRectTransformDimensionsChange()
    {
        dirty = true;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        dirty = true;
    }
#endif

    public void Apply()
    {
        dirty = false;
        if (_safeArea == null || _topBar == null || _board == null || _questPanel == null)
            return;

        ApplySafeArea();

        var size = _safeArea.rect.size;
        if (size.x <= 0 || size.y <= 0)
            return;

        IsPortrait = size.y > size.x;

        SetRect(_topBar, 0, 0, size.x, _topBarHeight);
        var bodyTop = _topBarHeight + _margin;
        var bodyHeight = size.y - bodyTop - _margin;

        if (IsPortrait)
        {
            // Board takes the full width; quests get what's left above it (two rows if it fits)
            var width = size.x - 2 * _margin;
            var boardSize = MeasureBoard(width, bodyHeight - _margin - _questStripHeight.x, out var cell);
            var stripSpace = bodyHeight - _margin - boardSize.y;
            var twoRowHeight = QuestStripHeight(2);
            var rows = stripSpace >= twoRowHeight ? 2 : 1;
            var stripHeight =
                rows == 2 ? twoRowHeight : Mathf.Clamp(stripSpace, _questStripHeight.x, _questStripHeight.y);

            var stackHeight = stripHeight + _margin + boardSize.y;
            var top = bodyTop + Mathf.Max(0, (bodyHeight - stackHeight) / 2);
            SetRect(_questPanel, _margin, top, width, stripHeight);
            LayoutQuests(true, stripHeight, rows);
            PlaceBoard(_margin + (width - boardSize.x) / 2, top + stripHeight + _margin, boardSize, cell);
        }
        else
        {
            var columnWidth = Mathf.Clamp(size.x * 0.27f, _questColumnWidth.x, _questColumnWidth.y);
            SetRect(_questPanel, size.x - _margin - columnWidth, bodyTop, columnWidth, bodyHeight);
            LayoutQuests(false, columnWidth, 1);

            var areaWidth = size.x - columnWidth - 3 * _margin;
            var boardSize = MeasureBoard(areaWidth, bodyHeight, out var cell);
            PlaceBoard(_margin + (areaWidth - boardSize.x) / 2, bodyTop + (bodyHeight - boardSize.y) / 2, boardSize,
                cell);
        }
    }

    private float QuestStripHeight(int rows)
    {
        return _questHeaderHeight + _headerTop + _questPanelPadding * 2f + rows * _questCardHeight +
               (rows - 1) * _questPanelPadding;
    }

    private void ApplySafeArea()
    {
        if (!Application.isPlaying)
        {
            _safeArea.anchorMin = Vector2.zero;
            _safeArea.anchorMax = Vector2.one;
            _safeArea.offsetMin = _safeArea.offsetMax = Vector2.zero;
            return;
        }

        lastSafeArea = Screen.safeArea;
        var min = lastSafeArea.position;
        var max = lastSafeArea.position + lastSafeArea.size;
        min.x /= Screen.width;
        max.x /= Screen.width;
        min.y /= Screen.height;
        max.y /= Screen.height;
        _safeArea.anchorMin = min;
        _safeArea.anchorMax = max;
        _safeArea.offsetMin = _safeArea.offsetMax = Vector2.zero;
    }

    private Vector2Int BoardDimensions()
    {
        var cols = Mathf.Max(1, _boardGrid.constraintCount);
        var slots = 0;
        foreach (Transform child in _boardGrid.transform)
            if (child.gameObject.activeSelf)
                slots++;
        return new Vector2Int(cols, Mathf.Max(1, Mathf.CeilToInt(slots / (float)cols)));
    }

    private Vector2 MeasureBoard(float maxWidth, float maxHeight, out float cell)
    {
        cell = 0;
        if (_boardGrid == null)
            return Vector2.zero;

        var dims = BoardDimensions();
        var cellW = (maxWidth - 2 * _boardPadding - (dims.x - 1) * _boardSpacing) / dims.x;
        var cellH = (maxHeight - 2 * _boardPadding - (dims.y - 1) * _boardSpacing) / dims.y;
        cell = Mathf.Max(0, Mathf.Floor(Mathf.Min(cellW, cellH, _maxCellSize)));
        return new Vector2(
            dims.x * cell + (dims.x - 1) * _boardSpacing + 2 * _boardPadding,
            dims.y * cell + (dims.y - 1) * _boardSpacing + 2 * _boardPadding);
    }

    private void PlaceBoard(float x, float yFromTop, Vector2 size, float cell)
    {
        if (_boardGrid == null || cell <= 0)
            return;

        SetRect(_board, x, yFromTop, size.x, size.y);

        var gridRect = (RectTransform)_boardGrid.transform;
        gridRect.anchorMin = Vector2.zero;
        gridRect.anchorMax = Vector2.one;
        gridRect.pivot = new Vector2(0.5f, 0.5f);
        gridRect.offsetMin = new Vector2(_boardPadding, _boardPadding);
        gridRect.offsetMax = new Vector2(-_boardPadding, -_boardPadding);

        _boardGrid.padding = new RectOffset();
        _boardGrid.cellSize = new Vector2(cell, cell);
        _boardGrid.spacing = new Vector2(_boardSpacing, _boardSpacing);
        _boardGrid.startCorner = GridLayoutGroup.Corner.UpperLeft;
        _boardGrid.startAxis = GridLayoutGroup.Axis.Horizontal;
        _boardGrid.childAlignment = TextAnchor.MiddleCenter;
        _boardGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;

        // Each slot centres its item with its own grid
        var itemSize = Mathf.Round(cell * _itemSizeInSlot);
        foreach (Transform slot in _boardGrid.transform)
        {
            var slotGrid = slot.GetComponent<GridLayoutGroup>();
            if (slotGrid == null)
                continue;
            slotGrid.cellSize = new Vector2(itemSize, itemSize);
            slotGrid.childAlignment = TextAnchor.MiddleCenter;
        }
    }

    private void LayoutQuests(bool horizontal, float panelSize, int rows)
    {
        if (_questScroll == null || _questGrid == null)
            return;

        if (_questHeader != null)
        {
            _questHeader.anchorMin = new Vector2(0, 1);
            _questHeader.anchorMax = new Vector2(1, 1);
            _questHeader.pivot = new Vector2(0.5f, 1);
            // Keep clear of the panel's corner marks
            _questHeader.offsetMin = new Vector2(_headerInset, -_questHeaderHeight - _headerTop);
            _questHeader.offsetMax = new Vector2(-_headerInset, -_headerTop);
        }

        var scrollRect = (RectTransform)_questScroll.transform;
        scrollRect.anchorMin = Vector2.zero;
        scrollRect.anchorMax = Vector2.one;
        scrollRect.pivot = new Vector2(0.5f, 0.5f);
        scrollRect.offsetMin = new Vector2(_questPanelPadding * 0.5f, _questPanelPadding * 0.5f);
        scrollRect.offsetMax = new Vector2(-_questPanelPadding * 0.5f, -(_questHeaderHeight + _headerTop));

        _questScroll.horizontal = horizontal;
        _questScroll.vertical = !horizontal;

        var content = (RectTransform)_questGrid.transform;
        var fitter = _questGrid.GetComponent<ContentSizeFitter>();
        var pad = Mathf.RoundToInt(_questPanelPadding * 0.5f);
        _questGrid.padding = new RectOffset(pad, pad, pad, pad);
        _questGrid.spacing = new Vector2(_questPanelPadding, _questPanelPadding);
        _questGrid.startCorner = GridLayoutGroup.Corner.UpperLeft;

        if (horizontal)
        {
            var cardHeight = (panelSize - _questHeaderHeight - _headerTop - 2f * _questPanelPadding -
                              (rows - 1) * _questPanelPadding) / rows;
            _questGrid.startAxis = GridLayoutGroup.Axis.Vertical;
            _questGrid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            _questGrid.constraintCount = rows;
            _questGrid.cellSize = new Vector2(_questCardWidthPortrait, cardHeight);
            _questGrid.childAlignment = TextAnchor.MiddleLeft;

            content.anchorMin = new Vector2(0, 0);
            content.anchorMax = new Vector2(0, 1);
            content.pivot = new Vector2(0, 0.5f);
            content.offsetMin = new Vector2(content.offsetMin.x, 0);
            content.offsetMax = new Vector2(content.offsetMax.x, 0);
            if (fitter != null)
            {
                fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                fitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
            }
        }
        else
        {
            var cardWidth = panelSize - 2 * _questPanelPadding;
            _questGrid.startAxis = GridLayoutGroup.Axis.Horizontal;
            _questGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _questGrid.constraintCount = 1;
            _questGrid.cellSize = new Vector2(cardWidth, _questCardHeight);
            _questGrid.childAlignment = TextAnchor.UpperCenter;

            content.anchorMin = new Vector2(0, 1);
            content.anchorMax = new Vector2(1, 1);
            content.pivot = new Vector2(0.5f, 1);
            content.offsetMin = new Vector2(0, content.offsetMin.y);
            content.offsetMax = new Vector2(0, content.offsetMax.y);
            if (fitter != null)
            {
                fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
                fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            }
        }
    }

    // Positions a rect inside safeArea using top-left based coordinates
    private static void SetRect(RectTransform rt, float x, float yFromTop, float w, float h)
    {
        rt.anchorMin = rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);
        rt.anchoredPosition = new Vector2(x, -yFromTop);
        rt.sizeDelta = new Vector2(w, h);
    }
}