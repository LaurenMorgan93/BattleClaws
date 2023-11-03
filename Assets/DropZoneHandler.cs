using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZoneHandler : MonoBehaviour
{
    private Material originalMaterial;
    private string originalName;
    public ParticleSystem particles;

    private float lockedTimer;
    // Start is called before the first frame update
    void Start()
    {
        originalMaterial = GetComponent<Renderer>().material;
        originalName = gameObject.name;
        particles = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lockedTimer >= 0)
        {
            lockedTimer -= Time.deltaTime;
        }
        else
        {
            GetComponent<Renderer>().material = originalMaterial;
            gameObject.name = originalName;
        }

    }

    public void lockDropZone()
    {
        print("lock" + gameObject.name);
        gameObject.name = "locked " + gameObject.name;
        lockedTimer = 30f;
        Material locked = new Material(GetComponent<Renderer>().material);
        particles.Play();
        locked.color = Color.gray;
        GetComponent<Renderer>().material = locked;
    }
}
