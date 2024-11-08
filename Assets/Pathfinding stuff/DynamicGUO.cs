using UnityEngine;
using Pathfinding;
using com.spacepuppy.Pathfinding;

[RequireComponent(typeof(GraphUpdateScene))]
public class DynamicGUO : MonoBehaviour
{
    private GraphUpdateScene _graph;
    private GraphUpdateObject _guo;
    private float _t = 0f;

    private void Start()
    {
        _graph = this.GetComponent<GraphUpdateScene>();
        Sync();
    }

    private void Update()
    {
        _t += Time.deltaTime;
        if (_t > 0.5f)  // Adjust the update frequency to balance performance and responsiveness
        {
            Sync();
            _t = 0f;
        }
    }

    private void Sync()
    {
        // Create a layer mask that includes only the avoidance layer
        int avoidanceLayer = LayerMask.GetMask("AvoidanceLayer");

        if (_guo != null) AstarPath.active.UpdateGraphs(_guo);

        // Ensure only objects in the avoidance layer are considered
        _guo = _graph.GetGUO();
        _guo.bounds = GetBoundsForLayer(avoidanceLayer);
        AstarPath.active.UpdateGraphs(_guo);

        _graph.InvertSettings();
        _guo = _graph.GetGUO();
        _graph.InvertSettings();
    }

    private Bounds GetBoundsForLayer(int layerMask)
    {
        // Get all colliders on the object
        Collider2D[] colliders = GetComponents<Collider2D>();

        foreach (var col in colliders)
        {
            // Check if the collider belongs to the avoidance layer
            if (col.gameObject.layer == layerMask)
            {
                return col.bounds; // Return the bounds of the avoidance collider
            }
        }

        return new Bounds(); // Default empty bounds if no collider found
    }
    }
