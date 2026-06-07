using UniVRM10;
using UnityEngine;

namespace Ysdzm.VrmModelController
{
    public enum LookAtMode
    {
        None,
        EyesOnly,
        Head
    }

    [DefaultExecutionOrder(11020)]
    public class VrmModelController : MonoBehaviour
    {
        private static readonly ExpressionPreset[] ResetExpressionPresets =
        {
            ExpressionPreset.happy,
            ExpressionPreset.angry,
            ExpressionPreset.sad,
            ExpressionPreset.relaxed,
            ExpressionPreset.surprised,
            ExpressionPreset.aa,
            ExpressionPreset.ih,
            ExpressionPreset.ou,
            ExpressionPreset.ee,
            ExpressionPreset.oh,
            ExpressionPreset.blink,
            ExpressionPreset.blinkLeft,
            ExpressionPreset.blinkRight,
            ExpressionPreset.lookUp,
            ExpressionPreset.lookDown,
            ExpressionPreset.lookLeft,
            ExpressionPreset.lookRight
        };

        [SerializeField] private Vrm10Instance vrmInstance;
        [SerializeField] private Transform lookAtTarget;
        [SerializeField] private LookAtMode lookAtMode = LookAtMode.None;
        [SerializeField] private float headLookAtSpeed = 12f;

        private Transform head;
        private Quaternion initialHeadLocalRotation;
        private bool hasInitialHeadLocalRotation;

        public Vrm10Instance VrmInstance => ResolveVrmInstance();
        public bool HasVrmInstance => VrmInstance != null;
        public string VrmInstanceName => VrmInstance != null ? VrmInstance.name : string.Empty;
        public LookAtMode CurrentLookAtMode => lookAtMode;
        public Transform LookAtTarget => lookAtTarget;

        private void LateUpdate()
        {
            UpdateLookAt();
        }

        public Vector3 GetLocalPosition()
        {
            return transform.localPosition;
        }

        public Vector3 GetLocalRotation()
        {
            return transform.localEulerAngles;
        }

        public float GetUniformScale()
        {
            return transform.localScale.x;
        }

        public void SetLocalPosition(Vector3 position)
        {
            transform.localPosition = position;
        }

        public void SetLocalRotation(Vector3 eulerAngles)
        {
            transform.localEulerAngles = eulerAngles;
        }

        public void SetUniformScale(float scale)
        {
            transform.localScale = Vector3.one * scale;
        }

        public void ResetTransform()
        {
            SetLocalPosition(Vector3.zero);
            SetLocalRotation(Vector3.zero);
            SetUniformScale(1f);
        }

        public void SetExpressionWeight(ExpressionPreset preset, float weight)
        {
            SetExpressionWeight(ExpressionKey.CreateFromPreset(preset), weight);
        }

        public void SetCustomExpressionWeight(string expressionName, float weight)
        {
            SetExpressionWeight(ExpressionKey.CreateCustom(expressionName), weight);
        }

        public void ResetPresetExpressions()
        {
            foreach (var preset in ResetExpressionPresets)
            {
                SetExpressionWeight(preset, 0f);
            }
        }

        public void ProcessVrmRuntime()
        {
            if (VrmInstance == null)
            {
                return;
            }

            VrmInstance.Runtime.Process();
        }

        public void SetLookAtTarget(Transform target)
        {
            lookAtTarget = target;
            ApplyEyeLookAtSettings();
        }

        public void SetLookAtMode(LookAtMode mode)
        {
            lookAtMode = mode;
            ApplyEyeLookAtSettings();
        }

        public void ClearLookAtTarget()
        {
            SetLookAtTarget(null);
        }

        private void SetExpressionWeight(ExpressionKey key, float weight)
        {
            if (VrmInstance == null)
            {
                return;
            }

            VrmInstance.Runtime.Expression.SetWeight(key, Mathf.Clamp01(weight));
        }

        private void UpdateLookAt()
        {
            ApplyEyeLookAtSettings();

            if (lookAtMode == LookAtMode.EyesOnly)
            {
                ResetHeadRotation();
                return;
            }

            if (lookAtMode == LookAtMode.Head)
            {
                ApplyHeadLookAt();
            }
        }

        private void ApplyEyeLookAtSettings()
        {
            if (VrmInstance == null)
            {
                return;
            }

            if (lookAtMode == LookAtMode.EyesOnly && lookAtTarget != null)
            {
                VrmInstance.LookAtTarget = lookAtTarget;
                VrmInstance.LookAtTargetType = VRM10ObjectLookAt.LookAtTargetTypes.SpecifiedTransform;
                return;
            }

            VrmInstance.LookAtTarget = null;
            VrmInstance.LookAtTargetType = VRM10ObjectLookAt.LookAtTargetTypes.YawPitchValue;
        }

        private void ApplyHeadLookAt()
        {
            if (lookAtTarget == null || !TryGetHead(out var headBone))
            {
                return;
            }

            var direction = lookAtTarget.position - headBone.position;
            if (direction.sqrMagnitude <= Mathf.Epsilon)
            {
                return;
            }

            var targetRotation = Quaternion.LookRotation(direction.normalized, transform.up);
            headBone.rotation = Quaternion.Slerp(headBone.rotation, targetRotation, Time.deltaTime * headLookAtSpeed);
        }

        private void ResetHeadRotation()
        {
            if (!TryGetHead(out var headBone))
            {
                return;
            }

            headBone.localRotation = initialHeadLocalRotation;
        }

        private bool TryGetHead(out Transform headBone)
        {
            if (head != null)
            {
                headBone = head;
                return true;
            }

            var instance = VrmInstance;
            if (instance == null || !instance.TryGetBoneTransform(HumanBodyBones.Head, out head))
            {
                headBone = null;
                return false;
            }

            if (!hasInitialHeadLocalRotation)
            {
                initialHeadLocalRotation = head.localRotation;
                hasInitialHeadLocalRotation = true;
            }

            headBone = head;
            return true;
        }

        private Vrm10Instance ResolveVrmInstance()
        {
            if (vrmInstance != null)
            {
                return vrmInstance;
            }

            return vrmInstance =
                GetComponentInChildren<Vrm10Instance>()
                ?? GetComponentInParent<Vrm10Instance>()
                ?? FindFirstObjectByType<Vrm10Instance>();
        }
    }
}
