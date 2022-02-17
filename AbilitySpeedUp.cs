using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// increases speed and sprint speed temporarily
/// </summary>

public class AbilitySpeedUp : MonoBehaviour
{
    StarterAssets.ThirdPersonController thirdPersonController;

    public float abilityDuration = 10f; //how long till player reverts to normal


    //initial settings variables 
    float moveSpeed;
    float sprintSpeed;

    //bool to toggle speedup functionality
    bool speedingUp = false;

    public float duration = 3f;
    float elapsedTime = 0f;
    public float scaleFactor = 5f;

    //for Lerp
    public AnimationCurve curve;
    float percentageComplete;

    //animation reference
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        thirdPersonController = GetComponent<StarterAssets.ThirdPersonController>();

        //set initial default normal values
        moveSpeed = thirdPersonController.MoveSpeed;
        sprintSpeed = thirdPersonController.SprintSpeed;

        animator = GetComponent<Animator>();
    }


    public void ActivateSpeedUp()
    {
        if (!speedingUp)
        {
            //Debug.Log("2 pressed");
            speedingUp = true;

            StartCoroutine(SpeedUp());
            StartCoroutine(SpeedDown());
        }
    }

    //coroutine for speed shift
    //keep changing until full speed reached
    IEnumerator SpeedUp()
    {
        elapsedTime = 0;
        percentageComplete = 0;

        while (percentageComplete < 1.0f)
        {
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / duration;
           
            animator.speed = Mathf.Lerp(1,2,percentageComplete);

            thirdPersonController.MoveSpeed = Mathf.Lerp(moveSpeed, moveSpeed * scaleFactor, curve.Evaluate(percentageComplete));
            thirdPersonController.SprintSpeed = Mathf.Lerp(sprintSpeed, sprintSpeed * scaleFactor, curve.Evaluate(percentageComplete));

            yield return null;
        }
    }   
    
    IEnumerator SpeedDown()
    {
        yield return new WaitForSeconds(abilityDuration);

        elapsedTime = 0;
        percentageComplete = 0;

        while (percentageComplete < 1.0f)
        {
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / duration;

            animator.speed = Mathf.Lerp(2, 1, percentageComplete);

            thirdPersonController.MoveSpeed = Mathf.Lerp(moveSpeed * scaleFactor, moveSpeed, curve.Evaluate(percentageComplete));
            thirdPersonController.SprintSpeed = Mathf.Lerp(sprintSpeed * scaleFactor, sprintSpeed, curve.Evaluate(percentageComplete));

            yield return null;
        }
        speedingUp = false;
        Inventory.AbilityIsActive = false;
    }





}
