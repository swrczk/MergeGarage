using UnityEngine;
using UnityEngine.UI;

// Swaps a rect's size (and optionally a grid's arrangement) between landscape and portrait
[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class OrientationSwitch : MonoBehaviour
{
    [SerializeField] private Vector2 _landscapeSize;

    [SerializeField] private Vector2 _portraitSize;

    [SerializeField] private GridLayoutGroup _grid;

    [SerializeField] private Vector2 _landscapeCell;

    [SerializeField] private Vector2 _portraitCell;

    private bool? portrait;

    private void LateUpdate()
    {
        Apply();
    }

    private void OnEnable()
    {
        portrait = null;
        Apply();
    }

    public void Apply()
    {
        var canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
            return;

        var size = ((RectTransform)canvas.rootCanvas.transform).rect.size;
        var portrait = size.y > size.x;
        if (portrait == this.portrait)
            return;
        this.portrait = portrait;

        ((RectTransform)transform).sizeDelta = portrait ? _portraitSize : _landscapeSize;
        if (_grid == null)
            return;

        _grid.cellSize = portrait ? _portraitCell : _landscapeCell;
        _grid.constraint =
            portrait ? GridLayoutGroup.Constraint.FixedColumnCount : GridLayoutGroup.Constraint.FixedRowCount;
        _grid.constraintCount = 1;
    }
}