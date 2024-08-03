using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class HolyWater : MonoBehaviour
{
    [SerializeField] private ParticleSystem holyWaterVFX;
    [SerializeField] private GameObject holyWaterHitBox;
    [SerializeField] private Animator anim;
    [SerializeField] private AnimationClip clip;

    private float vfxDuration;

    private void Start()
    {
        holyWaterVFX.Stop(); 
        vfxDuration = holyWaterVFX.main.duration;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StopAllCoroutines();
            holyWaterHitBox.SetActive(true);
            holyWaterVFX.Play();
            StartCoroutine(DisableHitBoxAfterVFX());

            anim.SetInteger("State", 1);
        }
    }

    IEnumerator DisableHitBoxAfterVFX()
    {
        yield return new WaitForSeconds(vfxDuration);
        holyWaterHitBox.SetActive(false);

        yield return new WaitForSeconds(clip.length);  
        anim.SetInteger("State", 0);
    }
}
