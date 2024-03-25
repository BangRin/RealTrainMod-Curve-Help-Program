using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGrid : Singleton<LineGrid>
{
    public LineRenderer lr;
    public int sr, sc;
    public int rowCount, colCount;
    public float gridSize;
    public Camera mainCamera;

    void OnValidate()
    {
        if (rowCount + colCount > 0)
        {
            MakeGrid(lr, sr, sc, rowCount, colCount);
        }
    }

    public void UpdateGrid()
    {
        MakeGrid(lr, sr, sc, rowCount, colCount);
    }

    public void UpdateOffsetGrid(int x, int z)
    {
        int roffset = (rowCount / -2) + x;
        int coffset = (colCount / -2) + z;

        sr = roffset;
        sc = coffset;

        MakeGrid(lr, sr, sc, rowCount, colCount);
    }

    void initLineRenderer(LineRenderer lr)
    {
        lr.startWidth = lr.endWidth = 0.05f;
        lr.material.color = Color.blue;
    }

    void MakeGrid(LineRenderer lr, float sr, float sc, int rowCount, int colCount)
    {
        List<Vector3> gridPos = new List<Vector3>();

        float ec = sc + colCount * gridSize;

        gridPos.Add(new Vector3(sr, this.transform.position.y, sc));
        gridPos.Add(new Vector3(sr, this.transform.position.y, ec));

        int toggle = -1;
        Vector3 currentPos = new Vector3(sr, this.transform.position.y, ec);
        for (int i = 0; i < rowCount; i++)
        {
            Vector3 nextPos = currentPos;

            nextPos.x += gridSize;
            gridPos.Add(nextPos);

            nextPos.z += (colCount * toggle * gridSize);
            gridPos.Add(nextPos);

            currentPos = nextPos;
            toggle *= -1;
        }

        currentPos.x = sr;
        gridPos.Add(currentPos);

        int colToggle = toggle = 1;
        if (currentPos.z == ec) colToggle = -1;

        for (int i = 0; i < colCount; i++)
        {
            Vector3 nextPos = currentPos;

            nextPos.z += (colToggle * gridSize);
            gridPos.Add(nextPos);

            nextPos.x += (rowCount * toggle * gridSize);
            gridPos.Add(nextPos);

            currentPos = nextPos;
            toggle *= -1;
        }

        lr.positionCount = gridPos.Count;
        lr.SetPositions(gridPos.ToArray());
    }

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        initLineRenderer(lr);

        MakeGrid(lr, sr, sc, rowCount, colCount);
    }
}
