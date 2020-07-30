using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class BoundaryBehaviour : MonoBehaviour
{

    public static float collisionDisplayTime = 3f;
    public float maxAlphaValue = 0.35f;
    public float lerpSpeed = 5f;
    
    private bool lerp = false;
    private Color currColor;
    
    private Renderer mRenderer;
    private Color renderColor;
    
    // Start is called before the first frame update
    void Start()
    {
        mRenderer = GetComponent<Renderer>();
        renderColor = mRenderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (lerp)
        {
            currColor = Color.Lerp(currColor, renderColor, Time.deltaTime * lerpSpeed);
            mRenderer.material.color = currColor;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<BoundaryDisplayCountDown>().DisplayFor(collisionDisplayTime);
            lerp = true;
            currColor = renderColor;
            currColor.a = 0.35f;
        }
    }
}
