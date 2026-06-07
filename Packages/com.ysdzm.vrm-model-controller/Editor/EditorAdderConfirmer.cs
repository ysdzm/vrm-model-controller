using UnityEngine;

namespace Ysdzm.VrmModelController
{
    public static class EditorAdderConfirmer
    {
        [RuntimeInitializeOnLoadMethod]
        public static void ConfirmFeature()
        {
            Debug.Log("あなたは vrm-model-controller をインストールしています");

            var adder = new Adder();
            const int x = 10;
            const int y = 20;
            
            Debug.Log($"x = {x} + y = {y} の結果は {adder.Add(x, y)}です。 これは vrm-model-controller の Adder クラスで計算できます。");
        }
    }
}
