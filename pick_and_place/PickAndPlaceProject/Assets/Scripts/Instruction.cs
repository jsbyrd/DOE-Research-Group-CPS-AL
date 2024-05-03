using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instruction {
    ROSLock rosLock;
    private int startIndex;
    private int endIndex;
    private GameObject robot;
    private GameObject[] nodes;
    public Instruction next;
    public bool isBusy;
    private GameObject target;

    public Instruction(int startIndex, int endIndex, GameObject[] robots, GameObject[] nodes) {
        this.startIndex = startIndex;
        this.endIndex = endIndex;
        this.robot = FindRobot(startIndex, endIndex, robots, nodes);
        this.next = null;
        this.nodes = nodes;
        this.isBusy = true;
    }

    public void ExecuteInstruction() {
        GameObject target = nodes[0];
        PrintInstructionInfo();
        GameObject endPlacement = nodes[endIndex];
        TrajectoryPlanner tp = robot.GetComponent<TrajectoryPlanner>();
        // Tell arm to move target to endplacement
        tp.PublishJointsCustom(target, endPlacement);
    }

    private GameObject FindRobot(int startIndex, int endIndex, GameObject[] robots, GameObject[] nodes) {
        GameObject closestRobot = robots[0]; // Default value
        float smallestDistance = int.MaxValue;

        for (int i = 0; i < robots.Length; i++) {
            float dist1 = GetMagnitude(robots[i].transform.position, nodes[startIndex].transform.position);
            float dist2 = GetMagnitude(robots[i].transform.position, nodes[endIndex].transform.position);
            if (dist1 + dist2 < smallestDistance) {
                closestRobot = robots[i];
                smallestDistance = dist1 + dist2;
            }
        }
        return closestRobot;
    }

    private float GetMagnitude(Vector3 v1, Vector3 v2) {
        return (float) Mathf.Sqrt(Mathf.Pow((v1.x-v2.x), 2) + Mathf.Pow((v1.z-v2.z), 2)); 
    }

    public void PrintInstructionInfo() {
        Debug.Log("Start Node: " + startIndex + ", End Node: " + endIndex);
        Debug.Log("Start: " + nodes[0].transform.position);
        Debug.Log("End: " + nodes[endIndex].transform.position);
        Debug.Log("Robot Position: " + robot.transform.position);
    }

    public bool IsCurrentlyBusy() {
        return this.isBusy;
    }
}