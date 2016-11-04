using UnityEngine;
using GameDataEditor;

[RequireComponent(typeof(AudioSource))]


public class Gun : MonoBehaviour
{
    public int id = 0;
    public bool Active = true;
    public GameObject Bullets; // Bullet prefeb
    public float LifeTimeBullet = 5;
    public GameObject Shell; // Shell prefeb
    public Transform ShellSpawn; // shell spawing position
    public Camera NormalCamera;// FPS camera
    public float FireRate = 0.2f;
    public float KickPower = 10;
    public float[] ZoomFOVLists;
    public int IndexZoom = 0;
    public Vector2 Offset = Vector2.zero;
    public float CooldownTime = 0.8f;
    public float BoltTime = 0.35f;
    public Texture2D CrosshairImg, CrosshairZoom;
    public bool HideGunWhileZooming = true;
    public AudioClip SoundGunFire;
    public AudioClip SoundBoltEnd;
    public AudioClip SoundBoltStart;
    public AudioClip SoundReloadStart;
    public AudioClip SoundReloadEnd;
    public AudioClip SoundEmpty;
    public float MouseSensitive = 1;
    public float MouseStability = 20.5f;
    public bool Zooming;
    /// <summary>
    /// 初始化的狙击FOV,
    /// </summary>
    public float ZoomFOV = 50f;
    /// <summary>
    /// 当前的FOV ,= zoomFOV - maxMult * sense * forPerMulti
    /// </summary>
    public float currentFOV = 50f;
    /// <summary>
    /// 最大的狙击倍数
    /// </summary>
    public float maxMulti = 4f;

    public float minMulti = 2f;
    /// <summary>
    /// 狙击镜每增加一倍 ,fov减少的值
    /// </summary>
    public float fovPerMulti = 2f;
    public bool ZoomingBeforeReload;
    float Power = 0;
    public float MaxZoom = 6.0f;
    public float Infrared = 8f;
    public bool SemiAuto;
    public bool InfinityAmmo = true;
    public int BulletNum = 1;
    public int Spread = 0;
    public int Clip = 30;
    public int ClipSize = 30;
    public int AmmoIn = 1;
    public int AmmoPack = 90;
    public int AmmoPackMax = 90;
    private float MouseSensitiveZoom = 0.5f;
    private bool boltout;
    private float timefire = 0;
    private int gunState = 0;
    private AudioSource audiosource;
    [HideInInspector]
    public float fovTemp;
    private float cooldowntime = 0;
    private Quaternion rotationTemp;
    public Vector3 positionTemp;
    public string IdlePose = "Idle";
    public string ShootPose = "Shoot";
    public string ReloadPose = "Reload";
    public string BoltPose = "Bolt";
    [HideInInspector]
    public FPSController FPSmotor;
    [HideInInspector]
    public float CurrentZoom = 1f;



    void Start()
    {
        if (GetComponent<Animation>())
            GetComponent<Animation>().cullingType = AnimationCullingType.AlwaysAnimate;
        FPSmotor = transform.root.GetComponentInChildren<FPSController>();

        Zooming = false;
        if (GetComponent<AudioSource>())
        {
            audiosource = GetComponent<AudioSource>();
        }

        if (NormalCamera)
            fovTemp = NormalCamera.GetComponent<Camera>().fieldOfView;

        Clip = ClipSize;
        if (AmmoIn > 1)
            AmmoIn = 1;
    }

    void Awake()
    {
        rotationTemp = this.transform.localRotation;
        this.transform.localPosition = positionTemp - (Vector3.up);

        GDEWeaponData wd = WeaponManager.Instance.GetWeaponById(id);
        if (wd != null)
        {
            Power = wd.GetAttributeCurrentVal(0);
            MaxZoom = wd.GetAttributeCurrentVal(1);
            MouseStability = wd.GetAttributeCurrentVal(2);
            ClipSize = ConvertUtil.ToInt32(wd.GetAttributeCurrentVal(3));
            Infrared = wd.GetAttributeCurrentVal(4);
        }
    }

    public void SetActive(bool active)
    {
        Active = active;
        this.gameObject.SetActive(active);
        //Zooming = false;
        Zoom(false);
        IndexZoom = 0;
        this.transform.localPosition = positionTemp - (Vector3.up);

        if (NormalCamera)
            NormalCamera.GetComponent<Camera>().fieldOfView = fovTemp;

    }

    void FixedUpdate()
    {
        if (!FPSmotor || !Active)
            return;

        float magnitude = FPSmotor.motor.controller.velocity.magnitude * 0.5f;
        magnitude = Mathf.Clamp(magnitude, 0, 10);
        float swaySpeed = 0;
        float sizeY = 0.1f;
        float sizeX = 0.1f;
        // Gun sway volume depending on move velosity.
        if (magnitude > 2)
        {
            swaySpeed = 1.4f;
            sizeY = 0.2f;
            sizeX = 0.2f;
        }
        else
        {
            if (magnitude < 1.0f)
            {
                swaySpeed = 0;
                sizeY = 0.05f;
                sizeX = 0.05f;
            }
            else
            {
                swaySpeed = 1;
                sizeY = 0.1f;
                sizeX = 0.1f;

            }
        }
        float swayY = (Mathf.Cos(Time.time * 10 * swaySpeed) * 0.3f) * sizeY;
        float swayX = (Mathf.Sin(Time.time * 5 * swaySpeed) * 0.2f) * sizeX;
        this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, positionTemp + new Vector3(swayX, swayY, 0), Time.fixedDeltaTime * 4);
        this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, Quaternion.Euler((rotationTemp.eulerAngles.x + (FPSmotor.rotationDif.x)), (rotationTemp.eulerAngles.y + (FPSmotor.rotationDif.y)), (rotationTemp.eulerAngles.z + (FPSmotor.direction.x * 7))), Time.fixedDeltaTime * 3);
    }

    void Update()
    {
        if (HideGunWhileZooming && FPSmotor && NormalCamera.GetComponent<Camera>().enabled)
        {
            FPSmotor.HideGun(!Zooming);
        }

        if (!GetComponent<Animation>() || !Active)
            return;


        switch (gunState)
        {
            case 0:
                // Start Bolt
                if (AmmoIn <= 0)
                {
                    // Check Ammo in clip
                    if (Clip > 0)
                    {
                        GetComponent<Animation>().clip = GetComponent<Animation>()[BoltPose].clip;
                        GetComponent<Animation>().CrossFade(BoltPose, 0.5f, PlayMode.StopAll);
                        gunState = 2;
                        // scope rotation a bit when reloading
                        if (FPSmotor && Zooming)
                        {
                            FPSmotor.CameraForceRotation(new Vector3(0, 0, 20));
                            FPSmotor.Stun(0.2f);
                        }
                        if (SoundBoltStart && audiosource != null)
                        {
                            audiosource.PlayOneShot(SoundBoltStart);
                        }
                        Clip -= 1;
                    }
                    else
                    {
                        gunState = 3;
                    }
                }
                break;
            case 1:
                // Countdown to idle state
                if (Time.time >= cooldowntime + CooldownTime)
                {

                    gunState = 0;
                }
                break;
            case 2:
                GetComponent<Animation>().Play();
                // finish bold animation
                if (GetComponent<Animation>()[BoltPose].normalizedTime > BoltTime)
                {
                    if (Shell && ShellSpawn)
                    {
                        if (!boltout)
                        {
                            GameObject shell = (GameObject)Instantiate(Shell, ShellSpawn.position, ShellSpawn.rotation);
                            shell.GetComponent<Rigidbody>().AddForce(ShellSpawn.transform.right * 2);
                            shell.GetComponent<Rigidbody>().AddTorque(Random.rotation.eulerAngles * 10);
                            GameObject.Destroy(shell, 5);
                            boltout = true;
                            if (FPSmotor && Zooming)
                            {
                                FPSmotor.CameraForceRotation(new Vector3(0, 0, -5));
                                FPSmotor.Stun(0.1f);
                            }
                        }
                    }

                }
                if (GetComponent<Animation>()[BoltPose].normalizedTime > 0.9f)
                {
                    gunState = 0;
                    AmmoIn = 1;
                    GetComponent<Animation>().CrossFade(IdlePose);
                    if (SoundBoltEnd && audiosource != null)
                    {
                        audiosource.PlayOneShot(SoundBoltEnd);
                    }
                    LeanTween.delayedCall(0.5f, () => { Zoom(ZoomingBeforeReload); });

                }
                break;
            case 3:
                // Start Reloading
                if (GetComponent<Animation>()[ReloadPose])
                {
                    if (AmmoPack > 0 || InfinityAmmo)
                    {
                        GetComponent<Animation>().clip = GetComponent<Animation>()[ReloadPose].clip;
                        GetComponent<Animation>().CrossFade(ReloadPose, 0.2f, PlayMode.StopAll);
                        gunState = 4;
                        ZoomingBeforeReload = Zooming;
                        Zoom(false);
                        if (SoundReloadStart && audiosource != null)
                        {
                            audiosource.PlayOneShot(SoundReloadStart);
                        }
                    }
                    else
                    {
                        gunState = 5;
                    }
                }
                else
                {
                    gunState = 0;
                }
                break;
            case 4:

                if (GetComponent<Animation>()[ReloadPose])
                {
                    if (GetComponent<Animation>().clip.name != GetComponent<Animation>()[ReloadPose].name)
                    {
                        GetComponent<Animation>().clip = GetComponent<Animation>()[ReloadPose].clip;
                        GetComponent<Animation>().CrossFade(ReloadPose, 0.5f, PlayMode.StopAll);
                    }
                    if (GetComponent<Animation>()[ReloadPose].normalizedTime > 0.8f)
                    {
                        gunState = 0;
                        Clip = ClipSize;
                        if (Clip > 0)
                        {
                            GetComponent<Animation>().CrossFade(IdlePose);
                            if (SoundReloadEnd && audiosource != null)
                            {
                                audiosource.PlayOneShot(SoundReloadEnd);
                            }
                        }
                    }
                }
                else
                {
                    gunState = 0;
                }
                break;
        }

        if (FPSmotor)
        {
            if (Zooming)
            {
                FPSmotor.sensitivityXMult = MouseSensitiveZoom;
                FPSmotor.sensitivityYMult = MouseSensitiveZoom;
                FPSmotor.stability = MouseStability / 100;
                FPSmotor.Noise = true;
            }
            else
            {
                FPSmotor.sensitivityXMult = MouseSensitive;
                FPSmotor.sensitivityYMult = MouseSensitive;
                FPSmotor.stability = MouseStability / 100;
                FPSmotor.Noise = false;
            }
        }

        if (Zooming)
        {
            if (ZoomFOVLists.Length > 0)
            {

                //MouseSensitiveZoom = ((MouseSensitive * 0.16f) / 10) * (fovTemp / CurrentZoom);
                MouseSensitiveZoom = ((MouseSensitive * 0.16f) / 10) * currentFOV;
                //  NormalCamera.GetComponent<Camera>().fieldOfView += (ZoomFOVLists[IndexZoom] - NormalCamera.GetComponent<Camera>().fieldOfView) / 10;
                //NormalCamera.GetComponent<Camera>().fieldOfView += ((fovTemp / CurrentZoom) - NormalCamera.GetComponent<Camera>().fieldOfView) / 10;
                NormalCamera.GetComponent<Camera>().fieldOfView += (currentFOV - NormalCamera.GetComponent<Camera>().fieldOfView) / 10;
            }
        }
        else
        {
            NormalCamera.GetComponent<Camera>().fieldOfView += (fovTemp - NormalCamera.GetComponent<Camera>().fieldOfView) / 10;
        }

        if (audiosource != null)
        {
            audiosource.pitch = Time.timeScale;
            if (audiosource.pitch < 0.5f)
            {
                audiosource.pitch = 0.5f;
            }
        }

    }

    public void ZoomDelta(int plus)
    {
        if (!Active)
            return;

        if (plus > 0)
        {
            if (IndexZoom < ZoomFOVLists.Length - 1)
            {
                IndexZoom += 1;
            }
        }
        else
        {
            if (IndexZoom > 0)
            {
                IndexZoom -= 1;
            }
        }
    }

    private float currentDelta = 0;
    public void ZoomDelta(float delta)
    {
        if (!Active)
            return;
        //Zooming = true;
        currentDelta += delta;
        currentDelta = Mathf.Min(1, Mathf.Max(0, currentDelta));
        if (currentDelta > 0)
        {
            Zooming = true;
        }
        else
        {
            Zooming = false;
        }
        currentFOV = Mathf.Max(0, ZoomFOV - (maxMulti - minMulti) * currentDelta * fovPerMulti);
        Debug.Log("current FOV :" + currentFOV);
    }

    /// <summary>
    /// 获得当前放大倍数
    /// </summary>
    /// <returns></returns>
    public float GetZoom()
    {
        return minMulti + (maxMulti - minMulti) * currentDelta;
    }

    public void Reload()
    {
        if (gunState == 0)
        {
            AmmoIn = 0;
            Clip = 0;
            gunState = 3;
        }
    }

    public void Zoom()
    {
        Zooming = !Zooming;
        LeanTween.dispatchEvent((int)Events.ZOOM, Zooming);
    }

    public void Zoom(bool z)
    {
        Zooming = z;
        LeanTween.dispatchEvent((int)Events.ZOOM, Zooming);
    }

    public void ZoomToggle()
    {
        Zooming = true;

        if (IndexZoom < ZoomFOVLists.Length - 1)
        {
            IndexZoom += 1;
        }
        else
        {
            Zooming = false;
            IndexZoom = 0;
        }

    }

    public void OffsetAdjust(Vector2 adj)
    {
        Offset += adj;
    }
    public void Shoot()
    {
        if (!Active)
            return;

        if (timefire + FireRate < Time.time)
        {
            if (gunState == 0)
            {
                if (AmmoIn > 0)
                {
                    if (FPSmotor)
                        FPSmotor.Stun(KickPower);
                    if (SoundGunFire && audiosource != null)
                    {
                        audiosource.PlayOneShot(SoundGunFire);
                    }
                    for (int i = 0; i < BulletNum; i++)
                    {
                        if (Bullets)
                        {
                            Vector3 point = NormalCamera.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                            GameObject bullet = (GameObject)Instantiate(Bullets, point, NormalCamera.gameObject.transform.rotation);
                            var asBuulet = bullet.GetComponent<AS_Bullet>();
                            if (asBuulet)
                            {
                                asBuulet.Damage = ConvertUtil.ToInt32(Power);
                            }
                            bullet.transform.forward = NormalCamera.transform.forward + (new Vector3(Random.Range(-Spread, Spread) + Offset.x, Random.Range(-Spread, Spread) + Offset.y, Random.Range(-Spread, Spread)) * 0.001f);
                            Destroy(bullet, LifeTimeBullet);
                        }
                    }
                    boltout = false;
                    GetComponent<Animation>().Stop();
                    GetComponent<Animation>().Play(ShootPose, PlayMode.StopAll);
                    timefire = Time.time;
                    cooldowntime = Time.time;
                    if (!InfinityAmmo)
                    {
                        // Player.CurrentUser.BuyGunAmmo(id, -1);
                    }
                    AmmoIn -= 1;
                    if (!SemiAuto)
                    {
                        gunState = 1;

                    }
                    else
                    {

                        if (Shell && ShellSpawn)
                        {
                            GameObject shell = (GameObject)Instantiate(Shell, ShellSpawn.position, ShellSpawn.rotation);
                            shell.GetComponent<Rigidbody>().AddForce(ShellSpawn.transform.right * 2);
                            shell.GetComponent<Rigidbody>().AddTorque(Random.rotation.eulerAngles * 10);
                            GameObject.Destroy(shell, 5);
                        }

                        if (Clip > 0)
                        {
                            AmmoIn = 1;
                            Clip -= 1;
                        }
                        else
                        {
                            gunState = 3;
                        }
                    }
                }

                if (Clip <= 0 && AmmoIn <= 0)
                {
                    gunState = 3;
                }

            }
            else if (gunState == 5)
            {
                audiosource.PlayOneShot(SoundEmpty);
            }
        }
    }

    void OnGUI()
    {
        if (!Active)
            return;
        GUI.depth = 1;
        if (NormalCamera)
        {
            if (NormalCamera.GetComponent<Camera>().enabled)
            {
                if (!Zooming)
                {
                    if (CrosshairImg)
                    {
                        GUI.color = new Color(1, 1, 1, 0.8f);
                        GUI.DrawTexture(new Rect((Screen.width * 0.5f) - (CrosshairImg.width * 0.5f), (Screen.height * 0.5f) - (CrosshairImg.height * 0.5f), CrosshairImg.width, CrosshairImg.height), CrosshairImg);
                        GUI.color = Color.white;
                    }
                }
                else
                {
                    if (CrosshairZoom)
                    {
                        float scopeSize = (Screen.height * 1.8f);
                        GUI.DrawTexture(new Rect((Screen.width * 0.5f) - (scopeSize * 0.5f), (Screen.height * 0.5f) - (scopeSize * 0.5f), scopeSize, scopeSize), CrosshairZoom);
                        GUI.skin.label.fontSize = 24;
                        GUI.skin.label.normal.textColor = new Color(0, 0, 0, 1);
                        GUI.Label(new Rect((Screen.width * 0.5f) - (CrosshairImg.width * 0.2f) + 30, (Screen.height * 0.5f) - (CrosshairImg.height * 0.2f), 200, 30), "H " + Offset.x + " : V " + Offset.y);

                    }
                }
            }
        }
    }


}
