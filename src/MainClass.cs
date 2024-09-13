using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;
using System.Reflection;
using UnityEngine;

using UnityEngine.Rendering;
using System.Collections;
using TMPro;
using static MelonLoader.MelonLogger;
using System.Runtime.InteropServices;

namespace SuperhotRandomizer
{
    public static class BuildInfo
    {
        public const string Name = "Superhot Randomizer";
        public const string Description = "Mod for randomizing weapons on player and enemies.";
        public const string Author = "Gasterbuzzer";
        public const string Company = null;
        public const string Version = "1.1.0";
        public const string DownloadLink = "https://github.com/Gasterbuzzer/SuperhotRandomizer/releases/";
    }

    /// <summary>
    /// Main Class / Currently unused
    /// </summary>
    public class MainClass : MelonMod
    {
    }

    [HarmonyPatch(typeof(PejAiController), "Start", new Type[] { })]
    public static class PatchEnemyStart
    {
        public static System.Random random = new System.Random();

        /// <summary>
        /// Patches Start Method of Enemies. Changes weapon, Speed and their Size.
        /// </summary>
        /// <param name="__originalMethod"> Method which was called (Used to get class type.) </param>
        /// <param name="__instance"> Caller of function. </param>
        private static void Postfix(MethodBase __originalMethod, object __instance)
        {
            Type classType = __originalMethod.DeclaringType;

            MethodInfo pickupWeapon = classType.GetMethod("PickupWeapon", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            int randomNumber = random.Next(0, 7);

            if (pickupWeapon != null)
            {
                switch (randomNumber)
                {
                    case 0:
                        FieldInfo civilianWeapon = classType.GetField("CivilianWeaponPrefab", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        var civilianWeaponPrefab = civilianWeapon.GetValue(__instance);
                        pickupWeapon.Invoke(__instance, new object[] { civilianWeaponPrefab, null, true });
                        break;

                    case 1:
                        FieldInfo machineGunWeapon = classType.GetField("MachineGunWeaponPrefab", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        var machineGunWeaponPrefab = machineGunWeapon.GetValue(__instance);
                        pickupWeapon.Invoke(__instance, new object[] { machineGunWeaponPrefab, null, true });
                        break;

                    case 2:
                        FieldInfo meleeWeapon = classType.GetField("MeleeWeaponPrefab", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        var meleeWeaponPrefab = meleeWeapon.GetValue(__instance);
                        pickupWeapon.Invoke(__instance, new object[] { meleeWeaponPrefab, null, true });
                        break;

                    case 3:
                        FieldInfo pistolWeapon = classType.GetField("PistolWeaponPrefab", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        var pistolWeaponPrefab = pistolWeapon.GetValue(__instance);
                        pickupWeapon.Invoke(__instance, new object[] { pistolWeaponPrefab, null, true });
                        break;

                    case 4:
                        FieldInfo shotgunWeapon = classType.GetField("ShotGunWeaponPrefab", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        var shotgunWeaponPrefab = shotgunWeapon.GetValue(__instance);
                        pickupWeapon.Invoke(__instance, new object[] { shotgunWeaponPrefab, null, true });
                        break;

                    case 5:
                        FieldInfo katanaWeapon = classType.GetField("KatanaWeaponPrefab", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        var katanaWeaponPrefab = katanaWeapon.GetValue(__instance);
                        pickupWeapon.Invoke(__instance, new object[] { katanaWeaponPrefab, null, true });
                        break;

                    // Extra Shotgun
                    case 6:
                        FieldInfo shotgunWeapon_2 = classType.GetField("ShotGunWeaponPrefab", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        var shotgunWeaponPrefab_2 = shotgunWeapon_2.GetValue(__instance);
                        pickupWeapon.Invoke(__instance, new object[] { shotgunWeaponPrefab_2, null, true });
                        break;

                    // Fallback, enemies follow you but cause a crash if you hit them. Unsure what it is used for.
                    default:
                        FieldInfo followWeapon = classType.GetField("MeleeWWeaponPrefab", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        var followWeaponPrefab = followWeapon.GetValue(__instance);
                        pickupWeapon.Invoke(__instance, new object[] { followWeaponPrefab, null, true });
                        break;
                }

            }
            else
            {
                MelonLoader.MelonLogger.Warning("PickUpWeapon method not found on the class.");
            }

            // Now we randomize some of their stats
            // Walking / Running / Look Speed
            float walkingSpeed = random.Next(0, 7);
            float runningSpeed = random.Next((int)walkingSpeed, 10);
            float lookSpeed = random.Next(180, 640);

            classType.GetField("WalkingSpeed", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(__instance, walkingSpeed);
            classType.GetField("RunningSpeed", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(__instance, runningSpeed);
            classType.GetField("LookSpeed", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(__instance, lookSpeed);

            // Weapon Speed
            float weaponSpeed = random.Next(0, 3);

            classType.GetField("weaponSpeedMultiplier", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(__instance, weaponSpeed);

            // Stand Still or Move
            int standStill = random.Next(0, 2);
            if (standStill == 0)
            {
                classType.GetField("standsStillWhileShooting", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(__instance, false);
            }
            else if (standStill == 1)
            {
                classType.GetField("standsStillWhileShooting", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(__instance, true);
            }

            // Randomize Size
            int resizeChance = random.Next(0, 4);

            if (resizeChance == 0)
            {
                // Yes, we resize

                MelonLoader.MelonLogger.Msg("Resizing Enemy");

                double randomSize = random.NextDouble();
                
                // Force minsize
                if (randomSize <= 0.1)
                {
                    randomSize += 0.1;
                }

                int factor = random.Next(1, 3);
                GameObject enemyGameObject = ((MonoBehaviour)__instance).gameObject;

                // If we are too small we factor it once and try again.
                if ((float)randomSize * factor <= 0.2f)
                {
                    randomSize *= factor;
                }

                enemyGameObject.transform.localScale = new Vector3((float)randomSize * factor, (float)randomSize * factor, (float)randomSize * factor);
            }

        }
    }

    [HarmonyPatch(typeof(HandManager), "Start", new Type[] { })]
    public static class PatchHandStart
    {
        public static System.Random random = new System.Random();

        /// <summary>
        /// Patches Start Method of Hand Manager of Player. Used to randomize starter weapon.
        /// </summary>
        /// <param name="__originalMethod"> Method which was called (Used to get class type.) </param>
        /// <param name="__instance"> Caller of function. </param>
        private static void Postfix(MethodBase __originalMethod, object __instance)
        {
            Type classType = __originalMethod.DeclaringType;
            FieldInfo currentWeapon = classType.GetField("weapon", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            MethodInfo spawnPlayerWeapon = classType.GetMethod("SpawnWeapon", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            int randomNumber = random.Next(0, 5);

            if (currentWeapon.GetValue(__instance) == null || currentWeapon.GetValue(__instance) is TakedownWeapon)
            {
                // No Weapon equipped on start
                MelonLoader.MelonLogger.Msg("No weapon equipped. 60% to gain one...");

                if (randomNumber < 4)
                {
                    MelonLoader.MelonLogger.Msg("Giving player a random weapon.");
                    randomNumber = random.Next(0, 4);
                    spawnPlayerWeapon.Invoke(__instance, new object[] { randomNumber });
                }

                // If we didn't have a weapon, we have a chance to be come ultra.
                randomNumber = random.Next(0, 25);

                if (randomNumber == 1)
                {
                    MethodInfo becomeUltra = classType.GetMethod("Ultra", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    becomeUltra.Invoke(__instance, new object[] { true });
                }

                return;
            }

            // We have a weapon, we first remove it
            // Remove the current weapon
            MelonLoader.MelonLogger.Msg("Already have weapon, replacing...");
            MethodInfo removeCurrentWeapon = classType.GetMethod("RemoveWeapon", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            removeCurrentWeapon.Invoke(__instance, new object[] { false, true });

            randomNumber = random.Next(0, 5);

            spawnPlayerWeapon.Invoke(__instance, new object[] { randomNumber });
        }
    }

    [HarmonyPatch(typeof(PejAiSpawner), "Spawn", new Type[] { })]
    public static class PatchAiSpawner
    {
        public static System.Random random = new System.Random();

        /// <summary>
        /// Patches Enemy Spawn Method. Used to skip spawn with chance.
        /// </summary>
        private static bool Prefix()
        {
            int randomNumber = random.Next(0, 4);

            // Skip Spawn
            if (randomNumber == 1)
            {
                MelonLoader.MelonLogger.Msg("Skipping one spawn.");
                return false;
            }

            // Do not skip spawn.
            return true;
        }

        /// <summary>
        /// Patches Enemy Spawn Method. Used to randomly add even more enemies.
        /// </summary>
        private static void Postfix(MethodBase __originalMethod, object __instance)
        {
            int randomNumber = random.Next(0, 5);

            // Dupliacte Spawn
            if (randomNumber == 1)
            {
                // One Extra Enemy
                MelonLoader.MelonLogger.Msg("Spawning extra enemy.");
                __originalMethod.Invoke(__instance, new object[] { });
            }
            else if (randomNumber == 2)
            {
                // Two Extra Enemies
                MelonLoader.MelonLogger.Msg("Spawning extra 2 enemies.");
                __originalMethod.Invoke(__instance, new object[] { });
                __originalMethod.Invoke(__instance, new object[] { });
            }
        }
    }

    [HarmonyPatch(typeof(PejAiBody), "Start", new Type[] { })]
    public static class EnemyHeadPatchRandom
    {
        public static System.Random random = new System.Random();

        /// <summary>
        /// Patches Start Method of Enemy Body. Used to randomly assign a new head prefab to enemy.
        /// </summary>
        /// <param name="__originalMethod"> Method which was called (Used to get class type.) </param>
        /// <param name="__instance"> Caller of function. </param>
        private static void Postfix(MethodBase __originalMethod, object __instance)
        {
            int randomNumber = random.Next(0, 5);

            Type classType = __originalMethod.DeclaringType;

            var pumpkinHeadPrefab = classType.GetField("PumpkinHeadPrefab", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(__instance);

            if (randomNumber == 0)
            {
                // Apply pumpkin head to enemy
                MethodInfo replaceHeadMethod = classType.GetMethod("ReplaceHead", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                replaceHeadMethod.Invoke(__instance, new object[] { pumpkinHeadPrefab });
            }
        }
    }
}