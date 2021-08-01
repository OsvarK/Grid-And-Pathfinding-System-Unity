using UnityEngine;

public class GridCameraDragController : MonoBehaviour
{
    private static GridCameraDragController singleton;
    [SerializeField]
    private float zoomStep = 5, minCamSize = 10, maxCamSize = 200;
    private Vector2 clampMin, clampMax;
    private Camera cam;
    private Vector3 dragOrigin;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else
            Destroy(this);
    }
    private void Start()
    {

        cam = gameObject.GetComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = maxCamSize / 2;
        Grid grid = GridRenderer.GetSingleton().GetGrid();  // Get referance to grid
        System.Tuple<float, float> centerOfGrid = grid.GetGridCenterInWorldSpace();
        SetCameraLocation(centerOfGrid.Item1, centerOfGrid.Item2);
        // Clamp camera to grid
        Vector2 clampMin = new Vector2(0, 0);
        Vector2 clampMax = new Vector2(
            grid.GetGridArray().GetLength(0) * grid.GetCellSize(),
            grid.GetGridArray().GetLength(1) * grid.GetCellSize());
        SetCameraBounds(clampMin, clampMax);
    }
    public static GridCameraDragController GetSingleton() { return singleton; }
    void Update()
    {
        PanCamera();
        Zoom();
        ClampCamera();
    }
    public void SetCameraBounds(Vector2 clampMin, Vector3 clampMax)
    {
        this.clampMin = clampMin;
        this.clampMax = clampMax;
    }
    private void ClampCamera()
    {
        float x = Mathf.Clamp(transform.position.x, clampMin.x, clampMax.x);
        float y = Mathf.Clamp(transform.position.y, clampMin.y, clampMax.y);
        transform.position = new Vector3(x, y, transform.position.z);
    }
    public void SetCameraLocation(float x, float y)
    {
       transform.position = new Vector3(x, y, transform.position.z);
    }
    private void PanCamera()
    {
        if (Input.GetMouseButtonDown(0))
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0))
        {
            Vector3 differance = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position += differance;
        }
    }
    public void Zoom()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            float newSize = 0;
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
            {
                newSize = cam.orthographicSize - zoomStep;
            } else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            {
                newSize = cam.orthographicSize + zoomStep;
            }
            cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);
        }
    }
}
