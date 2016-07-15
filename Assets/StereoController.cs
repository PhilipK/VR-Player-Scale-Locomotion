using UnityEngine;
using System.Collections;

public class StereoController : MonoBehaviour
{


    private Valve.VR.EVRButtonId touchPad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
    private SteamVR_TrackedObject trackedObj;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }

    private float prevStereo = 0.022f;
    private float stereoConvergence = 10;

    public GameObject cameraRig;

    // Use this for initialization
    void Start()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();

    }

    // Update is called once per frame
    void Update()
    {
        if (cameraRig != null)
        {
            var axis = controller.GetAxis(touchPad);

            if (controller.GetPress(touchPad))
            {
                var movingTowards = new Vector3(axis.x, 0, axis.y);
                cameraRig.transform.position = Vector3.Lerp(cameraRig.transform.position, cameraRig.transform.position + movingTowards, Time.deltaTime);



            }


        }
    }
}
