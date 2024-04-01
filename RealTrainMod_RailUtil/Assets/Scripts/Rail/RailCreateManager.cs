using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class RailCreateManager : Singleton<RailCreateManager>
{
    public bool railCreateMode;

    [Header("좌표 이동 UI")]
    public TMP_InputField xInput;
    public TMP_InputField zInput;

    public TextMeshProUGUI myPosition;
    public Transform mousePosCube;

    public Marker prefebMarker;
    public RailLine prefebRail;

    Marker sp;
    Marker ep;

    float moveUnit = 1f; // 이동 단위

    public void MoveCoord()
    {
        float x = ParseInput(xInput.text + ".5");
        float z = ParseInput(zInput.text+ ".5");

        Vector3 vec = ConvertMinecraftToUnityCoords(x, 10, z);

        Camera.main.transform.position = vec;
        //mousePosCube.position = new Vector3(vec.x, mousePosCube.position.y, vec.z);
        myPosition.text = $"[{vec.x}, 0, {-vec.z}]";
        LineGrid.Instance.UpdateOffsetGrid((int)vec.x, (int)vec.z);
    }

    private float ParseInput(string input)
    {
        return string.IsNullOrEmpty(input) ? 0f : float.Parse(input);
    }

    Vector3 ConvertMinecraftToUnityCoords(float x, float y, float z)
    {
        //Minecraft Coordinates to Unity Coords
        return new Vector3(x, y, -z);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    Vector3 currentMousePosition;
    bool selectChange = false;
    bool advancedSettingMode = false;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F3))
            railCreateMode = !railCreateMode;
        if(Input.GetKeyDown(KeyCode.LeftShift))
            selectChange = !selectChange;
        if(Input.GetKeyDown(KeyCode.LeftControl))
            advancedSettingMode = !advancedSettingMode;

        if (railCreateMode)
        {
            if(Input.GetMouseButtonDown(0))
            {
                currentMousePosition = Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));

                if(sp == null && ep == null)
                {
                    sp = Instantiate(prefebMarker);
                    sp.transform.position = mousePosCube.position;
                }
                else if(sp != null && ep == null)
                {
                    ep = Instantiate(prefebMarker);
                    ep.transform.position = mousePosCube.position;
                }
            }
            else if (Input.GetMouseButton(0))
            {
                TargetPosUpdate(mousePosCube, currentMousePosition);

                if((sp != null && ep == null)|| selectChange)
                {
                    sp.DirectionCalc(sp.transform.position, new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y), advancedSettingMode);
                }
                else if(ep != null && !selectChange)
                {
                    ep.DirectionCalc(ep.transform.position, new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y), advancedSettingMode);
                }
            }
            else
            {
                currentMousePosition = Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));

                TargetPosUpdate(mousePosCube, currentMousePosition);
            }
        }
        else
        {
            currentMousePosition = Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));

            TargetPosUpdate(mousePosCube, currentMousePosition);
        }
    }

    void TargetPosUpdate(Transform target, Vector3 currentPos)
    {
        //Debug.Log(currentPos);

        bool isZeroZ = currentPos.z < 0f && currentPos.z > -1f;
        bool isZeroX = currentPos.x < 0f && currentPos.x > -1f;

        int x, z;

        x = (int)currentPos.x;
        z = (int)currentPos.z;

        currentPos.x = x;
        currentPos.z = z;

        currentPos.x = isZeroX ? -ParseInput(x.ToString() + ".5") : ParseInput(x.ToString() + ".5");
        currentPos.z = isZeroZ ? -ParseInput(z.ToString() + ".5") : ParseInput(z.ToString() + ".5");

        target.position = new Vector3(currentPos.x, target.position.y, currentPos.z);

        myPosition.text = $"[{currentPos.x}, 0, {-currentPos.z}]";

    }
}
