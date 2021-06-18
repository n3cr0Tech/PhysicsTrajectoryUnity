using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{

    public GameObject firePoint;
    public GameObject ballPrefab;    
    public float rotationSpeed;
    
    private TrajectorySim trajectorySim;
    private Vector3 prevCannonPosition;
    private Quaternion prevCannonRotation;

    // Start is called before the first frame update
    public void Load(TrajectorySim _trajectorySim)
    {
        trajectorySim = _trajectorySim;
    }

    // Update is called once per frame
    void Update()
    {        
        EnsureDrawBallTrajectory();
    }

    public void UpdateTransform(float horInput, float vertInput)
    {        
        transform.Rotate(
            -vertInput * rotationSpeed,
            horInput * rotationSpeed,
            0.0f
        );        
    }

    public void Shoot()
    {
        GameObject ball = Instantiate(ballPrefab, firePoint.transform.position, Quaternion.identity);
        ball.GetComponent<Rigidbody>().AddForce(CalculateForce(), ForceMode.Impulse);        
    }

    private Vector3 CalculateForce()
    {
        float power = 3.0f;
        return transform.forward * power;
    }

    private void EnsureDrawBallTrajectory()
    {
        if (prevCannonRotation != transform.rotation || prevCannonPosition != transform.position)
        {
            trajectorySim.PredictPhysics(ballPrefab, firePoint.transform.position, CalculateForce());
        }
        prevCannonPosition = transform.position;
        prevCannonRotation = transform.rotation;
    }

}
