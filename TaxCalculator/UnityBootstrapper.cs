using System.Web.Http;
using Unity;
using Service;

namespace TaxCalculator
{
    public static class UnityBootstrapper
    {
        public static void Init(HttpConfiguration config)
        {
            var container = new UnityContainer();
            container.RegisterType<IMessageProcessor, MessageProcessor>();
            config.DependencyResolver = new UnityResolver(container);
        }
    }
}