using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class AbilitySizeShift : MonoBehaviour
{
    public float abilityDuration = 10f; //how long till player reverts to normal

    const float scaleFactor = 3f;
    
    //references
    public CinemachineVirtualCamera virtualCamera;
    StarterAssets.ThirdPersonController thirdPersonController;
    CharacterController characterController;
    GameObject skeleton;
    GameObject playerCameraRoot;  

    //initial settings variables 
    float moveSpeed;
    float jumpHeight;
    float gravity;
    float sprintSpeed;

    Vector3 vCamShoulderOffset;

    //bool to toggle size shifting functionality
    bool sizeShifting = false;

    //more settings
    Vector3 startScale = new Vector3(1, 1, 1);
    Vector3 endScale = new Vector3(3, 3, 3);
    public float duration = 3f;
    float elapsedTime = 0f;

    //for Lerp
    public AnimationCurve curve;
    float percentageComplete;

    private void Start()
    {
        //set references
        thirdPersonController = GetComponent<StarterAssets.ThirdPersonController>();
        skeleton = transform.GetChild(2).gameObject;
        playerCameraRoot = transform.GetChild(0).gameObject;
        characterController = GetComponent<CharacterController>();
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        //set initial default normal values
        moveSpeed = thirdPersonController.MoveSpeed;
        jumpHeight = thirdPersonController.JumpHeight;
        gravity = thirdPersonController.Gravity;
        sprintSpeed = thirdPersonController.SprintSpeed;

        vCamShoulderOffset = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset;
    }

    public void ActivateSizeShift()
    {
        if (!sizeShifting)
        {
            sizeShifting = true;
            StartCoroutine(SizeGrow());
            StartCoroutine(SizeShrink());
        }

    }

    //coroutine for size shift
    //keep changing until full size reached
    IEnumerator SizeGrow()
    {
        elapsedTime = 0;
        percentageComplete = 0;

        while (percentageComplete < 1.0f)
        {
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / duration;

            //scale skeleton
            skeleton.transform.localScale = Vector3.Lerp(startScale, endScale, curve.Evaluate(percentageComplete));

            //scale characterController
            characterController.height = Mathf.Lerp(1.8f, 1.8f * scaleFactor, curve.Evaluate(percentageComplete));
            characterController.radius = Mathf.Lerp(.28f, .28f * scaleFactor, curve.Evaluate(percentageComplete));
            characterController.center = Vector3.Lerp(new Vector3(0f, .93f, 0f), new Vector3(0f, .93f * scaleFactor, 0f), curve.Evaluate(percentageComplete));

            //scale thirdPersonController
            thirdPersonController.GroundedRadius = Mathf.Lerp(.28f, .28f * scaleFactor, curve.Evaluate(percentageComplete));
            thirdPersonController.MoveSpeed = Mathf.Lerp(moveSpeed, moveSpeed * scaleFactor, curve.Evaluate(percentageComplete));
            thirdPersonController.JumpHeight = Mathf.Lerp(jumpHeight, jumpHeight * scaleFactor, curve.Evaluate(percentageComplete));
            thirdPersonController.Gravity = Mathf.Lerp(gravity, gravity * scaleFactor, curve.Evaluate(percentageComplete));
            thirdPersonController.SprintSpeed = Mathf.Lerp(sprintSpeed, sprintSpeed * scaleFactor, curve.Evaluate(percentageComplete));

            //scale playerCameraRoot
            playerCameraRoot.transform.localPosition = Vector3.Lerp(new Vector3(0, 1.375f, 0), new Vector3(0, 1.375f * scaleFactor, 0), curve.Evaluate(percentageComplete));

            //scale vcam
            virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset = Vector3.Lerp(new Vector3(1, 0, 0), new Vector3(1, 0, -3f), curve.Evaluate(percentageComplete));

            yield return null;
        }    
    }

    IEnumerator SizeShrink()
    {
        //Debug.Log("size shrink");
        yield return new WaitForSeconds(abilityDuration);
        elapsedTime = 0;
        percentageComplete = 0;

        while (percentageComplete < 1.0f)
        {
            //Debug.Log(percentageComplete);
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / duration;

            //scale skeleton
            skeleton.transform.localScale = Vector3.Lerp(endScale,startScale, curve.Evaluate(percentageComplete));

            //scale characterController
            characterController.height = Mathf.Lerp(1.8f *scaleFactor, 1.8f, curve.Evaluate(percentageComplete));
            characterController.radius = Mathf.Lerp(.28f * scaleFactor, .28f, curve.Evaluate(percentageComplete));
            characterController.center = Vector3.Lerp(new Vector3(0f, .93f * scaleFactor, 0f), new Vector3(0f, .93f, 0f), curve.Evaluate(percentageComplete));

            //scale thirdPersonController
            thirdPersonController.GroundedRadius = Mathf.Lerp( .28f * scaleFactor, .28f, curve.Evaluate(percentageComplete));
            thirdPersonController.MoveSpeed = Mathf.Lerp(moveSpeed * scaleFactor, moveSpeed, curve.Evaluate(percentageComplete));
            thirdPersonController.JumpHeight = Mathf.Lerp(jumpHeight * scaleFactor, jumpHeight, curve.Evaluate(percentageComplete));
            thirdPersonController.Gravity = Mathf.Lerp(gravity * scaleFactor, gravity, curve.Evaluate(percentageComplete));
            thirdPersonController.SprintSpeed = Mathf.Lerp(sprintSpeed * scaleFactor, sprintSpeed, curve.Evaluate(percentageComplete));

            //scale playerCameraRoot
            playerCameraRoot.transform.localPosition = Vector3.Lerp(new Vector3(0, 1.375f * scaleFactor, 0), new Vector3(0, 1.375f, 0), curve.Evaluate(percentageComplete));

            //scale vcam
            virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset = Vector3.Lerp(new Vector3(1, 0, -3f), new Vector3(1, 0, 0), curve.Evaluate(percentageComplete));

            yield return null;
        }
        
        //   Debug.Log("size shifting is done.");
        sizeShifting = false;
        // can size shift again
        Inventory.AbilityIsActive = false;
    }
}
