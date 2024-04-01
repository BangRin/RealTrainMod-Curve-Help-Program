using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

public class RailLine : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public LineRenderer lineStartRenderer;
    public LineRenderer lineEndRenderer;

    public Transform[] transforms = new Transform[4];

    public Material redMaterial;

    public GameObject testBall;

    public Marker startMarker;
    public Marker endMarker;

    public int numPoints;
    public List<Vector3> points = new List<Vector3>();

    private Transform selectedTransform; // 현재 선택된 Transform 객체
    private Vector3 screenSpace;
    private Vector3 offset;

    public float fixedSphereTValue;

    public bool isSplit;


    //레일을 자르고 새로운 곡선을 생성할때 사용
    public void SetRailLine(Vector3[] positions)
    {
        for (int i = 0; i < positions.Length; i++)
        {
            transforms[i].position = positions[i];
        }
        DrawBezierCurve();
    }

    public Vector3[] GetControlPoints()
    {
        Vector3[] vector3s = new Vector3[transforms.Length];
        if (isSplit)
        {
            for (int i = 0; i < transforms.Length; i++)
            {
                vector3s[i] = transforms[i].position;
            }
        }
        else
        {
            vector3s[0] = startMarker.selectAnchor.position;
            vector3s[1] = startMarker.secondAnchor.position;
            vector3s[2] = endMarker.secondAnchor.position;
            vector3s[3] = endMarker.selectAnchor.position;
        }
        return vector3s;
    }


    //마커 시스템이 선의 중앙 혹은 끝에만 설치가 가능하니 0.5 혹은 0만 걸러주는 시스템
    bool IsCloseToHalfOrZero(float value)
    {
        float fractionalPart = value % 1; // 소수부 추출
                                          // 소수부가 0.5에 가까운지 또는 0에 가까운지 확인
        return Mathf.Abs(fractionalPart - 0.5f) < 0.09f || Mathf.Abs(fractionalPart) < 0.09f;
    }


    private void Start()
    {
        //Debug.Log(Vector3.Distance(transforms[0].position, transforms[1].position));

        //DrawHelpers();
    }

    void Update()
    {
        //if (RailCreateManager.Instance.railCreateMode.Equals(false)) return;

        //if (Input.GetMouseButtonDown(0))
        //{
        //    RaycastHit clickhit;
        //    Ray clickray = Camera.main.ScreenPointToRay(Input.mousePosition);


        //    if (Physics.Raycast(clickray, out clickhit))
        //    {
        //        Debug.Log(clickhit.collider.gameObject.name);
        //        Debug.Log(clickhit.point);

        //        for (int i = 0; i < transforms.Length; i++)
        //        {
        //            if (clickhit.transform == transforms[i])
        //            {
        //                selectedTransform = transforms[i];
        //                screenSpace = Camera.main.WorldToScreenPoint(selectedTransform.position);
        //                offset = selectedTransform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
        //                return;
        //            }
        //        }
        //    }
        //}
        //if (Input.GetMouseButtonUp(0))
        //{
        //    if (selectedTransform != null)
        //    {
        //        selectedTransform = null; 
        //    }
        //}

        //if (selectedTransform != null && Input.GetMouseButton(0))
        //{
        //    var curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
        //    var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;
        //    selectedTransform.position = curPosition;
        //}

        //DrawHelpers();
    }

    public void DrawHelpers()
    {

        if(isSplit)
        {
            lineStartRenderer.SetPosition(0, transforms[0].position);
            lineStartRenderer.SetPosition(1, transforms[1].position);

            lineEndRenderer.SetPosition(0, transforms[3].position);
            lineEndRenderer.SetPosition(1, transforms[2].position);
        }
        
        

        DrawBezierCurve();
    }

    void DrawBezierCurve()
    {
        points.Clear();

        for (int i = 0; i <= numPoints; i++)
        {
            float t = i / (float)numPoints;
            Vector3 point = isSplit ? 
                CalculateBezierPoint(t, transforms[0].position, transforms[1].position, transforms[2].position, transforms[3].position) : 
                CalculateBezierPoint(t, startMarker.selectAnchor.position, startMarker.secondAnchor.position, endMarker.secondAnchor.position, endMarker.selectAnchor.position);
            points.Add(point);
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }

    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0; // 첫 번째 항
        p += 3 * uu * t * p1; // 두 번째 항
        p += 3 * u * tt * p2; // 세 번째 항
        p += ttt * p3; // 네 번째 항

        return p;
    }
}
