using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class DriverAgent : Agent
{
    private DriverControl driverController;
    [SerializeField] private Transform checkpointTransform;
    [SerializeField] private Transform startPositionTransform;
    //private GameObject[] checkpoints;
    public override void  OnEpisodeBegin()
    {
        float x = Random.Range(startPositionTransform.localPosition.x-22.0f, startPositionTransform.localPosition.x+22.0f);
        float z = Random.Range(startPositionTransform.localPosition.z-17.0f, startPositionTransform.localPosition.z+17.0f);
        transform.localPosition=new Vector3(x,0,z);
        foreach(GameObject checkpoint in GameObject.FindGameObjectsWithTag("Checkpoint"))
        {
            MeshRenderer MeshComponent1 = checkpoint.gameObject.GetComponent<MeshRenderer>();
            MeshComponent1.enabled=true;
            Debug.Log("reset");
        }
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(checkpointTransform.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        driverController.motor=actions.ContinuousActions[0];
        driverController.steering=actions.ContinuousActions[1];

        driverController.FixedUpdate();
    }
    private void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "Checkpoint")
        {
           MeshRenderer MeshComponent = other.gameObject.GetComponent<MeshRenderer>();
           MeshComponent.enabled=false;
            AddReward(3f);
        }
        if (other.gameObject.tag == "End")
        {
            AddReward(100f);
             EndEpisode();
        }
        if (other.gameObject.tag == "Wall")
        {
            AddReward(-100f);
            EndEpisode();
        }
        
    }
    

}
