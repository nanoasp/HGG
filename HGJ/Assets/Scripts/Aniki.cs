using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Timeline;

public class Aniki : MonoBehaviour
{
    public GameObject AnikiMarker;
    Transform mTransform;
    Transform mMarkerTransform;

    public float mSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        mMarkerTransform = AnikiMarker.GetComponent<Transform>();
        mTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 marker_pos = mMarkerTransform.position;
        Vector2 my_pos = mTransform.position;

        Vector2 dir = (marker_pos - my_pos);
        dir.Normalize();

        mTransform.position = new Vector2(mTransform.position.x, mTransform.position.y) + (dir * mSpeed * Time.fixedDeltaTime);
    }
}
