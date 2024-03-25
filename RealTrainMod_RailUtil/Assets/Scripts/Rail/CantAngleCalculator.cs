using UnityEngine;

public class CantAngleCalculator : MonoBehaviour
{
    // 설정 가능한 변수: 최고 속도(km/h) 및 곡선의 반경(m)
    public double speedKmh = 200;
    public double radius = 2000;

    // Start is called before the first frame update
    void Start()
    {
        // 캔트 각도 계산 및 출력
        double cantAngleDegrees = CalculateCantAngle(speedKmh, radius);
        Debug.Log($"Required cant angle for {speedKmh} km/h: {cantAngleDegrees} degrees");
    }

    private double CalculateCantAngle(double speedKmh, double radius)
    {
        // 중력 가속도
        double g = 9.81;

        // 속도를 m/s로 변환
        double speedMs = speedKmh * 1000 / 3600;

        // 캔트 각도 계산
        double thetaRadians = Mathf.Atan((float)((speedMs * speedMs) / (g * radius)));

        // 라디안을 도로 변환
        double thetaDegrees = thetaRadians * Mathf.Rad2Deg;

        return thetaDegrees;
    }
}
