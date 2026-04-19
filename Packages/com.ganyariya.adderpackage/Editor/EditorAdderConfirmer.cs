using UnityEngine;

namespace Ysdzm.SampleAdder
{
    public static class EditorAdderConfirmer
    {
        [RuntimeInitializeOnLoadMethod]
        public static void ConfirmFeature()
        {
            Debug.Log("あなたは ysdzm.adderpackage をインストールしています");

            var adder = new Adder();
            const int x = 10;
            const int y = 20;
            
            Debug.Log($"x = {x} + y = {y} の結果は {adder.Add(x, y)}です。 これは adderpackage の Adder クラスで計算できます。");
        }
    }
}