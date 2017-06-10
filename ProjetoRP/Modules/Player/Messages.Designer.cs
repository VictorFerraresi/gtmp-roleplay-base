﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjetoRP.Modules.Player {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ProjetoRP.Modules.Player.Messages", typeof(Messages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to login.
        /// </summary>
        public static string command_login {
            get {
                return ResourceManager.GetString("command_login", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Módulo Player carregado!.
        /// </summary>
        public static string console_startup {
            get {
                return ResourceManager.GetString("console_startup", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ~r~Sua conta não está ativada. Verifique seu email ou contate um administrador..
        /// </summary>
        public static string player_account_not_activated {
            get {
                return ResourceManager.GetString("player_account_not_activated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Você está com o CEF desabilitado e sua experiência de jogo será afetada. Habilite-o nas opções.
        /// </summary>
        public static string player_cef_disabled {
            get {
                return ResourceManager.GetString("player_cef_disabled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Você está com o CEF desabilitado e sua experiência de jogo será afetada. Habilite-o nas opções..
        /// </summary>
        public static string player_cef_is_disabled {
            get {
                return ResourceManager.GetString("player_cef_is_disabled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Você não possui um personagem nesse slot!.
        /// </summary>
        public static string player_character_idx_not_exists {
            get {
                return ResourceManager.GetString("player_character_idx_not_exists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ({0}) {1}: Level {2} ({3}/{4}) ${5} + ${6} (banco).
        /// </summary>
        public static string player_character_n {
            get {
                return ResourceManager.GetString("player_character_n", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Você foi kickado por inconsistencia de informações transmitidas..
        /// </summary>
        public static string player_kicked_inconsistency {
            get {
                return ResourceManager.GetString("player_kicked_inconsistency", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Seu último login foi {0} pelo IP {1}. Desde então, você teve {2} tentativas de login falhas..
        /// </summary>
        public static string player_last_logins {
            get {
                return ResourceManager.GetString("player_last_logins", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Seu login foi bloqueado..
        /// </summary>
        public static string player_login_blocked {
            get {
                return ResourceManager.GetString("player_login_blocked", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Você fez o login com sucesso!.
        /// </summary>
        public static string player_login_success {
            get {
                return ResourceManager.GetString("player_login_success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Você não está proximo a nada que possa comprar!.
        /// </summary>
        public static string player_not_near_any_buyable {
            get {
                return ResourceManager.GetString("player_not_near_any_buyable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Você ainda não está registrado em nosso servidor..
        /// </summary>
        public static string player_not_registered {
            get {
                return ResourceManager.GetString("player_not_registered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Para realizar login, por favor digite suas informações abaixo..
        /// </summary>
        public static string player_please_login {
            get {
                return ResourceManager.GetString("player_please_login", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Para realizar login, digite /login [usuário] [senha]..
        /// </summary>
        public static string player_please_login_nocef {
            get {
                return ResourceManager.GetString("player_please_login_nocef", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Você já possui uma conta. Faça o login agora com /login..
        /// </summary>
        public static string player_registered {
            get {
                return ResourceManager.GetString("player_registered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ~w~Bem-vindo ao ~o~Projeto~w~RP.
        /// </summary>
        public static string player_welcome_message {
            get {
                return ResourceManager.GetString("player_welcome_message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Você errou a senha. Tente novamente. Você está na tentativa {0} de {1}.
        /// </summary>
        public static string player_wrong_password {
            get {
                return ResourceManager.GetString("player_wrong_password", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ~r~Você está banido até {0}.
        /// </summary>
        public static string player_you_are_banned_until {
            get {
                return ResourceManager.GetString("player_you_are_banned_until", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Estes são os seus personagens. Use /spawn [n] para jogar:.
        /// </summary>
        public static string player_your_characters {
            get {
                return ResourceManager.GetString("player_your_characters", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Você está logado como ID {0}.
        /// </summary>
        public static string player_your_id_is {
            get {
                return ResourceManager.GetString("player_your_id_is", resourceCulture);
            }
        }
    }
}
