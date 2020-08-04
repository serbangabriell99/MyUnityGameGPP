using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorRight : MonoBehaviour
{
    private Animator anim;

    public Camera cam;

    public Camera mainCam;

    private float range;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        range = Vector3.Distance(cam.transform.position, transform.position);

        if (PlayerController.level1win )
        {
            anim.SetBool("win", true);
            mainCam.gameObject.SetActive(false);
            cam.gameObject.SetActive(true);
            Vector3 dir = transform.position - cam.transform.position;
            dir = dir.normalized;
            cam.transform.Translate(dir * 3.0f * Time.deltaTime, Space.World);
            StartCoroutine(CameraChange());
            
        }
        
    }

    IEnumerator CameraChange()
    {
        yield return new WaitForSeconds(3.5f);
        cam.gameObject.SetActive(false);
        mainCam.gameObject.SetActive(true);
    }
}
