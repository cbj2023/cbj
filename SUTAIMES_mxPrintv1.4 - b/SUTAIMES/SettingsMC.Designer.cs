﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SUTAIMES {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.9.0.0")]
    internal sealed partial class SettingsMC : global::System.Configuration.ApplicationSettingsBase {
        
        private static SettingsMC defaultInstance = ((SettingsMC)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new SettingsMC())));
        
        public static SettingsMC Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public int MBOutCmdFlow {
            get {
                return ((int)(this["MBOutCmdFlow"]));
            }
            set {
                this["MBOutCmdFlow"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public int ZKOutCmdFlow {
            get {
                return ((int)(this["ZKOutCmdFlow"]));
            }
            set {
                this["ZKOutCmdFlow"] = value;
            }
        }
    }
}
