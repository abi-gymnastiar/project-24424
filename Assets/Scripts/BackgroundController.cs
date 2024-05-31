using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public GameObject[] backgrounds;
    private Vector2[] startPos = new Vector2[7];
    private float[] length = new float[7];
    public GameObject cam;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            startPos[i] = new Vector2(backgrounds[i].transform.position.x, backgrounds[i].transform.position.y);
            length[i] = backgrounds[i].GetComponent<SpriteRenderer>().bounds.size.x * backgrounds[i].transform.localScale.x;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float temp;
            if (i == 0) { temp = 1f; }
            else if (i > 0 && i <= 3) { temp = 0.92f; }
            else if (i < 6) { temp = 0.9f; }
            else { temp = 0.7f; }
            Vector2 parallaxEffect = new Vector2(1 - (i * 0.125f), temp);
            //float parallaxEffect = 1 - (i * 0.125f);
            Vector2 distance = new Vector2(cam.transform.position.x * parallaxEffect.x, cam.transform.position.y * parallaxEffect.y);
            float movement = cam.transform.position.x * (1 - parallaxEffect.x);
            backgrounds[i].transform.position = new Vector3(startPos[i].x + distance.x, startPos[i].y + distance.y, backgrounds[i].transform.position.z);

            if (movement > startPos[i].x + length[i])
            {
                startPos[i].x += length[i];
            }
            else if (movement < startPos[i].x - length[i])
            {
                startPos[i].x -= length[i];
            }
        }
    }
}
