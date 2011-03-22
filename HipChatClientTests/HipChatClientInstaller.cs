using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace HipChatClientTests
{
    public class HipChatClientInstaller: IWindsorInstaller
    {
        /// <summary>
        /// Custom installer - reads the config from an XML file.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="store"></param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Install(Configuration.FromAppConfig());
        }
    }
}