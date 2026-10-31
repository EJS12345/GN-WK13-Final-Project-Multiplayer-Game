using System.Collections;
using UnityEngine;

public class LobbyPanelBase : MonoBehaviour
{
    [field: SerializeField, Header("LobbyPanelBase Vars")]
    public LobbyPanelType PanelType { get; private set; }
    [SerializeField] private Animator panelAnimator;

    protected LobbyUIManager lobbyUIManager;

    public enum LobbyPanelType
    {
        None,
        CreateNicknamePanel,
        MiddleSectionPanel
    }

    public virtual void InitPanel(LobbyUIManager uiManager)
    {
        lobbyUIManager = uiManager;
    }

    public void ShowPanel()
    {
        const string POP_IN_CLIP_NAME = "In";
        CallAnimationCoroutine(POP_IN_CLIP_NAME, true);
    }

    protected void ClosePanel()
    {
        const string POP_OUT_CLIP_NAME = "Out";
        CallAnimationCoroutine(POP_OUT_CLIP_NAME, false);
    }

    private void CallAnimationCoroutine(string clipName, bool state)
    {
        // Diverted internally to bypass the missing static Utils definition
        StartCoroutine(PlayAnimAndSetStateWhenFinished(gameObject, panelAnimator, clipName, state));
    }

    private IEnumerator PlayAnimAndSetStateWhenFinished(GameObject target, Animator animator, string clipName, bool finalActiveState)
    {
        if (finalActiveState)
        {
            target.SetActive(true);
        }

        if (animator != null && animator.isActiveAndEnabled)
        {
            animator.Play(clipName);

            // Wait for Animator to evaluate the state change
            yield return null;

            // Wait out the dynamic length of the "In" or "Out" clip execution
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        }

        if (!finalActiveState)
        {
            target.SetActive(false);
        }
    }
}
