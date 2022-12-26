using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] bool isPlayerUnit;
    [SerializeField] BattleHUD hud;
    
    public bool IsPlayerUnit { get => isPlayerUnit; }
    public BattleHUD Hud { get => hud; }

    public Pokemon Pokemon { get; set; }

    Image image;
    Vector3 originalPosi;
    Color originalColor;

    private void Awake()
    {
        image = GetComponent<Image>();
        originalPosi = image.transform.localPosition;
        originalColor = image.color;
    }

    public void Setup(Pokemon pokemon)
    {
        Pokemon = pokemon;

        if (isPlayerUnit)
            image.sprite = Pokemon.Base.BackSprite;
        else
            image.sprite = Pokemon.Base.FrontSprite;

        hud.SetData(pokemon);

        image.color = originalColor;
        PlayEnterAnimation();
    }

    public void PlayEnterAnimation()
    {
        if (isPlayerUnit)
        {
            image.transform.localPosition = new Vector3(-650f, originalPosi.y);
        }
        else
        {
            image.transform.localPosition = new Vector3(650f, originalPosi.y);
        }

        Tween enterTween = image.transform.DOLocalMoveX(originalPosi.x, 1f);
        //Tween enterTween = DOTween.To(() => image.transform.localPosition, x => image.transform.localPosition = x, originalPosi, 1f);
        //enterTween.Play();
        StartCoroutine(SkipAnimation(enterTween));
    }

    public void PlaySwitchAnimation()
    {
        image.transform.DOLocalMoveX(-650f, 1f);
    }

    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        
        if (isPlayerUnit)
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPosi.x + 50f, 0.25f));
        }
        else
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPosi.x - 50f, 0.25f));
        }

        sequence.Append(image.transform.DOLocalMoveX(originalPosi.x, 0.25f));
    }

    public void PlayHitAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }

    public void PlayFaintAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(originalPosi.y - 150f, 0.5f));
        sequence.Join(image.DOFade(0f, 0.5f));
    }

    IEnumerator SkipAnimation(Tween enterTween)
    {
        yield return new WaitForSeconds(Time.deltaTime);
        while (enterTween.IsPlaying())
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                enterTween.Complete();
                break;
            }

            yield return null;
        }
    }
}
