using UnityEngine;
using System.Collections;

public class StepPlayerScaling : MonoBehaviour {


    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private Valve.VR.EVRButtonId touchPad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
    private Valve.VR.EVRButtonId menu= Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;

    private SteamVR_TrackedObject trackedObj;
    private GameObject cameraRig;
    public GameObject cameraHead;
    public GameObject indicator;

    private Vector3 newPivotPosition;
    private Vector3 localPosition;

    private bool isSmoothScaling = true;
    private bool withPivot = true;

    public float stepScale = 2f;
    public float scalePrSecond = 2.0f;


    public TextMesh pivotText;
    public TextMesh smoothScaleText;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }

    // Use this for initialization
    void Start()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        cameraRig = GameObject.Find("[CameraRig]");

    }




    public enum Direction{
        left,
        right,
        up,
        down,
        none
    }

    public static Direction GetDirection(Vector2 axis)
    {
        if(axis.x == 0 && axis.y == 0)
        {
            return Direction.none;
        }
        if(Mathf.Abs(axis.x) > Mathf.Abs(axis.y))
        {
            //mostly horizontal
            return axis.x < 0 ? Direction.left : Direction.right;
        }else
        {
            //mostly vertical
            return axis.y < 0 ? Direction.down : Direction.up;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (indicator != null)
        {
            indicator.transform.position = new Vector3(cameraHead.transform.position.x, 0, cameraHead.transform.position.z);
        }

        if (controller.GetPressDown(menu))
        {
            cameraRig.transform.position = new Vector3(0, 0, 0);
            cameraRig.transform.localScale = new Vector3(1, 1, 1);
        }
        var axis = controller.GetAxis(touchPad);
        var direction = GetDirection(axis);
        if (controller.GetPressDown(touchPad))
        {
            if (direction == Direction.left)
            {
                isSmoothScaling = !isSmoothScaling;
                if (smoothScaleText != null)
                {
                    smoothScaleText.text = "Smooth Scaling: " + (isSmoothScaling ? "On" : "Off");
                }
            }
            if (direction == Direction.right)
            {
                withPivot = !withPivot;
                if (pivotText != null)
                {
                    pivotText.text = "Pivot: " + (withPivot ? "On" : "Off");
                }
            }
        }


        var isUp = direction == Direction.up;
        var isDown = direction == Direction.down;
        if (isUp || isDown)
        {
            var shouldScale = isSmoothScaling ? controller.GetPress(touchPad) : controller.GetPressDown(touchPad);

            if (shouldScale) {
                var progress = isSmoothScaling ? Time.deltaTime : 1;
                var scale = isSmoothScaling ? scalePrSecond  : stepScale;
                var scaleFactor = isUp ? scale : (1 / scale);
                var oldScale = cameraRig.transform.localScale;
                var newScale = Vector3.Lerp(oldScale, oldScale * scaleFactor, progress);
                if (withPivot )
                {                    
                    var pivotPointBefore = new Vector3(cameraHead.transform.position.x, 0, cameraHead.transform.position.z);
                    cameraRig.transform.localScale = newScale;
                    var pivotPointAfter = new Vector3(cameraHead.transform.position.x, 0, cameraHead.transform.position.z);
                    var distance = pivotPointAfter - pivotPointBefore;
                    cameraRig.transform.position = cameraRig.transform.position - distance;

                }else
                {
                    cameraRig.transform.localScale = newScale;

                }


            }
        }
    }

}
