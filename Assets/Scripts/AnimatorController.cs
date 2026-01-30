using DG.Tweening;
using UnityEngine;

public class AnimatorController : BasicActor
{
    private GameObject panelBlack;
    private Animator imgCinematic;

    AnimatorStateInfo stateInfo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    protected override void Awake()
    {
        panelBlack = GameObject.Find("CanvaUI").transform.Find("PanelBlack").gameObject;
        imgCinematic = panelBlack.GetComponentInChildren<Animator>(true);
        base.Awake();
        entity.AddAction<float>("passCinematic", PassCinematic);
        entity.AddAction("fadeOut", FadeOut);
        entity.AddAction("fadeInt", FadeIn);
    }

    private void Update()
    {
        if (imgCinematic.isActiveAndEnabled)
        {
            stateInfo = imgCinematic.GetCurrentAnimatorStateInfo(0); // 0 = capa base
            if (stateInfo.normalizedTime >= 1f && !imgCinematic.IsInTransition(0))
            {
                continueStep = true;
            }
        }
    }

    private void PassCinematic(float cinematic)
    {
        panelBlack.gameObject.SetActive(true);
        panelBlack.GetComponent<CanvasGroup>().alpha = 1;
        imgCinematic.gameObject.SetActive(true);
        imgCinematic.SetInteger("Cinematic", ((int)cinematic));
        continueStep = false;
    }

    public void EndCinematic()
    {
        continueStep = true;

    }

    private void FadeOut()
    {
        // Difumina poco a poco hasta alpha = 0 en 1 segundo
        panelBlack.GetComponent<CanvasGroup>().DOFade(0f, 1.5f).SetEase(Ease.Linear);
    }

    private void FadeIn()
    {
        // Difumina poco a poco hasta alpha = 0 en 1 segundo
        panelBlack.GetComponent<CanvasGroup>().DOFade(1f, 1.5f).SetEase(Ease.Linear);
    }

    public override void NormalMoving()
    {
        base.NormalMoving();
        panelBlack.gameObject.SetActive(false);
        imgCinematic.gameObject.SetActive(false);
        Destroy(gameObject);
    }

}
