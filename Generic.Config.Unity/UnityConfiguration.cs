
using Microsoft.Practices.Unity;
namespace Generic.Config.Unity
{

    /// <summary>
    /// A environment configuration based on Unity. All requested instances
    /// will be get from the Unity .
    /// </summary>
    /// <code>
    /// var config = new UnityConfiguration(container);
    /// NcqrsEnvironment.Configure(config);
    /// </code>
    public class UnityConfiguration : IEnvironmentConfiguration
    {

        private readonly IUnityContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityConfiguration" /> class.
        /// </summary>
        /// <param name="container">The Unity Container which will provide components to Ncqrs</param>
        public UnityConfiguration(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// get instance scope configured interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryGet<T>(out T result) where T : class
        {
            result = _container.Resolve<T>();
            return result != null;
        }
    }

}
