using MelonLoader;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace SuperhotRandomizer
{
    /// <summary>
    /// Main Class / Currently unused
    /// </summary>
    public class MainClass : MelonMod
    {
    }

    [HarmonyPatch(typeof(PejAiController), "Start")]
    public static class PatchEnemyStart
    {
        private static readonly System.Random RandomSystem = new System.Random();

        private static readonly MethodInfo PickupWeapon = typeof(PejAiController).GetMethod("PickupWeapon",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        private static readonly FieldInfo CivilianWeapon = typeof(PejAiController).GetField("CivilianWeaponPrefab",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        private static readonly FieldInfo MachineGunWeapon = typeof(PejAiController).GetField("MachineGunWeaponPrefab",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        private static readonly FieldInfo MeleeWeapon = typeof(PejAiController).GetField("MeleeWeaponPrefab",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        private static readonly FieldInfo PistolWeapon = typeof(PejAiController).GetField("PistolWeaponPrefab",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        private static readonly FieldInfo ShotgunWeapon = typeof(PejAiController).GetField("ShotGunWeaponPrefab",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        private static readonly FieldInfo KatanaWeapon = typeof(PejAiController).GetField("KatanaWeaponPrefab",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        private static readonly FieldInfo FollowWeapon = typeof(PejAiController).GetField("MeleeWWeaponPrefab",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        private static readonly FieldInfo WalkingSpeed = typeof(PejAiController).GetField("WalkingSpeed",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        private static readonly FieldInfo RunningSpeed = typeof(PejAiController).GetField("RunningSpeed",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        private static readonly FieldInfo LookSpeed = typeof(PejAiController).GetField("LookSpeed",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        private static readonly FieldInfo WeaponSpeedMultiplier = typeof(PejAiController).GetField(
            "weaponSpeedMultiplier",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        private static readonly FieldInfo StandsStillWhileShooting = typeof(PejAiController).GetField(
            "standsStillWhileShooting",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        /// <summary>
        /// Patches Start Method of Enemies. Changes weapon, Speed and their Size.
        /// </summary>
        /// <param name="__instance"> Caller of function. </param>
        // ReSharper disable once UnusedMember.Local
        private static void Postfix(PejAiController __instance)
        {
            int randomNumber = RandomSystem.Next(0, 7);

            if (PickupWeapon != null)
            {
                switch (randomNumber)
                {
                    case 0:
                        GameObject civilianWeaponPrefab = (GameObject)CivilianWeapon.GetValue(__instance);
                        PickupWeapon.Invoke(__instance, new object[] { civilianWeaponPrefab, null, true });
                        break;

                    case 1:
                        GameObject machineGunWeaponPrefab = (GameObject)MachineGunWeapon.GetValue(__instance);
                        PickupWeapon.Invoke(__instance, new object[] { machineGunWeaponPrefab, null, true });
                        break;

                    case 2:
                        GameObject meleeWeaponPrefab = (GameObject)MeleeWeapon.GetValue(__instance);
                        PickupWeapon.Invoke(__instance, new object[] { meleeWeaponPrefab, null, true });
                        break;

                    case 3:
                        GameObject pistolWeaponPrefab = (GameObject)PistolWeapon.GetValue(__instance);
                        PickupWeapon.Invoke(__instance, new object[] { pistolWeaponPrefab, null, true });
                        break;

                    case 4:
                        GameObject shotgunWeaponPrefab = (GameObject)ShotgunWeapon.GetValue(__instance);
                        PickupWeapon.Invoke(__instance, new object[] { shotgunWeaponPrefab, null, true });
                        break;

                    case 5:
                        GameObject katanaWeaponPrefab = (GameObject)KatanaWeapon.GetValue(__instance);
                        PickupWeapon.Invoke(__instance, new object[] { katanaWeaponPrefab, null, true });
                        break;

                    // Extra Shotgun
                    case 6:
                        GameObject shotgunWeaponPrefab_2 = (GameObject)ShotgunWeapon.GetValue(__instance);
                        PickupWeapon.Invoke(__instance, new object[] { shotgunWeaponPrefab_2, null, true });
                        break;

                    // Fallback, enemies follow you but cause a crash if you hit them. Unsure what it is used for.
                    default:
                        GameObject followWeaponPrefab = (GameObject)FollowWeapon.GetValue(__instance);
                        PickupWeapon.Invoke(__instance, new object[] { followWeaponPrefab, null, true });
                        break;
                }
            }
            else
            {
                MelonLogger.Warning("PickUpWeapon method not found on the class.");
            }

            // Now we randomize some of their stats
            // Walking / Running / Look Speed
            float walkingSpeed = RandomSystem.Next(0, 7);
            float runningSpeed = RandomSystem.Next((int)walkingSpeed, 10);
            float lookSpeed = RandomSystem.Next(180, 640);

            WalkingSpeed.SetValue(__instance, walkingSpeed);
            RunningSpeed.SetValue(__instance, runningSpeed);
            LookSpeed.SetValue(__instance, lookSpeed);

            // Weapon Speed
            float weaponSpeed = RandomSystem.Next(0, 3);

            WeaponSpeedMultiplier.SetValue(__instance, weaponSpeed);

            // Stand Still or Move
            int standStill = RandomSystem.Next(0, 2);
            if (standStill == 0)
            {
                StandsStillWhileShooting.SetValue(__instance, false);
            }
            else if (standStill == 1)
            {
                StandsStillWhileShooting.SetValue(__instance, true);
            }

            // Randomize Size
            int resizeChance = RandomSystem.Next(0, 4);

            // Resize
            if (resizeChance == 0)
            {
                double randomSize = RandomSystem.NextDouble();

                // Force minimum size.
                if (randomSize <= 0.1)
                {
                    randomSize += 0.1;
                }

                int factor = RandomSystem.Next(1, 3);
                GameObject enemyGameObject = __instance.gameObject;

                // If we are too small we factor it once and try again.
                if ((float)randomSize * factor <= 0.2f)
                {
                    randomSize *= factor;
                }

                enemyGameObject.transform.localScale = new Vector3((float)randomSize * factor,
                    (float)randomSize * factor, (float)randomSize * factor);
            }
        }
    }

    [HarmonyPatch(typeof(HandManager), "Start")]
    public static class PatchHandStart
    {
        private static readonly System.Random RandomSystem = new System.Random();

        private static readonly FieldInfo CurrentWeapon = typeof(HandManager).GetField("weapon",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        private static readonly MethodInfo SpawnPlayerWeapon = typeof(HandManager).GetMethod("SpawnWeapon",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        private static readonly MethodInfo BecomeUltra = typeof(HandManager).GetMethod("Ultra",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        private static readonly MethodInfo RemoveCurrentWeapon = typeof(HandManager).GetMethod("RemoveWeapon",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        /// <summary>
        /// Patches Start Method of Hand Manager of Player. Used to randomize starter weapon.
        /// </summary>
        /// <param name="__instance"> Caller of function. </param>
        // ReSharper disable once UnusedMember.Local
        private static void Postfix(HandManager __instance)
        {
            int randomNumber = RandomSystem.Next(0, 5);

            if (CurrentWeapon.GetValue(__instance) == null
                || CurrentWeapon.GetValue(__instance) is TakedownWeapon)
            {
                // No Weapon equipped on start
                MelonLogger.Msg("No weapon equipped. 60% to gain one...");

                if (randomNumber < 4)
                {
                    MelonLogger.Msg("Giving player a random weapon.");
                    randomNumber = RandomSystem.Next(0, 4);
                    SpawnPlayerWeapon.Invoke(__instance, new object[] { randomNumber });
                }

                // If we didn't have a weapon, we have a chance to become ultra.
                randomNumber = RandomSystem.Next(0, 25);

                if (randomNumber == 1)
                {
                    BecomeUltra.Invoke(__instance, new object[] { true });
                }

                return;
            }

            // We have a weapon, we first remove it
            // Remove the current weapon
            MelonLogger.Msg("Already have weapon, replacing...");

            RemoveCurrentWeapon.Invoke(__instance, new object[] { false, true });

            randomNumber = RandomSystem.Next(0, 5);

            RemoveCurrentWeapon.Invoke(__instance, new object[] { randomNumber });
        }
    }

    [HarmonyPatch(typeof(PejAiSpawner), "Spawn")]
    public static class PatchAiSpawner
    {
        private static readonly System.Random RandomSystem = new System.Random();

        /// <summary>
        /// Patches Enemy Spawn Method. Used to skip spawn with chance.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private static bool Prefix()
        {
            int randomNumber = RandomSystem.Next(0, 4);

            // Skip Spawn
            if (randomNumber == 1)
            {
                MelonLogger.Msg("Skipping one spawn.");
                return false;
            }

            // Do not skip spawn.
            return true;
        }

        /// <summary>
        /// Patches Enemy Spawn Method. Used to randomly add even more enemies.
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private static void Postfix(MethodBase __originalMethod, PejAiSpawner __instance)
        {
            int randomNumber = RandomSystem.Next(0, 5);

            // Duplicate Spawn
            if (randomNumber == 1)
            {
                // One Extra Enemy
                MelonLogger.Msg("Spawning extra enemy.");
                __originalMethod.Invoke(__instance, null);
            }
            else if (randomNumber == 2)
            {
                // Two Extra Enemies
                MelonLogger.Msg("Spawning extra 2 enemies.");
                __originalMethod.Invoke(__instance, null);
                __originalMethod.Invoke(__instance, null);
            }
        }
    }

    [HarmonyPatch(typeof(PejAiBody), "Start")]
    public static class EnemyHeadPatchRandom
    {
        private static readonly System.Random RandomSystem = new System.Random();

        private static readonly FieldInfo PumpkinHeadPrefab = typeof(PejAiBody).GetField("PumpkinHeadPrefab",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        private static readonly MethodInfo ReplaceHeadMethod = typeof(PejAiBody).GetMethod("ReplaceHead",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        /// <summary>
        /// Patches Start Method of Enemy Body. Used to randomly assign a new head prefab to enemy.
        /// </summary>
        /// <param name="__instance"> Caller of function. </param>
        // ReSharper disable once UnusedMember.Local
        private static void Postfix(PejAiBody __instance)
        {
            int randomNumber = RandomSystem.Next(0, 5);

            GameObject pumpkinHeadPrefab = (GameObject)PumpkinHeadPrefab.GetValue(__instance);

            if (randomNumber == 0)
            {
                // Apply pumpkin head to enemy
                ReplaceHeadMethod.Invoke(__instance, new object[] { pumpkinHeadPrefab });
            }
        }
    }
}