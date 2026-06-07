using UniVRM10;
using UnityEngine;

namespace Ysdzm.VrmModelController
{
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

        [SerializeField] private Transform targetRoot;
        [SerializeField] private Vrm10Instance vrmInstance;

        public Transform TargetRoot => targetRoot != null ? targetRoot : transform;
        public Vrm10Instance VrmInstance => ResolveVrmInstance();
        public bool HasVrmInstance => VrmInstance != null;
        public string VrmInstanceName => VrmInstance != null ? VrmInstance.name : string.Empty;

        public Vector3 GetLocalPosition()
        {
            return TargetRoot.localPosition;
        }

        public Vector3 GetLocalRotation()
        {
            return TargetRoot.localEulerAngles;
        }

        public float GetUniformScale()
        {
            return TargetRoot.localScale.x;
        }

        public void SetLocalPosition(Vector3 position)
        {
            TargetRoot.localPosition = position;
        }

        public void SetLocalRotation(Vector3 eulerAngles)
        {
            TargetRoot.localEulerAngles = eulerAngles;
        }

        public void SetUniformScale(float scale)
        {
            TargetRoot.localScale = Vector3.one * scale;
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
            var instance = VrmInstance;
            if (instance == null || instance.Runtime == null)
            {
                return;
            }

            instance.Runtime.Process();
        }

        private void SetExpressionWeight(ExpressionKey key, float weight)
        {
            var instance = VrmInstance;
            if (instance == null || instance.Runtime == null)
            {
                return;
            }

            instance.Runtime.Expression.SetWeight(key, Mathf.Clamp01(weight));
        }

        private Vrm10Instance ResolveVrmInstance()
        {
            if (vrmInstance != null)
            {
                return vrmInstance;
            }

            return vrmInstance =
                TargetRoot.GetComponentInChildren<Vrm10Instance>()
                ?? TargetRoot.GetComponentInParent<Vrm10Instance>()
                ?? FindFirstObjectByType<Vrm10Instance>();
        }
    }
}
