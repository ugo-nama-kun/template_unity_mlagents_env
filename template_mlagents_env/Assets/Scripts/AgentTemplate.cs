using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class AgentTemplate : Agent
{
    public Transform Target;

    Rigidbody rBody;

    private void Awake()
    {
        // Disable auto update of the physics engine.
        // Otherwise image rendering is not done every tick
        Physics.autoSimulation = false;

        // Enable Frame rate control. If you use VR, this is ignored.
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = (int)(60.0 * Time.timeScale);
    }

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();    
    }

    // Update is called once per frame
    void Update()
    {
        // Same function with DecisionRequester
        if (this.StepCount % 5 == 0)
        {
            RequestDecision();
        }
        else
        {
            RequestAction();
        }

        // Update Simulation every frame. 
        // This Update enable us to render every frame and hence provide image to the agent
        Physics.Simulate(Time.fixedDeltaTime);

        // Control of the target frame rate. Python client can control timeScale.
        Application.targetFrameRate = (int)(60.0 * Time.timeScale);
    }

    public override void OnEpisodeBegin()
    {
        // Resetting Environment
        if (this.transform.localPosition.y < 0)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.5f, 0);
        }

        Target.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }

    public float forceMultiplier = 10;
    public override void OnActionReceived(float[] vectorAction)
    {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        rBody.AddForce(controlSignal * forceMultiplier);

        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        // Reached target
        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }

        // Fell off platform
        if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");

        // Action Visualization
        Monitor.Log("horizontal", Input.GetAxis("Horizontal"));
        Monitor.Log("vertical", Input.GetAxis("Vertical"));
    }
}
