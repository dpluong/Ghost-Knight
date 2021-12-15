using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterfallController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField] private Texture[] textures;

    private int animationSteps;

    [SerializeField] private float fps = 30f;
    private float fpsCounter;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        fpsCounter += Time.deltaTime;
        if (fpsCounter >= 1f / fps)
        {
            animationSteps ++;
            if (animationSteps == textures.Length)
            {
                animationSteps = 0;
                Debug.Log("Line render");
            }
            lineRenderer.material.SetTexture("_MainTex", textures[animationSteps]);
           
            fpsCounter = 0f;
        }
    }
}
