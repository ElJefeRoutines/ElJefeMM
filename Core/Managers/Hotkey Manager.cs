using Styx.Common;
using Styx.WoWInternals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;



namespace ElJefeMM.Core.Managers
{
    class Hotkey_Manager
    {
        public static bool aoeOn { get; set; }
        public static bool cooldownsOn { get; set; }
        public static bool manualOn { get; set; }
        public static bool keysRegistered { get; set; }

        #region [Method] - Hotkey Registration
        public static void registerHotKeys()
        {
            if (keysRegistered)
                return;
            HotkeysManager.Register("aoeOn", Keys.Q, ModifierKeys.Alt, ret =>
            {
                aoeOn = !aoeOn;
                Lua.DoString(aoeOn ? @"print('AoE Mode: \124cFF15E61C Enabled!')" : @"print('AoE Mode: \124cFFE61515 Disabled!')");
            });
            HotkeysManager.Register("cooldownsOn", Keys.E, ModifierKeys.Alt, ret =>
            {
                cooldownsOn = !cooldownsOn;
                Lua.DoString(cooldownsOn ? @"print('Cooldowns: \124cFF15E61C Enabled!')" : @"print('Cooldowns: \124cFFE61515 Disabled!')");
            });
            HotkeysManager.Register("manualOn", Keys.S, ModifierKeys.Alt, ret =>
            {
                manualOn = !manualOn;
                Lua.DoString(manualOn ? @"print('Manual Mode: \124cFF15E61C Enabled!')" : @"print('Manual Mode: \124cFFE61515 Disabled!')");
            });
            keysRegistered = true;
            Lua.DoString(@"print('Hotkeys: \124cFF15E61C Registered!')");
            Logging.Write(Colors.OrangeRed, "Hotkeys: Registered!");
        }  
        #endregion

        #region [Method] - Hotkey Removal
        public static void removeHotkeys()
        {
            if (!keysRegistered)
                return;
            HotkeysManager.Unregister("aoeOn");
            HotkeysManager.Unregister("cooldownsOn");
            HotkeysManager.Unregister("manualOn");
            aoeOn = false;
            cooldownsOn = false;
            manualOn = false;
            keysRegistered = false;
            Lua.DoString(@"print('Hotkeys: \124cFFE61515 Removed!')");
            Logging.Write(Colors.OrangeRed, "Hotkeys: Removed!");
        }  
        #endregion
    }
}
