using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationMainMenu : MonoBehaviour
{
    private Animator myAnim;

    void Awake()
    {
        myAnim = GetComponent<Animator>();
    }

    // �ν�����â���� �̸� applyRootMotion ��Ȱ��ȭ�ϰ� SlideIn �ִϸ��̼��� �����½����� �̺�Ʈ����ؼ� true����� ������ ����� ��.
    // ����, Animator���� ����Ʈ���״� Empty��
    // private���� �̺�Ʈ ��ϵǳ�.
    private void TurnOnRootMotion()
    {
        myAnim.applyRootMotion = true;
    }

    // ĳ���� SlideIn���߿� ���
    // Ground�� Animator�� applyRootMotion �̸� ��Ȱ��ȭ�س�����. üũ�س����� ���� �̻���
    private void AppearGround()
    {
        MainMenuAnimationsController.instance.GroundSlideIn();
    }

    private void ThunderEffect()
    {
        MainMenuAnimationsController.instance.ActivateThunderFX();
    }


    private void HeroAppearSound()
    {
        MainMenuAnimationsController.instance.PlayHeroAppearSound();
    }
}
