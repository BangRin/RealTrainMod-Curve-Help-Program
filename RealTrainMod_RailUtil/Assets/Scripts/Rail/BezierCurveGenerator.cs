using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurveGenerator : Singleton<BezierCurveGenerator>
{
    public List<RailLine> rails = new List<RailLine>();

    public RailLine railPrefab;
    public RailLine selectRail;
    public int splitCount;

    public void SplitRail()
    {
        if (selectRail == null || splitCount < 1) return;

        Vector3[] controlPoints = selectRail.GetControlPoints(); 

        float tIncrement = 1f / splitCount;
        float t = 0;

        // 이전 세그먼트의 끝점을 새 세그먼트의 시작점으로 사용
        Vector3[] firstHalf, secondHalf;

        for (int i = 0; i < splitCount; i++)
        {
            t = tIncrement * (i + 1); // 각 세그먼트의 t 계산
            SplitBezierCurve(t, controlPoints[0], controlPoints[1], controlPoints[2], controlPoints[3], out firstHalf, out secondHalf);

            // 첫 번째 반을 기반으로 새 RailLine 생성
            RailLine newRail = Instantiate(railPrefab);
            newRail.SetRailLine(firstHalf); // 새로운 세그먼트의 제어점 설정
            rails.Add(newRail); // 리스트에 추가

            // 다음 반복을 위해 secondHalf의 점들을 controlPoints로 설정
            controlPoints = secondHalf;
        }
    }

    // De Casteljau 알고리즘
    public static Vector3 DeCasteljau(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 c = Vector3.Lerp(p2, p3, t);
        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(d, e, t); // 곡선 위의 점
    }

    // 베지어 곡선을 분할하여 각 부분의 제어점 계산
    public static void SplitBezierCurve(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, out Vector3[] firstHalf, out Vector3[] secondHalf)
    {
        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 c = Vector3.Lerp(p2, p3, t);
        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);
        Vector3 pointOnCurve = Vector3.Lerp(d, e, t);

        firstHalf = new Vector3[] { p0, a, d, pointOnCurve };
        secondHalf = new Vector3[] { pointOnCurve, e, c, p3 };
    }
}
