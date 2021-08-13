﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace news_scrapper.resources {
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
    public class ApiResponses {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ApiResponses() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("news_scrapper.resources.ApiResponses", typeof(ApiResponses).Assembly);
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
        ///   Looks up a localized string similar to Articles added after scrapping: &apos;{0}&apos;..
        /// </summary>
        public static string ArticlesAddedAfterScrapping {
            get {
                return ResourceManager.GetString("ArticlesAddedAfterScrapping", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Articles not found..
        /// </summary>
        public static string ArticlesNotFound {
            get {
                return ResourceManager.GetString("ArticlesNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Article with id: &apos;{0}&apos; not found..
        /// </summary>
        public static string ArticleWithIdNotFound {
            get {
                return ResourceManager.GetString("ArticleWithIdNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot get main news node by xpath: &apos;{0}&apos;..
        /// </summary>
        public static string CannotGetMainNewsNodeByXpath {
            get {
                return ResourceManager.GetString("CannotGetMainNewsNodeByXpath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot get title from main node..
        /// </summary>
        public static string CannotGetTitleFromMainNode {
            get {
                return ResourceManager.GetString("CannotGetTitleFromMainNode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot  reach site with given url: &apos;{0}&apos;..
        /// </summary>
        public static string CannotReachSiteWithGivenUrl {
            get {
                return ResourceManager.GetString("CannotReachSiteWithGivenUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There are no websites to scrap..
        /// </summary>
        public static string ThereAreNoWebsitesToScrap {
            get {
                return ResourceManager.GetString("ThereAreNoWebsitesToScrap", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unexpected error occured: &apos;{0}&apos;..
        /// </summary>
        public static string UnexpectedErrorOccured {
            get {
                return ResourceManager.GetString("UnexpectedErrorOccured", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Website details cannot be null..
        /// </summary>
        public static string WebsiteDetailsCannotBeNull {
            get {
                return ResourceManager.GetString("WebsiteDetailsCannotBeNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Website details news node cannot be null or empty..
        /// </summary>
        public static string WebsiteDetailsNewsNodeCannotBeNullOrEmpty {
            get {
                return ResourceManager.GetString("WebsiteDetailsNewsNodeCannotBeNullOrEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Website details url cannot be null or empty..
        /// </summary>
        public static string WebsiteDetailsUrlCannotBeNullOrEmpty {
            get {
                return ResourceManager.GetString("WebsiteDetailsUrlCannotBeNullOrEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Website details Xpath cannot be null or empty..
        /// </summary>
        public static string WebsiteDetailsXpathCannotBeNullOrEmpty {
            get {
                return ResourceManager.GetString("WebsiteDetailsXpathCannotBeNullOrEmpty", resourceCulture);
            }
        }
    }
}
