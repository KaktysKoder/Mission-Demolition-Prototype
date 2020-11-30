using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [SerializeField] private GameObject launchPoint;

    private void Awake()
    {
        Transform lounchPointTrans = transform.Find("LaunchPoint");

        launchPoint = lounchPointTrans.gameObject;

        launchPoint.SetActive(false);
    }

    private void OnMouseEnter()
    {
        //print("Slingshot: OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    private void OnMouseExit()
    {
        //print("Slingshot: OnMouseExit()");
        launchPoint.SetActive(false);
    }
}