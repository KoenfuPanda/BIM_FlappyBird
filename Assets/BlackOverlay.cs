using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackOverlay : MonoBehaviour
{
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void FadeIn()
    {
        _animator.SetTrigger("In");
    }

    public void FadeOut()
    {
        _animator.SetTrigger("Out");
    }
}
