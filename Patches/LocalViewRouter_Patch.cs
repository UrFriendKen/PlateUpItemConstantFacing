using HarmonyLib;
using Kitchen;
using UnityEngine;

namespace KitchenItemConstantFacing.Patches
{
    [HarmonyPatch]
    static class LocalViewRouter_Patch
    {
        [HarmonyPatch(typeof(LocalViewRouter), "GetPrefab")]
        [HarmonyPostfix]
        static void GetPrefab_Postfix(ViewType view_type, GameObject __result)
        {
            if (view_type == ViewType.Appliance && !__result.GetComponent<ConstantFacing>())
            {
                if (__result.TryGetComponent<ApplianceView>(out var applianceView))
                {
                    ConstantFacing constantFacing = __result.AddComponent<ConstantFacing>();
                    constantFacing.View = applianceView;
                }
            }
        }
    }

    public class ConstantFacing : MonoBehaviour
    {
        public ApplianceView View;
        void Update()
        {
            if (View?.HeldItemPosition != null)
            {
                Vector3 rotation = View.HeldItemPosition.rotation.eulerAngles;
                if (Mathf.Abs(rotation.x) > 0.001f || Mathf.Abs(rotation.z) > 0.001f)
                    return;
                View.HeldItemPosition.rotation = Quaternion.Euler(rotation.x, 0f, rotation.z);
            }
        }
    }
}
