using UnityEngine;
using UnityEngine.UIElements;

public class DragCamera : MonoBehaviour
{
    private Vector3 dragOrigin;
    private bool isDragging = false;
    public float zoomSpeed = 4f; // 마우스 휠 줌 속도 조절
    public float minFOV = 15f; // 최소 FOV 값
    public float maxFOV = 130f; // 최대 FOV 값
    public float moveUnit = 0.5f; // 이동 단위

    void Update()
    {
        //if (RailCreateManager.Instance.railCreateMode.Equals(true)) return;
        // 마우스 휠 입력으로 FOV 조절
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.fieldOfView -= scroll * zoomSpeed;
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, minFOV, maxFOV);


        if(RailCreateManager.Instance.railCreateMode.Equals(true))
        {
            if(Input.GetMouseButtonDown(1))
            {
                isDragging = true;
                dragOrigin = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                isDragging = true;
                dragOrigin = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
            }
        }

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            isDragging = false;
        }

        // 드래그 중 카메라 이동
        if (isDragging)
        {
            Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
            Vector3 moveDirection = dragOrigin - currentMousePosition;

            // 좌표를 0.5 단위로 이동하도록 제한
            moveDirection.x = Mathf.Round(moveDirection.x / moveUnit) * moveUnit;
            moveDirection.y = Mathf.Round(moveDirection.y / moveUnit) * moveUnit;
            moveDirection.z = Mathf.Round(moveDirection.z / moveUnit) * moveUnit;

            float newX = transform.position.x + moveDirection.x;
            float newZ = transform.position.z + moveDirection.z;

            transform.position = new Vector3(newX, transform.position.y, newZ);

            LineGrid.Instance.UpdateOffsetGrid((int)newX, (int)newZ);

            //RailCreateManager.Instance.mousePosCube.transform.position = 
            //    new Vector3(newX, RailCreateManager.Instance.mousePosCube.transform.position.y, newZ);

            //RailCreateManager.Instance.myPosition.text = 
            //    $"[{newX}, 0, {-newZ}]";
        }
    }
}
