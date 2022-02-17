using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityJumpHigh : MonoBehaviour
{
    StarterAssets.ThirdPersonController thirdPersonController;

    public float abilityDuration = 10f; //how long till player reverts to normal


    //initial settings variables 
    float jumpHeight;

    //bool to toggle speedup functionality
    bool SuperJump = false;

    public float duration = 3f;
    float elapsedTime = 0f;
    [SerializeField]
    float scaleFactor = 6f;

    //for Lerp
    public AnimationCurve curve;
    float percentageComplete;

    // Start is called before the first frame update
    void Start()
    {
        thirdPersonController = GetComponent<StarterAssets.ThirdPersonController>();

        //set initial default normal values
        jumpHeight = thirdPersonController.JumpHeight;

    }

    public void ActivateJumpHigh()
    {
        if (!SuperJump)
        {
            SuperJump = true;

            StartCoroutine(SuperJumping());
            StartCoroutine(UnSuperJumping());
        }
    }

 
    IEnumerator SuperJumping()
    {
        elapsedTime = 0;
        percentageComplete = 0;

        while (percentageComplete < 1.0f)
        {
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / duration;

            thirdPersonController.JumpHeight = Mathf.Lerp(jumpHeight, jumpHeight * scaleFactor, curve.Evaluate(percentageComplete));
          
            yield return null;
        }
    }

    IEnumerator UnSuperJumping()
    {
        yield return new WaitForSeconds(abilityDuration);

        elapsedTime = 0;
        percentageComplete = 0;

        while (percentageComplete < 1.0f)
        {
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / duration;

            thirdPersonController.JumpHeight = Mathf.Lerp(jumpHeight * scaleFactor, jumpHeight, curve.Evaluate(percentageComplete));

            yield return null;
        }
        SuperJump = false;
        Inventory.AbilityIsActive = false;
    }

}
