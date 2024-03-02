using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridSystem : MonoBehaviour, IPointerDownHandler {
    public static GridSystem Current { get; set; }
    public RectTransform Area { get => area; set => area = value; }
    public Grid Grid { get => grid; set => grid = value; }
    public PlaceMask Mask { get => mask; set => mask = value; }    

    [SerializeField] private RectTransform area;
    [SerializeField] private Grid grid;
    [SerializeField] private PlaceMask mask;    

    public Vector2Int gridIndex = new Vector2Int(0, 0);
    public Vector2Int gridCount = new Vector2Int(6, 6);

    public bool enableOriginTop = false;
    public bool enableOriginRight = false;

    private Vector2Int positionOffset;

    public Vector3 mousePosition;

    private void OnValidate() {
        OutExtream();
    }

    public bool TryArea(Vector2Int index) {        
        bool isOutArea = false;
        if (index.y >= gridCount.y) {
            isOutArea = true;
        }
        else if (index.y < 0) {
            isOutArea = true;
        }
        if (index.x >= gridCount.x) {
            isOutArea = true;
        }
        else if (index.x < 0) {
            isOutArea = true;
        }
        return isOutArea;
    }

    public bool TryArea(Vector2Int index, Vector2Int cell) {
        int horizontalPosition = enableOriginRight ? index.x - cell.x : index.x + cell.x;
        int verticalPosition = enableOriginTop ? index.y - cell.y : index.y + cell.y;

        return TryArea(new Vector2Int(horizontalPosition, verticalPosition));
    }


    private void OutExtream() {
        if (gridIndex.y >= gridCount.y) {
            gridIndex.y = gridCount.y - 1;
        }
        else if (gridIndex.y < 0) {
            gridIndex.y = 0;
        }
        if (gridIndex.x >= gridCount.x) {
            gridIndex.x = gridCount.x - 1;
        }
        else if (gridIndex.x < 0) {
            gridIndex.x = 0;
        }
    }

    public void Initialize() {        
        Current = this;
        positionOffset = new Vector2Int(gridCount.x / 2, gridCount.y / 2);        
        Vector2 canvasSize = Area.rect.size;
        Grid.cellSize = new Vector3(canvasSize.x / gridCount.x, canvasSize.y / gridCount.y, 0);
        Mask.GetComponent<RectTransform>().sizeDelta = Grid.cellSize;
    }

    public void Setup() {
        OutExtream();

        int horizontalPosition = gridIndex.x - positionOffset.x;
        horizontalPosition = enableOriginRight ? -(gridIndex.x - positionOffset.x) - 1 : horizontalPosition;
        int verticalPosition = gridIndex.y - positionOffset.y;
        verticalPosition = enableOriginTop ? -(gridIndex.y - positionOffset.y) - 1 : verticalPosition;

        Vector3Int placePos = new Vector3Int(horizontalPosition, verticalPosition, 0);
        Mask.transform.position = Grid.GetCellCenterWorld(placePos);
    }

    public Vector3 GetWorldPosition(Vector2Int cell) {
        int horizontalPosition = cell.x - positionOffset.x;
        horizontalPosition = enableOriginRight ? -(cell.x - positionOffset.x) - 1 : horizontalPosition;
        int verticalPosition = cell.y - positionOffset.y;
        verticalPosition = enableOriginTop ? -(cell.y - positionOffset.y) - 1 : verticalPosition;

        Vector3Int placePos = new Vector3Int(horizontalPosition, verticalPosition, 0);
        return Grid.GetCellCenterWorld(placePos);
    }

    public void OnPointerDown(PointerEventData eventData) {
        Debug.Log(eventData.position);
        RectTransformUtility.ScreenPointToWorldPointInRectangle(Area, eventData.position, Camera.main, out Vector3 maskPos);
        SetMaskPosition(maskPos);
    }

    public void SetMaskPosition(Vector3 placePos) {
        Vector3Int cellPos = Grid.WorldToCell(placePos);
        int horizontalPosition = cellPos.x + positionOffset.x;
        if (enableOriginRight) {
            horizontalPosition = gridCount.x - horizontalPosition - 1;
        }
        int verticalPosition = cellPos.y + positionOffset.y;
        if (enableOriginTop) {
            verticalPosition = gridCount.y - verticalPosition - 1;
        }
        gridIndex = new Vector2Int(horizontalPosition, verticalPosition);
        bool isPlaceable = true;
        foreach (var mask in Mask.m_MaskGrids) {
            mask.SetSkin(true);
            if (TryArea(gridIndex, mask.cell)) {
                isPlaceable = false;
                Mask.Invalidate();
                mask.SetSkin(false);
            }
        }
        if (isPlaceable) {
            Mask.Placeables();
        }

        Mask.transform.position = Grid.GetCellCenterWorld(cellPos);
    }

    public Vector2 GetSizeIn2DFrom(Grid grid) {
        switch (grid.cellSwizzle) {
            case GridLayout.CellSwizzle.XYZ:
                return new Vector2(grid.cellSize.x, grid.cellSize.y);                
            case GridLayout.CellSwizzle.XZY:
                return new Vector2(grid.cellSize.x, grid.cellSize.z);                
            case GridLayout.CellSwizzle.YXZ:
                return new Vector2(grid.cellSize.y, grid.cellSize.x);                
            case GridLayout.CellSwizzle.YZX:
                return new Vector2(grid.cellSize.y, grid.cellSize.z);                
            case GridLayout.CellSwizzle.ZXY:
                return new Vector2(grid.cellSize.z, grid.cellSize.x);                
            case GridLayout.CellSwizzle.ZYX:
                return new Vector2(grid.cellSize.z, grid.cellSize.y);
        }
        return Vector2.zero;
    }

    public void EnableMask(bool[] Sharp) {
        Mask.Enable(Sharp);
    }
    public void TryPlace() {
        Mask.Disable();
    }
}
