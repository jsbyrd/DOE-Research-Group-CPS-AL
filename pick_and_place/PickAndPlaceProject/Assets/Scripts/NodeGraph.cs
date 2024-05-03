using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGraph {
    private const float MAX_DISTANCE_FROM_ROBOT = 0.35f;
    private const int NO_EDGE = 0;
    private float[,] graph;
    private GameObject target;
    private GameObject[] robots;
    private GameObject[] nodes;

    public NodeGraph(GameObject[] targetPlacements, GameObject target, GameObject endPlacement, GameObject[] robots) {
        this.robots = robots;
        int len = targetPlacements.Length;
        // Include target with target placements
        GameObject[] locations = new GameObject[len+2];
        locations[0] = target;
        locations[len+1] = endPlacement;
        this.nodes = locations;
        for (int i = 0; i < len; i++) locations[i+1] = targetPlacements[i];
        len += 2;

        // Create adjancency matrix
        this.graph = new float[len, len];
        for (int i = 0; i < len; i++) {
            for (int j = 0; j < len; j++) {
                if (i == j) {
                    this.graph[i, j] = NO_EDGE;
                }
                else {
                    GameObject placement1 = locations[i];
                    GameObject placement2 = locations[j];
                    Vector3 v1 = placement1.transform.position;
                    Vector3 v2 = placement2.transform.position;
                    float distance = GetMagnitude(v1, v2);
                    this.graph[i, j] = (DoesEdgeExist(v1, v2, this.robots)) ? distance : NO_EDGE;
                }
                // Debug.Log("(" + i + ", " + j + "): " + graph[i,j]);
            }
        }
    }

    private float GetMagnitude(Vector3 v1, Vector3 v2) {
        return (float) Mathf.Sqrt(Mathf.Pow((v1.x-v2.x), 2) + Mathf.Pow((v1.z-v2.z), 2)); 
    }

    // Find a robot that can take the target object from start to end. If no such robot exists, there is no edge
    private bool DoesEdgeExist(Vector3 start, Vector3 end, GameObject[] robots) {
        for (int i = 0; i < robots.Length; i++) {
            Vector3 robotLocation = robots[i].transform.position;
            float d1 = GetMagnitude(robotLocation, start);
            float d2 = GetMagnitude(robotLocation, end);
            if (d1 < MAX_DISTANCE_FROM_ROBOT && d2 < MAX_DISTANCE_FROM_ROBOT) return true;
        }
        return false;
    }

    private int MinDistance(float[] dist, bool[] sptSet) {
        float min = int.MaxValue;
        int minIndex = -1;
        for (int v = 0; v < dist.Length; v++) {
            if (sptSet[v] == false && dist[v] <= min) {
                min = dist[v];
                minIndex = v;
            }
        }
        return minIndex;
    }

    // Implement's Dijkstra's path-finding algorithm and returns a list/queue of instructions to be executed
    public InstructionList Dijkstra(int src) {
        int len = graph.GetLength(0);
        int[] path = new int[len];
        float[] dist = new float[len];
        bool[] sptSet = new bool[len];
        for (int i = 0; i < len; i++) {
            dist[i] = int.MaxValue;
            sptSet[i] = false;
        }
        dist[src] = 0;

        for (int count = 0; count < len-1; count++) {
            int u = MinDistance(dist, sptSet);
            sptSet[u] = true;
            for (int v = 0; v < len; v++) {
                if (!sptSet[v]
                    && graph[u, v] != NO_EDGE
                    && dist[u] != int.MaxValue
                    && dist[u] + graph[u, v] < dist[v]) {
                    dist[v] = dist[u] + graph[u, v];
                    path[v] = u;
                }
            }
        }
        InstructionList list = new InstructionList();
        int index = len-1;
        while (index != 0) {
            list.AddFirst(new Instruction(path[index], index, robots, nodes));
            index = path[index];
        }

        return list;
    }
}