using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Marker : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites = new Sprite[8];
    public Transform[] anchor = new Transform[8];

    public float angle;

    public void AnchorActive(int num)
    {
        for (int i = 0; i < anchor.Length; i++)
        {
            anchor[i].gameObject.SetActive(false);
        }
        anchor[num].gameObject.SetActive(true);
        spriteRenderer.sprite = sprites[num];
    }

    public void DirectionCalc(Vector3 targetPos, Vector3 currentPos)
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(currentPos);
        //float angle = Mathf.Atan2(targetPos.z - mouse.z, targetPos.x - mouse.x) * Mathf.Rad2Deg;
        angle = Vector3.SignedAngle(transform.up, targetPos - mouse, transform.forward);
        //Debug.Log("Angle: " + angle);

        if((angle >= 157.5f && angle <= 180) || (angle >= -180 && angle <= -157.5f))
        {
            //Debug.Log("����");
            AnchorActive(0);
        }
        else if((angle >= 112.5f && angle <=157.5f))
        {
            //Debug.Log("�ϵ���");
            AnchorActive(1);
        }
        else if(angle >= 67.5f && angle <=112.5f)
        {
            //Debug.Log("����");
            AnchorActive(2);
        }
        else if(angle >= 22.5f && angle <= 67.5f)
        {
            //Debug.Log("������");
            AnchorActive(3);
        }
        else if(angle <=22.5f && angle >= -22.5f)
        {
            //Debug.Log("����");
            AnchorActive(4);
        }
        else if(angle >= -67.5f && angle <= -22.5f)
        {
            //Debug.Log("������");
            AnchorActive(5);
        }
        else if(angle >= -112.5f && angle <= -67.5f)
        {
            //Debug.Log("����");
            AnchorActive(6);
        }
        else if(angle >= -157.5 && angle <= -112.5f)
        {
            //Debug.Log("�ϼ���");
            AnchorActive(7);
        }
    }
}
