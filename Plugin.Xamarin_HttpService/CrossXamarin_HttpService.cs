using Plugin.Xamarin_HttpService.Abstractions;
using Plugin.Xamarin_HttpService.Abstractions.Interfaces;
using Plugin.Xamarin_HttpService.Implementation;
using System;

namespace Plugin.Xamarin_HttpService
{
    /// <summary>
    /// Cross platform Xamarin_HttpService implemenations
    /// </summary>
    public class CrossXamarin_HttpService
    {
        static Lazy<IXamarin_HttpService_PlatformImplementation> Implementation = new Lazy<IXamarin_HttpService_PlatformImplementation>(() => CreateXamarin_HttpService(), System.Threading.LazyThreadSafetyMode.PublicationOnly);
        static Lazy<IXamarin_HttpService> PCL_Implementation = new Lazy<IXamarin_HttpService>(() => CreateXamarin_HttpService_PCL(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Cross platform Xamarin_HttpService implemenations
        /// </summary>
        /// 
#if PORTABLE
        public static bool Init(string BaseURL)
        {
            try
            {
                Settings.Startup_Settings.Base_URL = BaseURL;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
#endif

        /// <summary>
        /// Current settings to use
        /// </summary>
        public static IXamarin_HttpService_PlatformImplementation Current
        {
            get
            {
                var ret = Implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        /// <summary>
        /// Current settings to use for PCL
        /// </summary>
        /// 
#if PORTABLE
        public static IXamarin_HttpService Current_PCL
        {
            get
            {
                var ret = PCL_Implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }
#endif

        static IXamarin_HttpService_PlatformImplementation CreateXamarin_HttpService()
        {
#if PORTABLE
            return null;
#else
        return new Xamarin_HttpServiceImplementation();
#endif
        }

        static IXamarin_HttpService CreateXamarin_HttpService_PCL()
        {
#if PORTABLE
            return new Xamarin_HttpServicePCLImplementation();
#else
            return null;
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}
