using System;
using System.Collections.Generic;

public class Solver
{
    public static float distance((float x, float y) coordA, (float x, float y) coordB)
        => MathF.Sqrt((coordB.x - coordA.x) * (coordB.x - coordA.x) + (coordB.y - coordA.y) * (coordB.y - coordA.y));

    public int Move(
    Dictionary<int, (int id, float x, float y, int[] conns)> nodes,
    (float x, float y) target,
    (float x, float y) player,
    int targetLocId,
    int playerLocId)
    {
        var distances = new Dictionary<int, float>();
        var previousNodes = new Dictionary<int, int>();
        var queue = new PriorityQueue<int, float>();

        foreach (var node in nodes) {
            distances[node.Key] = float.PositiveInfinity;
            previousNodes[node.Key] = -1;
        }

        distances[playerLocId] = 0;
        queue.Enqueue(playerLocId, 0);

        while (queue.TryDequeue(out int currentNode, out float currentDistance)) {
            if (currentNode == targetLocId)
                break;

            if (currentDistance > distances[currentNode])
                continue;

            var currentNodeData = nodes[currentNode];

            foreach (var neighborId in currentNodeData.conns) {
                if (!nodes.ContainsKey(neighborId)) continue;

                var neighborData = nodes[neighborId];

                float newDistance =  currentDistance + distance(
                    (currentNodeData.x, currentNodeData.y),
                    (neighborData.x, neighborData.y)
                );

                if (newDistance < distances[neighborId]) {
                    distances[neighborId] = newDistance;
                    previousNodes[neighborId] = currentNode;
                    queue.Enqueue(neighborId, newDistance);
                }
            }
        }

        var path = new List<int>();
        var current = targetLocId;

        while (current != -1) {
            path.Add(current);
            current = previousNodes[current];
        }

        path.Reverse();

        
        int nextNode = -1;
        var playerIndex = path.IndexOf(playerLocId);

        if (playerIndex != -1 && playerIndex < path.Count - 1) {
            nextNode = path[playerIndex + 1];
        }

        Console.WriteLine(nextNode);
        return nextNode;
    }

}