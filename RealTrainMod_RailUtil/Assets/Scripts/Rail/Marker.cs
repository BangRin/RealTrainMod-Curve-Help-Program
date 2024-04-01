using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Marker : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites = new Sprite[8];
    public Transform[] anchors = new Transform[8];
    public Transform secondAnchor; //2번째 포인트
    public Transform selectAnchor;
    public float angle;

    public void AnchorActive(int num)
    {
        for (int i = 0; i < anchors.Length; i++)
        {
            anchors[i].gameObject.SetActive(false);
        }
        anchors[num].gameObject.SetActive(true);
        selectAnchor = anchors[num];
        spriteRenderer.sprite = sprites[num];
    }

    void Update()
    {
        if(selectAnchor != null)
        {
            lineRenderer.SetPosition(0, selectAnchor.position);
            lineRenderer.SetPosition(1, secondAnchor.position);
        }
    }

    public void DirectionCalc(Vector3 targetPos, Vector3 currentPos, bool advanced)
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(currentPos);
        //float angle = Mathf.Atan2(targetPos.z - mouse.z, targetPos.x - mouse.x) * Mathf.Rad2Deg;
        angle = Vector3.SignedAngle(transform.up, targetPos - mouse, transform.forward);
        //Debug.Log("Angle: " + angle);

        if (advanced)
        {
            secondAnchor.transform.position = mouse;
        }


        if((angle >= 157.5f && angle <= 180) || (angle >= -180 && angle <= -157.5f))
        {
            //Debug.Log("북쪽");
            AnchorActive(0);
        }
        else if((angle >= 112.5f && angle <=157.5f))
        {
            //Debug.Log("북동쪽");
            AnchorActive(1);
        }
        else if(angle >= 67.5f && angle <=112.5f)
        {
            //Debug.Log("동쪽");
            AnchorActive(2);
        }
        else if(angle >= 22.5f && angle <= 67.5f)
        {
            //Debug.Log("남동쪽");
            AnchorActive(3);
        }
        else if(angle <=22.5f && angle >= -22.5f)
        {
            //Debug.Log("남쪽");
            AnchorActive(4);
        }
        else if(angle >= -67.5f && angle <= -22.5f)
        {
            //Debug.Log("남서쪽");
            AnchorActive(5);
        }
        else if(angle >= -112.5f && angle <= -67.5f)
        {
            //Debug.Log("서쪽");
            AnchorActive(6);
        }
        else if(angle >= -157.5 && angle <= -112.5f)
        {
            //Debug.Log("북서쪽");
            AnchorActive(7);
        }
    }
}
