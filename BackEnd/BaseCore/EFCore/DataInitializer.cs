using BaseCore.Utilities;

namespace BaseCore.EFCore
{
    public interface IDataInitializer : IScopedDependency
    {
        void InitializeDataAsync();
    }
}
