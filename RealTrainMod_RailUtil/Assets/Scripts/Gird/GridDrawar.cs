using System.Collections.Generic;
using UnityEngine;

public class GridDrawar : MonoBehaviour
{
    public GameObject cubePrefab; // Inspector에서 할당
    public int rows;
    public int cols;
    public float cubeSize = 1.0f; // 큐브의 크기

    // 사용할 색상 정의
    public Color colorOne = Color.black;
    public Color colorTwo = Color.white;

    void Start()
    {
        //GenerateGrid();
    }

    void GenerateGrid()
    {

        // 그리드의 중앙을 (0, 0, 0)으로 설정
        int midRow = rows / 2;
        int midCol = cols / 2;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                // 각 셀의 위치 계산, 마인크래프트 좌표 시스템과 동일하게 조정
                Vector3 position = new Vector3(j - midCol, 0, i - midRow);
                // 프리팹을 사용하여 셀 생성
                GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity);
                cube.transform.parent = this.transform; // 생성된 큐브를 GridGenerator 오브젝트의 자식으로 설정

                // 블록 이름 설정
                cube.name = $"{j - midCol}_{i - midRow}";

                // MeshRenderer를 가져와서 색상 설정
                MeshRenderer renderer = cube.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    // 색상을 번갈아 가며 적용
                    renderer.material.color = (i + j) % 2 == 0 ? colorOne : colorTwo;
                }
            }
        }
    }
}
