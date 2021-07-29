using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

/// <summary>Render a grid in unity from a grid class, (turn on Gizmos).</summary>
public class GridRenderer : MonoBehaviour
{
    [Header("Grid properties")]
    [SerializeField] private int width = 50;
    [SerializeField] private int height = 50;
    [SerializeField] private int cellSize = 10;
    [Header("Render rules (May impact performance)")]
    [SerializeField] private bool renderCellsGizmos = true;           // Render cells when you have alot of cells will couse lag.
    [SerializeField] private bool renderCordinates = false;           // Render cordinates when you have alot of cells will couse lag.
    [SerializeField] private bool renderOutline = true;

    [SerializeField] private Color outlineColor = Color.gray;

    private LineRenderer lineRenderer;

    private static GridRenderer singleton;
    private Grid grid;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else
            Destroy(this);

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lineRenderer.receiveShadows = false;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = outlineColor;
        lineRenderer.endColor = outlineColor;

        Grid grid = new Grid(width, height, cellSize);
        GetSingleton().RenderGrid(grid);
    }

    public static GridRenderer GetSingleton() { return singleton; }

    /// <summary>Retrives the grid this visual grid represents.</summary>
    public Grid GetGrid() { return grid; }

    /// <summary>Retrives a cell from grid cordinates and returns the its world space.</summary>
    Vector3 CellToWorldSpace(int x, int y)
    {
        Tuple<float, float> cellWorldSpaceOrigin = grid.GetCellInWorldSpace(x, y);
        return new Vector3(cellWorldSpaceOrigin.Item1, cellWorldSpaceOrigin.Item2);
    }

    /// <summary>Method to render the grid, re renders grid if called again.</summary>
    public void RenderGrid(Grid grid)
    {
        ClearVisualGrid();
        this.grid = grid;
        StartCoroutine(StartRenderGrid());
        IEnumerator StartRenderGrid()
        {
            for (int x = 0; x < grid.GetGridArray().GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetGridArray().GetLength(1); y++)
                {
                    Tuple<float, float> cellWorldSpaceOrigin = grid.GetCellWorldPositionCenter(x, y);
                    Vector3 cellCenter = new Vector3(cellWorldSpaceOrigin.Item1, cellWorldSpaceOrigin.Item2);

                    if (renderCordinates)
                        CreateWorldText(Grid.GridCordinatesAsString(x, y), cellCenter, TextAnchor.MiddleCenter, TextAlignment.Center, 10, Color.white);
                    if (renderCellsGizmos)
                    {
                        Debug.DrawLine(CellToWorldSpace(x, y), CellToWorldSpace(x, y + 1), Color.white, 1 / 0f);
                        Debug.DrawLine(CellToWorldSpace(x, y), CellToWorldSpace(x + 1, y), Color.white, 1 / 0f);
                    }
                }
                yield return 0;
            }
        }

        if (renderCellsGizmos)
        {
            Debug.DrawLine(CellToWorldSpace(0, grid.GetGridArray().GetLength(1)), CellToWorldSpace(grid.GetGridArray().GetLength(0), grid.GetGridArray().GetLength(1)), Color.white, 1 / 0f);
            Debug.DrawLine(CellToWorldSpace(grid.GetGridArray().GetLength(0), 0), CellToWorldSpace(grid.GetGridArray().GetLength(0), grid.GetGridArray().GetLength(1)), Color.white, 1 / 0f);
        }

        if (renderOutline)
        {
            lineRenderer.positionCount = 5;
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, CellToWorldSpace(0, grid.GetGridArray().GetLength(1)));
            lineRenderer.SetPosition(2, CellToWorldSpace(grid.GetGridArray().GetLength(0), grid.GetGridArray().GetLength(1)));
            lineRenderer.SetPosition(3, CellToWorldSpace(grid.GetGridArray().GetLength(0), 0));
            lineRenderer.SetPosition(4, new Vector3((lineRenderer.startWidth / 2) * -1, 0, 0));
        }
    }

    /// <summary>Instantiates a TextMesh object, with corresponding propertyes.</summary>
    private TextMesh CreateWorldText(string text, Vector3 localPosition, TextAnchor textAnchor, TextAlignment textAlignment, int fontSize, Color color)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(this.gameObject.transform, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        return textMesh;
    }

    /// <summary>Destroys all gameobjects in the grid gameobject.</summary>
    private void ClearVisualGrid()
    {
        grid = null;
        foreach (Transform child in gameObject.transform)
            Destroy(child.gameObject);
    }
}
