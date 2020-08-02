using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationBulletSpawner : MonoBehaviour
{
    Transform mMyTransform;

    // Start is called before the first frame update
    void Start()
    {
        mMyTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Spawn(GameObject _prefab, Vector3 _velocity)
    {
        GameObject bullet;
        Vector3 forward = new Vector3(mMyTransform.forward.x, mMyTransform.forward.y, mMyTransform.forward.z);

        bullet = FormationBulletSpawner.Instantiate(_prefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
        bullet.transform.position = mMyTransform.position;
        bullet.GetComponent<Rigidbody2D>().velocity = _velocity;
    }
}
