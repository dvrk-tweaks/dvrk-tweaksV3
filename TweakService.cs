using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace DvrkTweaks.Services;

public static class TweakService
{
    private static void Reg(string path, string name, int value)
    {
        using var key = Registry.CurrentUser.CreateSubKey(path);
        key?.SetValue(name, value, RegistryValueKind.DWord);
    }

    private static void Run(string file, string args)
    {
        using var process = Process.Start(new ProcessStartInfo(file, args)
        {
            CreateNoWindow = true,
            UseShellExecute = false
        });
        process?.WaitForExit(10000);
    }

    private static void Open(string target)
    {
        Process.Start(new ProcessStartInfo(target) { UseShellExecute = true });
    }

    public static string Set(string id, bool enabled)
    {
        try
        {
            if (enabled)
                return Apply(id);

            switch (id)
            {
                case "game_on": Reg(@"Software\Microsoft\GameBar", "AutoGameModeEnabled", 0); break;
                case "gamedvr_off":
                case "xboxbar_off": Reg(@"Software\Microsoft\Windows\CurrentVersion\GameDVR", "AppCaptureEnabled", 1); break;
                case "hags_on": Reg(@"SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwSchMode", 1); break;
                case "fullscreen_on": Reg(@"Software\Microsoft\Windows\CurrentVersion\GameDVR", "FullscreenOnly", 0); break;
                case "visual_perf":
                case "animations_off": Reg(@"Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects", "VisualFXSetting", 0); break;
                case "transparency_off": Reg(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 1); break;
                case "power_high": Run("powercfg", "/SETACTIVE SCHEME_BALANCED"); break;
                case "power_balanced": Run("powercfg", "/SETACTIVE SCHEME_MIN"); break;
                case "foreground": Reg(@"SYSTEM\CurrentControlSet\Control\PriorityControl", "Win32PrioritySeparation", 26); break;
                case "mouse_accel_off":
                    Reg(@"Control Panel\Mouse", "MouseSpeed", 1);
                    Reg(@"Control Panel\Mouse", "MouseThreshold1", 6);
                    Reg(@"Control Panel\Mouse", "MouseThreshold2", 10);
                    break;
                case "notifications_off": Reg(@"Software\Microsoft\Windows\CurrentVersion\PushNotifications", "ToastEnabled", 1); break;
                case "clipboard_off": Reg(@"Software\Microsoft\Clipboard", "EnableClipboardHistory", 1); break;
                case "throttle_off": Reg(@"SYSTEM\CurrentControlSet\Control\Power\PowerThrottling", "PowerThrottlingOff", 0); break;
                case "network_throttle_off": Reg(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile", "NetworkThrottlingIndex", 10); break;
                case "cpu_min_100": Run("powercfg", "/SETACVALUEINDEX SCHEME_CURRENT SUB_PROCESSOR PROCTHROTTLEMIN 5"); break;
                case "cpu_max_100": Run("powercfg", "/SETACVALUEINDEX SCHEME_CURRENT SUB_PROCESSOR PROCTHROTTLEMAX 100"); break;
                case "trim_on": Run("fsutil", "behavior set DisableDeleteNotify 1"); break;
                default:
                    return "This item does not have a safe reversible OFF action.";
            }
            return "Disabled successfully.";
        }
        catch (Exception ex)
        {
            return "Could not change setting: " + ex.Message;
        }
    }

    public static string Apply(string id)
    {
        try
        {
            switch (id)
            {
                case "game_on": Reg(@"Software\Microsoft\GameBar", "AutoGameModeEnabled", 1); break;
                case "gamedvr_off":
                case "xboxbar_off": Reg(@"Software\Microsoft\Windows\CurrentVersion\GameDVR", "AppCaptureEnabled", 0); break;
                case "hags_on": Reg(@"SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwSchMode", 2); break;
                case "fullscreen_on": Reg(@"Software\Microsoft\Windows\CurrentVersion\GameDVR", "FullscreenOnly", 1); break;
                case "visual_perf":
                    Reg(@"Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects", "VisualFXSetting", 2); break;
                case "transparency_off": Reg(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "EnableTransparency", 0); break;
                case "animations_off": Reg(@"Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects", "VisualFXSetting", 2); break;
                case "background_off": break;
                case "power_high": Run("powercfg", "/SETACTIVE SCHEME_MIN"); break;
                case "power_balanced": Run("powercfg", "/SETACTIVE SCHEME_BALANCED"); break;
                case "foreground": Reg(@"SYSTEM\CurrentControlSet\Control\PriorityControl", "Win32PrioritySeparation", 38); break;
                case "mouse_accel_off":
                    Reg(@"Control Panel\Mouse", "MouseSpeed", 0);
                    Reg(@"Control Panel\Mouse", "MouseThreshold1", 0);
                    Reg(@"Control Panel\Mouse", "MouseThreshold2", 0);
                    break;
                case "notifications_off": Reg(@"Software\Microsoft\Windows\CurrentVersion\PushNotifications", "ToastEnabled", 0); break;
                case "clipboard_off": Reg(@"Software\Microsoft\Clipboard", "EnableClipboardHistory", 0); break;
                case "throttle_off": Reg(@"SYSTEM\CurrentControlSet\Control\Power\PowerThrottling", "PowerThrottlingOff", 1); break;
                case "tcp_normal": Run("netsh", "interface tcp set global autotuninglevel=normal"); break;
                case "dns_flush": Run("ipconfig", "/flushdns"); break;
                case "network_throttle_off": Reg(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile", "NetworkThrottlingIndex", -1); break;
                case "adapter_perf": break;
                case "delivery_pause": break;
                case "ping_test": Run("powershell", "-NoProfile -Command \"Test-Connection 1.1.1.1 -Count 4\""); break;
                case "network_diag": Run("msdt.exe", "/id NetworkDiagnosticsNetworkAdapter"); break;
                case "cpu_min_100": Run("powercfg", "/SETACVALUEINDEX SCHEME_CURRENT SUB_PROCESSOR PROCTHROTTLEMIN 100"); break;
                case "cpu_max_100": Run("powercfg", "/SETACVALUEINDEX SCHEME_CURRENT SUB_PROCESSOR PROCTHROTTLEMAX 100"); break;
                case "trim_on": Run("fsutil", "behavior set DisableDeleteNotify 0"); break;
                case "temp_clean": CleanTemp(); break;
                case "recycle_clean": Run("powershell", "-NoProfile -Command Clear-RecycleBin -Force"); break;
                case "delivery_clean": break;
                case "taskmgr": Open("taskmgr.exe"); break;
                case "resmon": Open("resmon.exe"); break;
                case "services": Open("services.msc"); break;
                case "devmgmt": Open("devmgmt.msc"); break;
                case "msinfo": Open("msinfo32.exe"); break;
                case "diskmgmt": Open("diskmgmt.msc"); break;
                case "cleanmgr": Open("cleanmgr.exe"); break;
                case "storage_guide": Open("ms-settings:storagesense"); break;
                case "startup_guide": Open("ms-settings:startupapps"); break;
                case "focus_guide": Open("ms-settings:quiethours"); break;
                case "update_guide": Open("ms-settings:windowsupdate"); break;
                case "graphics_guide": Open("ms-settings:display-advancedgraphics"); break;
                case "vrr_guide": Open("ms-settings:display-advanced"); break;
                case "hdr_guide": Open("ms-settings:display"); break;
                case "refresh_guide": Open("ms-settings:display-advanced"); break;
                case "driver_guide": Open("ms-settings:windowsupdate-optionalupdates"); break;
                case "gpu_reset_guide": return "Use Win+Ctrl+Shift+B only as a troubleshooting reset for the graphics driver.";
                case "proxy_guide": Open("ms-settings:network-proxy"); break;
                case "network_reset_guide": Open("ms-settings:network-advancedsettings"); break;
                case "wifi_guide": Open("ms-settings:network-wifi"); break;
                case "ethernet_guide": Open("ms-settings:network-ethernet"); break;
                case "network_settings": Open("ms-settings:network"); break;
                case "firewall_guide": Open("windowsdefender://network/"); break;
                case "dns_settings_guide": Open("ms-settings:network-advancedsettings"); break;
                case "processor_guide": Open("powercfg.cpl"); break;
                case "cooling_guide": Open("powercfg.cpl"); break;
                case "pagefile_guide": Open("sysdm.cpl"); break;
                case "memory_guide": Open("taskmgr.exe"); break;
                case "shader_guide": Open("ms-settings:display"); break;
                case "vsync_guide": Open("ms-settings:display-advanced"); break;
                case "amd_guide": Open("ms-settings:display-advancedgraphics"); break;
                case "nvidia_guide": Open("ms-settings:display-advancedgraphics"); break;
                case "nvidia_reflex": return "This option is NVIDIA-only and was not changed.";
                case "restore_guide": Open("sysdm.cpl"); break;
                case "temp_open": Open(Path.GetTempPath()); break;
                case "downloads_open": Open(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads")); break;
                case "thispc": Open("explorer.exe"); break;
                case "index_guide": Open("control.exe", "srchadmin.dll"); break;
                case "writecache_guide": Open("devmgmt.msc"); break;
                case "defrag_guide": Open("dfrgui.exe"); break;
                case "metered_guide": Open("ms-settings:network"); break;
                case "ipv6_guide": Open("ms-settings:network-advancedsettings"); break;
                case "wifi_roaming_guide": Open("ncpa.cpl"); break;
                case "duplex_guide": Open("ncpa.cpl"); break;
                case "system_settings": Open("ms-settings:"); break;
                default: return "No automatic system action is attached to this item.";
            }

            return "Applied successfully.";
        }
        catch (Exception ex)
        {
            return "Could not apply: " + ex.Message;
        }
    }

    private static void CleanTemp()
    {
        foreach (var file in Directory.EnumerateFiles(Path.GetTempPath(), "*", SearchOption.TopDirectoryOnly))
        {
            try { File.Delete(file); } catch { }
        }
    }

    private static void Open(string file, string args)
    {
        Process.Start(new ProcessStartInfo(file, args) { UseShellExecute = true });
    }
}
