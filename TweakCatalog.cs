using DvrkTweaks.Models;

namespace DvrkTweaks.Data;

public static class TweakCatalog
{
    private static Tweak T(string name, string description, string actionId, bool enabled = false, bool requiresAdmin = false)
        => new(name, description, actionId, enabled, requiresAdmin);

    public static readonly Dictionary<string, List<Tweak>> Categories = new()
    {
        ["FPS Tweaks"] = new()
        {
            T("Game Mode", "Prioritise gaming workloads through Windows Game Mode.", "game_on"),
            T("Hardware GPU Scheduling", "Enable hardware accelerated GPU scheduling when supported.", "hags_on"),
            T("Fullscreen Optimisations", "Enable fullscreen optimisation behaviour for supported games.", "fullscreen_on"),
            T("Visual Performance", "Reduce Windows visual effects for a lighter desktop.", "visual_perf"),
            T("Transparency Off", "Disable transparency effects to reduce desktop overhead.", "transparency_off"),
            T("Animations Off", "Reduce Windows animation effects.", "animations_off"),
            T("Background Apps", "Reduce optional background activity while gaming.", "background_off"),
            T("High Performance Power", "Switch to the High Performance power plan.", "power_high"),
            T("Foreground Priority", "Prioritise the active application workload.", "foreground"),
            T("Game DVR Capture Off", "Disable background Game DVR capture.", "gamedvr_off"),
            T("Mouse Acceleration Off", "Disable Enhance Pointer Precision.", "mouse_accel_off"),
            T("Notifications Off", "Reduce Windows notification interruptions.", "notifications_off")
        },

        ["Network Tweaks"] = new()
        {
            T("TCP Auto-Tuning", "Use normal Windows TCP receive-window auto-tuning.", "tcp_normal"),
            T("DNS Cache Flush", "Flush the Windows DNS resolver cache.", "dns_flush"),
            T("Network Throttling", "Adjust multimedia network throttling behaviour.", "network_throttle_off"),
            T("Adapter Performance", "Prefer network adapter performance over power saving.", "adapter_perf"),
            T("Delivery Optimisation Pause", "Pause background Windows delivery activity.", "delivery_pause"),
            T("Metered Connection Guide", "Open Windows metered connection settings.", "metered_guide", false),
            T("IPv6 Guide", "Open IPv6 adapter settings for inspection.", "ipv6_guide", false),
            T("Proxy Guide", "Open Windows proxy settings without changing security settings.", "proxy_guide", false),
            T("Wi-Fi Roaming", "Open adapter roaming settings for compatible Wi-Fi hardware.", "wifi_roaming_guide", false),
            T("Ethernet Duplex", "Open Ethernet adapter duplex settings.", "duplex_guide", false),
            T("DNS Settings", "Open adapter settings for manual DNS configuration.", "dns_settings_guide", false),
            T("Connection Test", "Ping 1.1.1.1 for a quick reachability check.", "ping_test")
        },

        ["Fortnite Tweaks"] = new()
        {
            T("Fortnite Game Mode", "Enable Windows Game Mode for Fortnite sessions.", "game_on"),
            T("Fortnite Game DVR Off", "Disable background capture while playing.", "gamedvr_off"),
            T("Fortnite Performance Power", "Use the High Performance Windows power plan.", "power_high"),
            T("Fortnite HAGS", "Enable hardware GPU scheduling when supported.", "hags_on"),
            T("Fortnite Fullscreen", "Enable fullscreen optimisation behaviour.", "fullscreen_on"),
            T("Fortnite Background Apps", "Reduce optional background activity.", "background_off"),
            T("Fortnite Notifications", "Reduce notification interruptions during matches.", "notifications_off"),
            T("Fortnite Animations", "Reduce Windows animation effects.", "animations_off"),
            T("Fortnite Transparency", "Disable transparency effects.", "transparency_off"),
            T("Fortnite Mouse Input", "Disable Windows mouse acceleration.", "mouse_accel_off"),
            T("Fortnite Foreground Priority", "Prioritise the active Fortnite workload.", "foreground"),
            T("Fortnite DNS Refresh", "Flush DNS before a session.", "dns_flush")
        },

        ["Windows Tweaks"] = new()
        {
            T("Game Mode", "Enable Windows Game Mode.", "game_on"),
            T("Transparency", "Disable transparency effects.", "transparency_off"),
            T("Animations", "Reduce animation effects.", "animations_off"),
            T("Notifications", "Reduce notification interruptions.", "notifications_off"),
            T("Clipboard History", "Open clipboard-related Windows settings.", "clipboard_off"),
            T("Background Apps", "Reduce optional background activity.", "background_off"),
            T("Power Throttling", "Adjust Windows foreground power-throttling behaviour.", "throttle_off"),
            T("Mouse Acceleration", "Disable Enhance Pointer Precision.", "mouse_accel_off"),
            T("Game Bar Capture", "Disable background Xbox/Game Bar capture.", "xboxbar_off"),
            T("High Performance Plan", "Use the High Performance power plan.", "power_high"),
            T("Focus Assist", "Open Focus Assist / notification controls.", "focus_guide", false),
            T("Windows Update", "Open Windows Update settings for manual review.", "update_guide", false)
        },

        ["GPU Tweaks"] = new()
        {
            T("Hardware GPU Scheduling", "Enable HAGS when supported by the GPU and driver.", "hags_on"),
            T("High Performance Plan", "Use the High Performance Windows power plan.", "power_high"),
            T("Shader Cache", "Open GPU shader-cache controls for inspection.", "shader_guide", false),
            T("VSync Guide", "Open display/GPU controls to review VSync.", "vsync_guide", false),
            T("Frame Pacing Guide", "Open graphics settings for frame-pacing review.", "graphics_guide", false),
            T("Windows Graphics", "Open Windows per-app graphics settings.", "graphics_guide", false),
            T("AMD Adrenalin", "Open AMD graphics software guidance.", "amd_guide", false),
            T("NVIDIA Control Panel", "Open NVIDIA graphics settings guidance.", "nvidia_guide", false),
            T("DirectX Graphics", "Open Windows graphics settings for DirectX apps.", "graphics_guide", false),
            T("Variable Refresh Rate", "Open Windows VRR settings.", "vrr_guide", false),
            T("HDR", "Open Windows HDR display settings.", "hdr_guide", false),
            T("Display Refresh Rate", "Open display settings to choose the supported refresh rate.", "refresh_guide", false)
        },

        ["CPU Tweaks"] = new()
        {
            T("High Performance Plan", "Use the High Performance Windows power plan.", "power_high"),
            T("Balanced Plan", "Switch back to the Balanced power plan.", "power_balanced"),
            T("Minimum Processor State", "Set the plugged-in minimum processor state.", "cpu_min_100"),
            T("Maximum Processor State", "Set the plugged-in maximum processor state to 100%.", "cpu_max_100"),
            T("Foreground Priority", "Prioritise the active application workload.", "foreground"),
            T("Core Parking Guide", "Open processor/power settings for core parking review.", "processor_guide", false),
            T("Power Saver", "Open Windows power settings for lower-power profiles.", "processor_guide", false),
            T("Processor Settings", "Open Windows processor power controls.", "processor_guide", false),
            T("Startup Apps", "Open startup-app controls to reduce background CPU load.", "startup_guide", false),
            T("Background Apps", "Reduce optional background activity.", "background_off"),
            T("Game Mode", "Enable Windows Game Mode.", "game_on"),
            T("CPU Cooling Guide", "Open Windows tools and guidance for cooling checks.", "cooling_guide", false)
        },

        ["RAM Tweaks"] = new()
        {
            T("Memory Compression Guide", "Open Windows memory controls for inspection.", "memory_guide", false),
            T("Background Apps", "Reduce optional apps using memory in the background.", "background_off"),
            T("Startup Apps", "Review startup apps that consume memory.", "startup_guide", false),
            T("Pagefile Settings", "Open virtual-memory/pagefile settings.", "pagefile_guide", false),
            T("Virtual Memory", "Open Windows virtual-memory configuration.", "pagefile_guide", false),
            T("Game Priority", "Prioritise the active application workload.", "foreground"),
            T("Game Mode", "Enable Windows Game Mode.", "game_on"),
            T("Clipboard History", "Open clipboard settings to manage stored items.", "clipboard_off"),
            T("Notifications", "Reduce notification activity in the background.", "notifications_off"),
            T("Visual Effects", "Reduce Windows visual effects.", "visual_perf"),
            T("Animations", "Reduce animation effects.", "animations_off"),
            T("Transparency", "Disable transparency effects.", "transparency_off")
        },

        ["Storage Tweaks"] = new()
        {
            T("Storage Sense", "Open Windows Storage Sense controls.", "storage_guide", false),
            T("SSD TRIM", "Enable SSD TRIM behaviour where supported.", "trim_on", true, true),
            T("Temporary Files", "Clean user temporary files.", "temp_clean"),
            T("Recycle Bin", "Empty the Recycle Bin.", "recycle_clean"),
            T("Thumbnail Cache", "Open Windows cleanup tools for thumbnail data.", "cleanmgr", false),
            T("Delivery Cache", "Clean Windows Delivery Optimisation cache.", "delivery_clean"),
            T("Windows Update Cleanup", "Open Windows cleanup tools.", "cleanmgr", false),
            T("Drive Optimisation", "Open Windows drive optimisation.", "defrag_guide", false),
            T("Disk Management", "Open Disk Management.", "diskmgmt", false),
            T("Storage Settings", "Open Windows storage settings.", "storage_guide", false),
            T("Search Indexing", "Open Windows search indexing settings.", "index_guide", false),
            T("Write Caching", "Open drive write-cache policy settings.", "writecache_guide", false)
        },

        ["System Tweaks"] = new()
        {
            T("Create Restore Point", "Open System Protection before making major changes.", "restore_guide", false),
            T("System Game Mode", "Enable Windows Game Mode.", "game_on"),
            T("System High Performance", "Use the High Performance power plan.", "power_high"),
            T("System Transparency", "Disable transparency effects.", "transparency_off"),
            T("System Animations", "Reduce Windows animation effects.", "animations_off"),
            T("System Notifications", "Reduce notification interruptions.", "notifications_off"),
            T("System Background Apps", "Reduce optional background activity.", "background_off"),
            T("System Mouse Acceleration", "Disable Enhance Pointer Precision.", "mouse_accel_off"),
            T("System Game DVR", "Disable background Game DVR capture.", "gamedvr_off"),
            T("System Clipboard", "Open clipboard history controls.", "clipboard_off"),
            T("System Power Throttling", "Adjust foreground power-throttling behaviour.", "throttle_off"),
            T("System Information", "Open Windows System Information.", "msinfo", false)
        },

        ["Latency / Ping"] = new()
        {
            T("DNS Flush", "Flush the DNS resolver cache before testing.", "dns_flush"),
            T("TCP Auto-Tuning", "Use normal TCP auto-tuning.", "tcp_normal"),
            T("Network Throttling", "Adjust multimedia network throttling behaviour.", "network_throttle_off"),
            T("Adapter Performance", "Prefer network adapter performance.", "adapter_perf"),
            T("Delivery Optimisation", "Pause background delivery activity.", "delivery_pause"),
            T("1.1.1.1 Connection Test", "Ping 1.1.1.1 to measure basic reachability.", "ping_test"),
            T("Windows Network Diagnostics", "Run Windows network diagnostics.", "network_diag"),
            T("Proxy Settings", "Open Windows proxy settings.", "proxy_guide", false),
            T("DNS Configuration", "Open adapter settings for DNS configuration.", "dns_settings_guide", false),
            T("Firewall Settings", "Open Firewall settings without disabling protection.", "firewall_guide", false),
            T("Wi-Fi Adapter", "Open Wi-Fi adapter settings.", "wifi_guide", false),
            T("Ethernet Adapter", "Open Ethernet adapter settings.", "ethernet_guide", false)
        }
    };

    public static readonly List<string> PS5Checklist = new()
    {
        "Use Performance Mode for games that support it.",
        "Enable 120 Hz when your display and game support 120 FPS.",
        "Enable VRR if both your PS5 and display support it.",
        "Turn on Game Mode on your TV/monitor.",
        "Use Ethernet for the most consistent competitive connection when possible.",
        "Run the PS5 Internet Connection Test after network changes.",
        "Keep PS5 system software updated.",
        "Keep games updated before competitive sessions.",
        "Leave enough free SSD space for updates and temporary data.",
        "Use a wired controller connection when you want to minimise wireless variables.",
        "Close games/apps you are not using before a competitive session.",
        "Check the display's correct HDMI input and supported refresh rate.",
        "Use the PS5 Safe Mode only for troubleshooting when necessary.",
        "Restart the PS5 and network equipment if you have persistent connection issues.",
        "Avoid unnecessary picture-processing modes on the TV that add input latency."
    };
}
