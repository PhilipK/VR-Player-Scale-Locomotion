using UnityEngine;
using System.Collections;

public class ConstantVelocityMove : MonoBehaviour {

    private Valve.VR.EVRButtonId touchPad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
    private Valve.VR.EVRButtonId menu= Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;
    private Valve.VR.EVRButtonId trigger = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private SteamVR_TrackedObject trackedObj;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }


    private GameObject cameraRig;

    private Vector3 movementDirection = new Vector3();

    private bool isMoving = false;
    private float metersPrSec = 1.5f;
    private float disableTime = 0.25f;
    private float difference= 0;

    private float maxDisableTime = 1f;
    

    private float fadeSpeed = 10f;

    private float timeSinceClick = 0;

    private bool shouldBlackout { get { return disableTime > 0; } }


    private bool isSettingTime = false;
    public Camera clearCamera;

    public Camera lookCamera;

    public TextMesh timeInfoTextMesh;

    private Vector3 startPos;
    

    // Use this for initialization
    void Start () {
        cameraRig = transform.parent.gameObject;
        trackedObj = GetComponent<SteamVR_TrackedObject>();

    }

    // Update is called once per frame
    void Update () {

        if (clearCamera.enabled)
        {
            timeSinceClick += Time.deltaTime;
            if(timeSinceClick >= disableTime)
            {
                timeSinceClick = 0.0f;
                clearCamera.transform.gameObject.SetActive(false && shouldBlackout);
            }
        }

        if (controller.GetPressDown(touchPad))
        {
            var axis = controller.GetAxis(touchPad);

            if (Mathf.Abs(axis.x) < 0.5 && Mathf.Abs(axis.y) < 0.5)
            {
                isMoving = false;
                clearCamera.transform.gameObject.SetActive(true && shouldBlackout);
            }
            else if (Mathf.Abs(axis.x) > 0 || Mathf.Abs(axis.y) > 0)
            {
                isMoving = true;
                movementDirection  =axis.x* lookCamera.transform.right + axis.y * lookCamera.transform.forward;
               movementDirection = new Vector3(movementDirection.x, 0, movementDirection.z);

                clearCamera.transform.gameObject.SetActive(true && shouldBlackout);
            }
           
        }
        
        if (isMoving) {
            cameraRig.transform.position = Vector3.Lerp(cameraRig.transform.position, cameraRig.transform.position + movementDirection, Time.deltaTime * metersPrSec);
        }


        if (controller.GetPressDown(trigger))
        {
            isSettingTime = true;
            startPos = controller.transform.pos;
            timeInfoTextMesh.gameObject.SetActive(true);
        }
        if (controller.GetPressUp(trigger))
        {
            isSettingTime = false;
            timeInfoTextMesh.gameObject.SetActive(false);
            disableTime = disableTime + difference;
        }
        if (isSettingTime && controller.GetPress(trigger))
        {
            var curPos = controller.transform.pos;
            var distance = (curPos - startPos).y;
            difference = Mathf.Min(distance, 1.0f) * maxDisableTime;
            var newValue = Mathf.Max(disableTime + difference, 0);
            if (newValue == 0)
            {
                difference = -disableTime;
            }
            timeInfoTextMesh.text = (Mathf.Round((newValue) * 1000)) +"ms";
        }



    }

    
}
