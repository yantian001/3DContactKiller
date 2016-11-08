using UnityEngine;
using System.Collections;
using CnControls;
public class FPSInputMobileController : MonoBehaviour
{

    private GunHanddle gunHanddle;
    private FPSController FPSmotor;

    public string aimHorizontal = "AimHorizontal";
    public string aimVertical = "AimVertical";

    public string moveHorizontal = "Horizontal";
    public string moveVertical = "Vertical";

    public string aimButtom = "Aim";
    public string fireButtom = "Fire1";
    public string switchButton = "Switch";
    public string holdButton = "Hold";
    public float touchSensMult = 0.05f;


    public float smoothly = 15f;
    Vector2 oldPos1, oldPos2;
    // Use this for initialization
    void Start()
    {

    }

    public void Awake()
    {
        Application.targetFrameRate = 60;
        FPSmotor = GetComponent<FPSController>();
        gunHanddle = GetComponent<GunHanddle>();
    }


    // Update is called once per frame
    void Update()
    {

        Vector2 aimDir = new Vector2(CnInputManager.GetAxis(aimHorizontal), CnInputManager.GetAxis(aimVertical)) * touchSensMult;

        // Debug.Log(aimDir);
        FPSmotor.Aim(aimDir);
        ////MouseLock.MouseLocked = false;

        Vector3 moveDir = new Vector3(CnInputManager.GetAxis(moveHorizontal), 0, CnInputManager.GetAxis(moveVertical));
        FPSmotor.Move(moveDir);

        UpdateFOV();

        if (CnInputManager.GetButtonDown(aimButtom))
        {
            gunHanddle.Zoom();
        }

        if (CnInputManager.GetButtonDown(fireButtom))
        {
            gunHanddle.Shoot();
        }

        if (CnInputManager.GetButtonDown(switchButton))
        {
            gunHanddle.SwitchGun();
        }

        if (CnInputManager.GetButtonDown(holdButton))
        {
            gunHanddle.HoldBreath(0);
        }
    }

    void UpdateFOV()
    {
        if (GameValue.staus != GameStatu.InGame)
            return;
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            //Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
            GunHanddle.Instance.ZoomAdjust(Input.GetAxis("Mouse ScrollWheel"));
            //Camera.main.fieldOfView += Input.GetAxis("Mouse ScrollWheel") * smoothly;
        }
        if (Input.touchCount > 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began)
            {
                oldPos1 = Input.GetTouch(0).position;
                oldPos2 = Input.GetTouch(1).position;
            }
            else
            {
                if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
                {
                    var newPos1 = Input.GetTouch(0).position;
                    var newPos2 = Input.GetTouch(1).position;
                    float delta = Vector2.Distance(oldPos1, oldPos2) - Vector2.Distance(newPos1, newPos2);
                    // Camera.main.fieldOfView += delta / smoothly;
                    Debug.Log(delta / smoothly);
                    GunHanddle.Instance.ZoomAdjust(-delta / smoothly);
                    oldPos1 = newPos1;
                    oldPos2 = newPos2;
                }
            }
        }
    }
}
