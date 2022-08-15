using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    private Camera mainCamera;
    private PickAndDrop pickDrop;

    [SerializeField]
    private RectTransform rectTransform;

    [Tooltip("Make sure you cant turn camera/player when this is on")]
    public bool followMouse;

    [SerializeField]
    private Image imageCursor;

    [SerializeField]
    private Sprite Dot, Hand;

    [HideInInspector]
    public bool craftIsOn;

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        pickDrop = mainCamera.GetComponent<PickAndDrop>();
    }

    private void Update()
    {
            if (followMouse) rectTransform.position = Input.mousePosition;
    }

    private void FixedUpdate()
    {
        int x = Screen.width / 2;
        int y = Screen.height / 2;

        Ray ray;
        ray = mainCamera.ScreenPointToRay(new Vector3(x, y));

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickDrop.pickDistance))
        {
            if (hit.collider.gameObject.GetComponent<Pickable>() || hit.collider.gameObject.GetComponent<Rebirth>())
            {
                imageCursor.sprite = Hand;
            }

            else
            {
                imageCursor.sprite = Dot;
            }
        }

        else
        {
            imageCursor.sprite = Dot;
        }
    }
}
