using System;
using Harmony;
using UnityEngine;

namespace VacuumOfSpace
{
	// Token: 0x02000003 RID: 3
	public class HeadquartersConfigPatches
	{
		// Token: 0x02000004 RID: 4
		public static class Mod_OnLoad
		{
			// Token: 0x06000006 RID: 6 RVA: 0x00002340 File Offset: 0x00000540
			public static void OnLoad()
			{
			}
		}

		// Token: 0x02000005 RID: 5
		[HarmonyPatch(typeof(HeadquartersConfig))]
		[HarmonyPatch("DoPostConfigureComplete")]
		public static class HeadquartersConfig_DoPostConfigureComplete_Patch
		{
			// Token: 0x06000007 RID: 7 RVA: 0x00002342 File Offset: 0x00000542
			public static void Postfix(GameObject go)
			{
				EntityTemplateExtensions.AddOrGet<VacuumCleanerOfSpace>(go);
			}
		}
	}
}
