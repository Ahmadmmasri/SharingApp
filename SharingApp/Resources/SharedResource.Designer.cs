﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SharingApp {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class SharedResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SharedResource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SharingApp.Resources.SharedResource", typeof(SharedResource).Assembly);
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
        ///   Looks up a localized string similar to Password Added Successfuly.
        /// </summary>
        public static string AddPassMessage {
            get {
                return ResourceManager.GetString("AddPassMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Password changed successfully please login again.
        /// </summary>
        public static string ChangedPassMessage {
            get {
                return ResourceManager.GetString("ChangedPassMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Passwords are not the same.
        /// </summary>
        public static string CompairePass {
            get {
                return ResourceManager.GetString("CompairePass", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Confirm Paswword.
        /// </summary>
        public static string ConfirmPassLable {
            get {
                return ResourceManager.GetString("ConfirmPassLable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You Shuld Type an Email Address.
        /// </summary>
        public static string Email {
            get {
                return ResourceManager.GetString("Email", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Email.
        /// </summary>
        public static string EmailLable {
            get {
                return ResourceManager.GetString("EmailLable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to First name.
        /// </summary>
        public static string FirstName {
            get {
                return ResourceManager.GetString("FirstName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Last name.
        /// </summary>
        public static string LastName {
            get {
                return ResourceManager.GetString("LastName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Password.
        /// </summary>
        public static string PassLable {
            get {
                return ResourceManager.GetString("PassLable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} is Required.
        /// </summary>
        public static string Required {
            get {
                return ResourceManager.GetString("Required", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to you should confirm your email.
        /// </summary>
        public static string RequiredEmailConfirmation {
            get {
                return ResourceManager.GetString("RequiredEmailConfirmation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Operation Completed Successfully.
        /// </summary>
        public static string SuccessMessage {
            get {
                return ResourceManager.GetString("SuccessMessage", resourceCulture);
            }
        }
    }
}
