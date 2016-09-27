using UnityEngine;
using System.Collections;
using System;

public class PredictionTrajectory : MonoBehaviour
{
    /// <summary>
    /// 瞄准的对象
    /// </summary>
    public string tag = "Enemy";
    [HideInInspector]
    public AS_Bullet bullet;
    [HideInInspector]
    public GunHanddle gunHanddle;
    [HideInInspector]
    public Rigidbody rb;

    /// <summary>
    /// 瞄准代理事件
    /// </summary>
    /// <param name="t"></param>
    public delegate void OnPredictionTargetEvent(Transform t);
    /// <summary>
    /// 瞄准事件
    /// </summary>
    public static OnPredictionTargetEvent ptEvent;

    // Use this for initialization
    void Start()
    {
        if (!gunHanddle)
            gunHanddle = GetComponent<GunHanddle>();

        //if (!tank) tank = GetComponent<Tank>();
        //if (!line)
        //    line = GetComponent<LineRenderer>();
        //if (!rb)
        //{
        //    rb = tank.Q.GetComponent<Rigidbody>();
        //}
        //bullet = tank.Q.GetComponent<AS_Bullet>();

        //if (rb.useGravity)
        //{
        //    StartCoroutine(Prediction());
        //}
    }

    public void Update()
    {
        if (gunHanddle.currentBullet && gunHanddle.currentBulletRgbody)
        {
            rb = gunHanddle.currentBulletRgbody;
            bullet = gunHanddle.currentBullet;
            if (rb.useGravity)
            {
                Prediction();
            }
        }
    }



    /// <summary>
    /// 预测目标点
    /// </summary>
    /// <returns></returns>
    void Prediction()
    {
        //yield return null;
        Vector3 gravity = Vector3.zero;
        if (rb.useGravity)
        {
            gravity = Physics.gravity;
        }
        //while (true)
        //{
        Vector3 initialPosition = gunHanddle.CurrentGun.NormalCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Vector3 position = initialPosition;
        Vector3 velocity = gunHanddle.CurrentGun.NormalCamera.transform.forward * bullet.MuzzleVelocity;
        Vector3 lastpos = initialPosition;
        int numStep = (int)bullet.DetectorLength;
        float timeDelta = 1.0f / velocity.magnitude;
        bool targetDetected = false;
        for (int i = 0; i < numStep && !targetDetected; ++i)
        {
            position += velocity * timeDelta + 0.5f * gravity * timeDelta * timeDelta;
            velocity += gravity * timeDelta;
            targetDetected = RayPrediction(lastpos, position, initialPosition, timeDelta);

            lastpos = position;
            if (targetDetected)
            {
                LeanTween.dispatchEvent((int)Events.AimedEnemy);
                break;
            }
        }

        //// print("target detected " + target);
        // line.enabled = true;
        // line.SetPosition(0, initialPosition);
        // line.SetPosition(1, target);
        //  yield return new WaitForSeconds(0.1f);
        //}
    }

    private bool RayPrediction(Vector3 lastpos, Vector3 currentpos, Vector3 initialPosition, float timeDelta)
    {
        RaycastHit[] hits;
        Vector3 dir = (currentpos - lastpos);
        dir.Normalize();

        hits = Physics.RaycastAll(lastpos, dir, 1);

        if (hits.Length > 0)
        {
            //target = hits[0].point;
            for (int i = 0; i < hits.Length; i++)
            {
                var hitter = hits[i].collider.GetComponent<AS_BulletHiter>();
                if (hitter && hitter.RootObject)
                {
                    if (hitter.RootObject.tag == tag)
                    {
                        if (ptEvent != null)
                        {
                            ptEvent(hitter.RootObject.transform);
                        }
                        return true;

                    }

                }
            }

        }

        return false;
    }
}
