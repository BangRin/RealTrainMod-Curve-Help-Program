using System.Collections.Generic;
using UnityEngine;

public class WorldGridManager : MonoBehaviour
{
    public int gridSize = 3000;
    public int subGridSize = 30;
    public Vector3 calibrationPos;
    int cubeSize = 1;
    public LineRenderer lineRendererPrefab;

    private LineRenderer[,] subGrids; // 배열로 변경
    private Vector3 lastCamPosition;
    private float lastFov;
    private Camera cam;

    void Start()
    {
        cam = Camera.main; // 매번 Camera.main을 호출하는 비용을 줄임
        GenerateSubGrids();
        UpdateVisibleSubGrids();
        lastCamPosition = cam.transform.position;
        lastFov = cam.fieldOfView;
    }

    void Update()
    {
        if (Vector3.SqrMagnitude(cam.transform.position - lastCamPosition) > 1.0f * 1.0f 
            || cam.fieldOfView != lastFov)
        {
            UpdateVisibleSubGrids();
            lastCamPosition = cam.transform.position;
            lastFov = cam.fieldOfView;
        }
    }

    void GenerateSubGrids()
    {
        int numSubGrids = gridSize / subGridSize;
        subGrids = new LineRenderer[numSubGrids, numSubGrids];

        for (int x = 0; x < gridSize; x += subGridSize)
        {
            for (int z = 0; z < gridSize; z += subGridSize)
            {
                LineRenderer lr = Instantiate(lineRendererPrefab, this.transform);
                lr.gameObject.SetActive(false);
                InitLineRenderer(lr);

                DrawSubGrid(lr, x, z, subGridSize, calibrationPos);
                int gridX = x / subGridSize;
                int gridZ = z / subGridSize;
                subGrids[gridX, gridZ] = lr;
            }
        }
    }

    void InitLineRenderer(LineRenderer lr)
    {
        lr.startWidth = lr.endWidth = 0.05f;
        //lr.material.color = Color.white;
    }

    void UpdateVisibleSubGrids()
    {
        float camHeight = cam.transform.position.y;
        float fov = cam.fieldOfView;
        float visibleRange = camHeight * Mathf.Tan(fov * Mathf.Deg2Rad / 2);
        float minvisibleRange = 20f;

        visibleRange = Mathf.Max(visibleRange, minvisibleRange);

        for (int x = 0; x < subGrids.GetLength(0); x++)
        {
            for (int z = 0; z < subGrids.GetLength(1); z++)
            {
                LineRenderer lr = subGrids[x, z];
                if (lr == null)
                    continue;

                Vector3 gridCenter = new Vector3(x * subGridSize + subGridSize / 2.0f, 0, z * subGridSize + subGridSize / 2.0f) + calibrationPos;
                float distance = Vector3.Distance(new Vector3(cam.transform.position.x, 0, cam.transform.position.z), gridCenter);

                lr.gameObject.SetActive(distance <= visibleRange);
            }
        }
    }

    void DrawSubGrid(LineRenderer lr, float sr, float sc, int size, Vector3 offset)
    {
        List<Vector3> gridPos = new List<Vector3>();
        float ec = sc + size;
        float er = sr + size;

        Vector3 startPosition = new Vector3(sr, 0, sc) + offset;
        Vector3 endPosition = new Vector3(er, 0, ec) + offset;

        gridPos.Add(new Vector3(startPosition.x, offset.y, startPosition.z));
        gridPos.Add(new Vector3(startPosition.x, offset.y, endPosition.z));

        int toggle = -1;
        Vector3 currentPos = new Vector3(startPosition.x, offset.y, endPosition.z);
        for (int i = 0; i < size; i++)
        {
            Vector3 nextPos = currentPos;

            nextPos.x += cubeSize;
            gridPos.Add(nextPos);

            nextPos.z += (size * toggle * cubeSize);
            gridPos.Add(nextPos);

            currentPos = nextPos;
            toggle *= -1;
        }

        currentPos.x = startPosition.x;
        gridPos.Add(currentPos);

        int colToggle = toggle = 1;
        if (currentPos.z == endPosition.z) colToggle = -1;

        for (int i = 0; i < size; i++)
        {
            Vector3 nextPos = currentPos;

            nextPos.z += (colToggle * cubeSize);
            gridPos.Add(nextPos);

            nextPos.x += (size * toggle * cubeSize);
            gridPos.Add(nextPos);

            currentPos = nextPos;
            toggle *= -1;
        }

        lr.positionCount = gridPos.Count;
        lr.SetPositions(gridPos.ToArray());
    }
}
