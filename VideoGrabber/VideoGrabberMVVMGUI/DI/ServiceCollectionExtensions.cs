using Microsoft.Extensions.DependencyInjection;
using VideoGrabberMVVMGUI.Models;

namespace VideoGrabberMVVMGUI.DI
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection collection)
        {
            collection.AddSingleton<IRepository, Repository>();
            collection.AddTransient<IBusinessService, BusinessService>();
            collection.AddTransient<MainViewModel>();
        }
    }
}
