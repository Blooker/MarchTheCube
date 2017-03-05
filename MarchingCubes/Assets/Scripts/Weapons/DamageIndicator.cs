using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour {
    [SerializeField]
    private Animator animator;

    private string textToShow;

    private Text damageText;
    private Vector3 hitPoint;
    private RectTransform canvasRect;

    public void SetText(string text) {
        textToShow = text;
    }

    public void SetHitPoint (Vector3 _hitPoint) {
        hitPoint = _hitPoint;
        //Debug.Break();
    }

    void GoToHitPoint () {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(hitPoint);
        //Debug.Log(Camera.main.name);
        if (canvasRect == null)
            canvasRect = transform.parent.GetComponent<RectTransform>();

        Vector3 indicatorPosition = new Vector2(
            ((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
            ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));
        GetComponent<RectTransform>().anchoredPosition = indicatorPosition;
    }

    // Use this for initialization
    void Start() {
        //animator = transform.GetChild(0).GetComponent<Animator>();
        damageText = animator.GetComponent<Text>();

        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length-0.05f);
    }

    // Update is called once per frame
    void Update () {
        GoToHitPoint();

        if (damageText != null)
            damageText.text = textToShow;
	}
}
