using Generic.Config;

namespace Generic.DataAccess
{
    /// <summary>
    /// It is used to avoid injecting few frequently used classes everywhere. 
    /// And make write few utility (e.g. db lookup) easier
    /// 
    /// It can also help inject Transient scope object to signlton scope object 
    /// </summary>
    /// </summary>
    public class DataRepositoryTool
    {
        public static IDataRepository GetDataRepository()
        {
            return CfgEnvironment.Get<IDataRepository>();
        }
    }
}
