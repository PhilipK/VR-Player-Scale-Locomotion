using UnityEngine;
using System.Collections;

public class PlayerScalingScript : MonoBehaviour {

    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private SteamVR_TrackedObject trackedObj;
    private GameObject cameraRig;

    private Vector3 startingPosition;
    private Vector3 startingScale;
    private bool isPressing;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }

    // Use this for initialization
    void Start()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        cameraRig = GameObject.Find("[CameraRig]");
    }

    // Update is called once per frame
    void Update () {
        if (controller.GetPressUp(triggerButton))
        {
            isPressing = false;
            startingPosition = new Vector3(0,0,0);
            startingScale = new Vector3(0, 0, 0);
        }
        if (controller.GetPressDown(triggerButton))
        {
            isPressing = true;
            startingPosition = controller.transform.pos;
            startingScale = cameraRig.transform.localScale;
        }
        if (isPressing)
        {
            
            var currentScale = cameraRig.transform.localScale;
            Debug.Log("pos:" + controller.transform.pos);
            var currentPos = controller.transform.pos / currentScale.y;
            Debug.Log("currentPos :" + currentPos);
            var distance = currentPos - startingPosition;
            distance = distance * 1.0f;
            var distanceWithScale = (new Vector3(distance.y,distance.y,distance.y) );            
            cameraRig.transform.localScale = startingScale + distanceWithScale;
        }
        
    }
}
