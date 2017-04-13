using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for handling damage indicator behaviour
/// </summary>

public class DamageIndicator : MonoBehaviour {

    // Defining variables
    // Square bracket tags change how Unity displays attributes in the inspector

    [SerializeField]
    private Animator animator;

    private string textToShow;

    private Text damageText;
    private Vector3 hitPoint;
    private RectTransform canvasRect;


    /* ------------------
     * BUILT-IN FUNCTIONS
     * ------------------ */
    
    // Also used for initialization, but executed before Start
    void Awake() {
        canvasRect = transform.parent.GetComponent<RectTransform>();
    }

    // Use this for initialization
    void Start() {
        damageText = animator.GetComponent<Text>();

        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length - 0.05f);
    }

    // Update is called once per frame
    void Update() {
        GoToHitPoint();

        if (damageText != null)
            damageText.text = textToShow;
    }


    /* ----------------
     * CUSTOM FUNCTIONS
     * ---------------- */
    
    // Sets the text displayed by the damage indicator
    public void SetText(string text) {
        textToShow = text;
    }

    // Sets the point where the enemy was hit
    public void SetHitPoint (Vector3 _hitPoint) {
        hitPoint = _hitPoint;
        //Debug.Break();
    }

    /* Moves this damage indicator to the enemy hit point
     * (Converts 3D hitpoint position to 2D screen space position) */
    void GoToHitPoint () {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(hitPoint);
        Vector3 indicatorPosition = new Vector2(
            ((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
            ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));
        GetComponent<RectTransform>().anchoredPosition = indicatorPosition;
    }
}
