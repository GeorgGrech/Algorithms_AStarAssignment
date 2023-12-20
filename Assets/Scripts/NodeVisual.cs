using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NodeVisual : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI gCost;
    [SerializeField] TextMeshProUGUI hCost;
    [SerializeField] TextMeshProUGUI fCost;
    [SerializeField] GameObject canvas;

    Renderer objRenderer;

    private void Awake()
    {
        objRenderer = gameObject.GetComponent<Renderer>();
    }

    public enum NodeState
    {
        Unwalkable,
        ClosedSet,
        OpenSet,
        Path
    }

    public void UpdateCosts(int _gCost, int _hCost, int _fCost)
    {
        gCost.text = _gCost.ToString();
        hCost.text = _hCost.ToString();
        fCost.text = _fCost.ToString();
    }

    public void UpdateColour(NodeState state)
    {
        switch(state)
        {
            case NodeState.Unwalkable:
                objRenderer.material.color = Color.red;
                canvas.SetActive(false);
                break;
            case NodeState.ClosedSet:
                objRenderer.material.color = Color.yellow;
                break;
            case NodeState.OpenSet:
                objRenderer.material.color = Color.magenta;
                break;
            case NodeState.Path:
                objRenderer.material.color = Color.green;
                break;
        }
    }
}
