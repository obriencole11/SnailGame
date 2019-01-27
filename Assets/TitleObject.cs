using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleObject : MonoBehaviour
{

    public Image logo;
    public Image ggjLogo;
    public List<Image> controls = new List<Image>();
    public List<Image> undoControls = new List<Image>();
    public Text controlText;
    public Text undoText;
    public Text resetText;

    void Awake() {
        GameManager.Instance.titleObject = this;

        foreach (Image control in undoControls) {
            control.color = new Color(control.color.r, control.color.g, control.color.b, 0.0f);
            controlText.color = new Color(controlText.color.r, controlText.color.g, controlText.color.b, 0.0f);
        }
        foreach (Image control in controls) {
            control.color = new Color(control.color.r, control.color.g, control.color.b, 0.0f);
            undoText.color = new Color(controlText.color.r, controlText.color.g, controlText.color.b, 0.0f);
            resetText.color = new Color(controlText.color.r, controlText.color.g, controlText.color.b, 0.0f);
        }
    }

    public void FadeInLogo(float t) {
        logo.color = new Color(logo.color.r, logo.color.g, logo.color.b, Mathf.Clamp01(t));
    }

    public void FadeOutLogo(float t) {
        logo.color = new Color(logo.color.r, logo.color.g, logo.color.b, 1 - Mathf.Clamp01(t));
    }

    public void FadeInGGJLogo(float t) {
        ggjLogo.color = new Color(ggjLogo.color.r, ggjLogo.color.g, ggjLogo.color.b, Mathf.Clamp01(t));
    }

    public void FadeOutGGJLogo(float t) {
        ggjLogo.color = new Color(ggjLogo.color.r, ggjLogo.color.g, ggjLogo.color.b, 1 - Mathf.Clamp01(t));
    }

    public void FadeInControls(float t) {
        foreach (Image control in controls) {
            control.color = new Color(control.color.r, control.color.g, control.color.b, Mathf.Clamp01(t));
            controlText.color = new Color(controlText.color.r, controlText.color.g, controlText.color.b, Mathf.Clamp01(t));
        }
    }
    public void FadeInUndoControls(float t) {
        foreach (Image control in undoControls) {
            control.color = new Color(control.color.r, control.color.g, control.color.b, Mathf.Clamp01(t));
            undoText.color = new Color(controlText.color.r, controlText.color.g, controlText.color.b, Mathf.Clamp01(t));
            resetText.color = new Color(controlText.color.r, controlText.color.g, controlText.color.b, Mathf.Clamp01(t));
        }
        
    }
    public void FadeOutUndoControls(float t) {
        foreach (Image control in undoControls) {
            control.color = new Color(control.color.r, control.color.g, control.color.b, 1 - Mathf.Clamp01(t));
            undoText.color = new Color(controlText.color.r, controlText.color.g, controlText.color.b, 1 - Mathf.Clamp01(t));
            resetText.color = new Color(controlText.color.r, controlText.color.g, controlText.color.b, 1 - Mathf.Clamp01(t));
        }
    }
}
