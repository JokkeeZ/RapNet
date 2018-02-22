using System.Collections.Generic;

namespace RapNet.Preprocessors
{
    /// <summary>
    /// Represents valid raP EntryTypes.RapValue names used for #define preprocessors.
    /// </summary>
    public class DefinePreprocessors
    {
        private static readonly List<RapDefinePreprocessor> _booleans =
            new List<RapDefinePreprocessor>
        {
            new RapDefinePreprocessor("false", 0),
            new RapDefinePreprocessor("true", 1)
        };

        private static readonly List<RapDefinePreprocessor> _accessors =
            new List<RapDefinePreprocessor>
        {
            new RapDefinePreprocessor("private", 0),
            new RapDefinePreprocessor("protected", 1),
            new RapDefinePreprocessor("public", 2)
        };

        private static readonly List<RapDefinePreprocessor> _weaponMagazineTypes =
            new List<RapDefinePreprocessor>
        {
            new RapDefinePreprocessor("WeaponNoSlot", 0),
            new RapDefinePreprocessor("WeaponSlotPrimary", 1),
            new RapDefinePreprocessor("WeaponSlotHandGun", 2),
            new RapDefinePreprocessor("WeaponSlotSecondary", 16),
            new RapDefinePreprocessor("WeaponSlotHandGunItem", 32),
            new RapDefinePreprocessor("WeaponSlotItem", 256),
            new RapDefinePreprocessor("WeaponSlotBinocular", 4096),
            new RapDefinePreprocessor("WeaponHardMounted", 65536),
            new RapDefinePreprocessor("WeaponSmallItems", 131072)
        };

        private static readonly List<RapDefinePreprocessor> _canSee =
            new List<RapDefinePreprocessor>
        {
            new RapDefinePreprocessor("CanSeeRadar", 1),
            new RapDefinePreprocessor("CanSeeEye", 2),
            new RapDefinePreprocessor("CanSeeOptics", 4),
            new RapDefinePreprocessor("CanSeeEar", 8),
            new RapDefinePreprocessor("CanSeeCompass", 16),
            new RapDefinePreprocessor("CanSeeAll", 31),
            new RapDefinePreprocessor("CanSeePeripheral", 32),
            new RapDefinePreprocessor("CanSeeRadarC", 17)
        };

        private static readonly Dictionary<string, List<RapDefinePreprocessor>> _defines =
            new Dictionary<string, List<RapDefinePreprocessor>>
        {
            { "scope", _accessors },
            { "scopeWeapon", _accessors },
            { "scopeMagazine", _accessors },

            { "weaponType", _weaponMagazineTypes },
            { "magazineType", _weaponMagazineTypes },

            {
                "canLock", new List<RapDefinePreprocessor> {
                    new RapDefinePreprocessor("LockDisabled", 0),
                    new RapDefinePreprocessor("LockCadet", 1),
                    new RapDefinePreprocessor("LockAlways", 2),
                }
            },

            {
                "type", new List<RapDefinePreprocessor> {
                    new RapDefinePreprocessor("VSoft", 0),
                    new RapDefinePreprocessor("VArmor", 1),
                    new RapDefinePreprocessor("VAir", 2)
                }
            },

            {
                "side", new List<RapDefinePreprocessor> {
                    new RapDefinePreprocessor("TEast", 0),
                    new RapDefinePreprocessor("TWest", 1),
                    new RapDefinePreprocessor("TGuerrila", 2),
                    new RapDefinePreprocessor("TCivilian", 3),
                    new RapDefinePreprocessor("TSideUnknown", 4),
                    new RapDefinePreprocessor("TEnemy", 5),
                    new RapDefinePreprocessor("TFriendly", 6),
                    new RapDefinePreprocessor("TLogic", 7)
                }
            },

            {
                "access", new List<RapDefinePreprocessor> {
                    new RapDefinePreprocessor("ReadAndWrite", 0),
                    new RapDefinePreprocessor("ReadAndCreate", 1),
                    new RapDefinePreprocessor("ReadOnly", 2),
                    new RapDefinePreprocessor("ReadOnlyVerified", 3)
                }
            },

            {
                "aiAmmoUsageFlags", new List<RapDefinePreprocessor> {
                    new RapDefinePreprocessor("None", 0),
                    new RapDefinePreprocessor("Light", 1),
                    new RapDefinePreprocessor("Marking", 2),
                    new RapDefinePreprocessor("Concealment", 4),
                    new RapDefinePreprocessor("CounterMeasures", 8),
                    new RapDefinePreprocessor("Mine", 16),
                    new RapDefinePreprocessor("Underwater", 32),
                    new RapDefinePreprocessor("OffensiveInf", 64),
                    new RapDefinePreprocessor("OffensiveVeh", 128),
                    new RapDefinePreprocessor("OffensiveAir", 256),
                    new RapDefinePreprocessor("OffensiveArmour", 512)
                }
            },

            {
                "weaponLockSystem", new List<RapDefinePreprocessor> {
                    new RapDefinePreprocessor("Undetectable", 0),
                    new RapDefinePreprocessor("VisualContrast", 1),
                    new RapDefinePreprocessor("InfraRed", 2),
                    new RapDefinePreprocessor("LaserGuided", 4),
                    new RapDefinePreprocessor("RadarGuided", 8),
                    new RapDefinePreprocessor("Missile", 16)
                }
            },

            {
                "brakeDistance", new List<RapDefinePreprocessor> {
                    new RapDefinePreprocessor("BrakeDistanceMan", 1),
                    new RapDefinePreprocessor("BrakeDistanceTank", 14),
                    new RapDefinePreprocessor("BrakeDistanceBoat", 50),
                    new RapDefinePreprocessor("BrakeDistancePlane", 500)
                }
            },

            { "disableWeapons", _booleans },
            { "rightHandIKEnd", _booleans },
            { "gunnerUsesPilotView", _booleans },
            { "explosive", _booleans },
            { "hasCollShapeSafe", _booleans },
            { "enableAttack", _booleans },
            { "showWeaponAim", _booleans },
            { "laserScanner", _booleans },
            { "onLadder", _booleans },
            { "shadow", _booleans },
            { "animated", _booleans },
            { "forceHideDriver", _booleans },
            { "showItemInRightHand", _booleans },
            { "unloadInCombat", _booleans },
            { "hasDriver", _booleans },
            { "ejectDeadCargo", _booleans },
            { "crewVulnerable", _booleans },
            { "canDeactivateMines", _booleans },
            { "attendant", _booleans },
            { "enableBinocular", _booleans },
            { "viewGunnerInExternal", _booleans },
            { "isBicycle", _booleans },
            { "boundingSphere", _booleans },
            { "preferRoads", _booleans },
            { "autoAimEnabled", _booleans },
            { "castDriverShadow", _booleans },
            { "viewDriverShadow", _booleans },
            { "canPullTrigger", _booleans },
            { "driverIsCommander", _booleans },
            { "hideWeaponsDriver", _booleans },
            { "ejectDeadDriver", _booleans },
            { "gearRetracting", _booleans },
            { "onLandBeg", _booleans },
            { "manualControl", _booleans },
            { "hasGunner", _booleans },
            { "enableAutoActions", _booleans },
            { "lockWhenDriverOut", _booleans },
            { "gunnerForceOptics", _booleans },
            { "hideWeaponsCargo", _booleans },
            { "east", _booleans },
            { "west", _booleans },
            { "enableOptics", _booleans },
            { "useAction", _booleans },
            { "useAsBinocular", _booleans },
            { "hasCommander", _booleans },
            { "useInternalLODInVehicles", _booleans },
            { "preload", _booleans },
            { "showAimCursorInternal", _booleans },
            { "shotFromTurret", _booleans },
            { "enableMissile", _booleans },
            { "rightHandIKBeg", _booleans },
            { "autoReload", _booleans },
            { "inGunnerMayFire", _booleans },
            { "airLock", _booleans },
            { "woman", _booleans },
            { "disableWeaponsLong", _booleans },
            { "canHideBodies", _booleans },
            { "opticsFlare", _booleans },
            { "laserLock", _booleans },
            { "ejectDeadGunner", _booleans },
            { "isMan", _booleans },
            { "onLandEnd", _booleans },
            { "irScanGround", _booleans },
            { "canFloat", _booleans },
            { "showHandGun", _booleans },
            { "primary", _booleans },
            { "gunnerHasFlares", _booleans },
            { "leftHandIKEnd", _booleans },
            { "opticsDisablePeripherialVision", _booleans },
            { "canBeShot", _booleans },
            { "viewCargoShadow", _booleans },
            { "leftHandIKBeg", _booleans },
            { "irLock", _booleans },
            { "autoFire", _booleans },
            { "soundEnabled", _booleans },
            { "forceOptics", _booleans },
            { "enableSweep", _booleans },
            { "looped", _booleans },
            { "optics", _booleans },
            { "laser", _booleans },
            { "showEmpty", _booleans },
            { "driverForceOptics", _booleans },
            { "reversed", _booleans },
            { "castGunnerShadow", _booleans },
            { "showSwitchAction", _booleans },
            { "hideWeaponsGunner", _booleans },
            { "hideProxyInCombat", _booleans },
            { "disappearAtContact", _booleans },
            { "limitGunMovement", _booleans },
            { "outGunnerMayFire", _booleans },
            { "autocenter", _booleans },
            { "textureTrackWheel", _booleans },
            { "showItemInHand", _booleans },
            { "startEngine", _booleans },
            { "forceHideGunner", _booleans },
            { "laserTarget", _booleans },
            { "backgroundReload", _booleans },
            { "showToPlayer", _booleans },
            { "canDrop", _booleans },
            { "ballisticsComputer", _booleans },
            { "soundBurst", _booleans },
            { "soundContinuous", _booleans },
            { "nightVision", _booleans },
            { "blinking", _booleans },
            { "interpolationRestart", _booleans },
            { "terminal", _booleans },
            { "hideUnitInfo", _booleans },
            { "irTarget", _booleans },
            { "viewGunnerShadow", _booleans },
            { "castCargoShadow", _booleans },
            { "passThrough", _booleans }
        };

        /// <summary>
        /// Gets a list of defines associated with raP value name.
        /// </summary>
        /// <param name="rapValueName">raP value name.</param>
        /// <returns>Returns a list of defines associated with rapValueName param.</returns>
        public static List<RapDefinePreprocessor> GetDefinesForRapValue(string rapValueName)
        {
            if (_defines.TryGetValue(rapValueName, out var defines)) {
                return defines;
            }

            return new List<RapDefinePreprocessor>(0);
        }
    }
}
