using System;
using UnityEngine;
using UnityStandardAssets.Utility;

public class HeadBob : MonoBehaviour
{
   public Camera Camera;
   public CurveControlledBob motionBob = new CurveControlledBob ();
   public LerpControlledBob jumpAndLandingBob = new LerpControlledBob ();
   public float StrideInterval;
   [Range (0f, 1f)] public float RunningStrideLengthen;

   private bool previouslyGrounded;
   private Vector3 originalCameraPosition;
   private PlayerController playerController;
   private void Start ()
   {
      motionBob.Setup (Camera, StrideInterval);
      originalCameraPosition = Camera.transform.localPosition;
      playerController = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
   }


   private void Update ()
   {
      Vector3 newCameraPosition;
      if (playerController.Velocity.magnitude > 0 && playerController.Grounded)
      {
         Camera.transform.localPosition = motionBob.DoHeadBob (playerController.Velocity.magnitude * (playerController.Running ? RunningStrideLengthen : 1f));
         newCameraPosition = Camera.transform.localPosition;
         newCameraPosition.y = Camera.transform.localPosition.y - jumpAndLandingBob.Offset ();
      } else
      {
         newCameraPosition = Camera.transform.localPosition;
         newCameraPosition.y = originalCameraPosition.y - jumpAndLandingBob.Offset ();
      }
      Camera.transform.localPosition = newCameraPosition;

      if (!previouslyGrounded && playerController.Grounded)
      {
         StartCoroutine (jumpAndLandingBob.DoBobCycle ());
      }

      previouslyGrounded = playerController.Grounded;
   }
}
