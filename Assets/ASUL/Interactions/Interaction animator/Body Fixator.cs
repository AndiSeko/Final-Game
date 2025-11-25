using ASUL.FPC.Animations;
using UnityEngine;

namespace ASUL.Interactions.Animation
{
    public class BodyFixator : MonoBehaviour
    {
       
        [SerializeField] private Transform targetBodyPosition;
        [SerializeField] private Transform targetCameraPosition;
        [SerializeField] private float transitionTime = 1.0f;   

        [SerializeField] private BodyDatabase bodyDatabase;
       
        private Transform playerBody;
        private Transform playerCamera;
        
        private Animator playerAnimator;

        private Vector3 originalBodyPos;
        private Quaternion originalBodyRot;
        private Vector3 originalCameraPos;
        private Quaternion originalCameraRot;

        private bool isWorking = false;
        public bool IsWorking => isWorking;

        private void Awake()
        {
            playerBody = bodyDatabase.Body;
            playerCamera = bodyDatabase.Camera;
            playerAnimator = bodyDatabase.Animator;
        }

        
        public void Fixing()
        {
            if (playerBody == null || playerCamera == null) return;

            isWorking = true;

            CSetPreset.Cutscene();

            FirstPersonAnimator.IsIdleLocked = true;

            originalBodyPos = playerBody.position;
            originalBodyRot = playerBody.rotation;
            originalCameraPos = playerCamera.position;
            originalCameraRot = playerCamera.rotation;

            
            StartCoroutine(MoveToTarget(playerBody, targetBodyPosition));
            StartCoroutine(MoveToTarget(playerCamera, targetCameraPosition));

            StartCoroutine(WaitAndEnd());
        }

        
        public void Detaching()
        {
            if (playerBody == null || playerCamera == null) return;
            
            isWorking = true;


            StartCoroutine(MoveBack(playerBody, originalBodyPos, originalBodyRot));
            StartCoroutine(MoveBack(playerCamera, originalCameraPos, originalCameraRot));
            StartCoroutine(UnlockMovement());

            StartCoroutine(WaitAndEnd());
        }

        
        private System.Collections.IEnumerator MoveToTarget(Transform obj, Transform target)
        {
            Vector3 startPos = obj.position;
            Quaternion startRot = obj.rotation;
            float elapsed = 0f;

            while (elapsed < transitionTime)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / transitionTime;

                obj.position = Vector3.Lerp(startPos, target.position, t);
                obj.rotation = Quaternion.Slerp(startRot, target.rotation, t);

                yield return null;
            }

            obj.position = target.position;
            obj.rotation = target.rotation;
        }

        
        private System.Collections.IEnumerator MoveBack(Transform obj, Vector3 pos, Quaternion rot)
        {
            Vector3 startPos = obj.position;
            Quaternion startRot = obj.rotation;
            float elapsed = 0f;

            while (elapsed < transitionTime)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / transitionTime;

                obj.position = Vector3.Lerp(startPos, pos, t);
                obj.rotation = Quaternion.Slerp(startRot, rot, t);

                yield return null;
            }

            obj.position = pos;
            obj.rotation = rot;
        }

        private System.Collections.IEnumerator UnlockMovement()
        {
            yield return new WaitForSeconds(transitionTime);
            CSetPreset.Play();
            FirstPersonAnimator.IsIdleLocked = false;
        }

        private System.Collections.IEnumerator WaitAndEnd()
        {
            yield return new WaitForSeconds(transitionTime);
            isWorking = false;
        }
    }

}