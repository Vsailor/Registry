using System;
using System.ServiceModel;
using Microsoft.Practices.Unity;
using Registry.Communication;
using Registry.Services;
using Registry.Services.Abstract;

namespace Registry
{
  public class RegistryRegistration
  {
    public static void Register(IUnityContainer container)
    {
      container.RegisterType<IUserService, UserService>();
      container.RegisterType<ICategoryService, CategoryService>();
      container.RegisterType<IResourceGroupService, ResourceGroupService>();
      container.RegisterType<IResourceService, ResourceService>();


      var netTcpBinding = new NetTcpBinding();

      container.RegisterInstance(
        typeof(IUserRepository), 
        new ChannelFactory<IUserRepository>(
          netTcpBinding,
          new EndpointAddress("net.tcp://localhost:8090/IUserRepository"))
        .CreateChannel());

      container.RegisterInstance(
        typeof(ICategoryRepository),
        new ChannelFactory<ICategoryRepository>(
          netTcpBinding,
          new EndpointAddress("net.tcp://localhost:8090/ICategoryRepository"))
        .CreateChannel());

      container.RegisterInstance(
        typeof(IResourceGroupRepository),
        new ChannelFactory<IResourceGroupRepository>(
          netTcpBinding,
          new EndpointAddress("net.tcp://localhost:8090/IResourceGroupRepository"))
        .CreateChannel());

      container.RegisterInstance(
        typeof(IResourceRepository),
        new ChannelFactory<IResourceRepository>(
          netTcpBinding,
          new EndpointAddress("net.tcp://localhost:8090/IResourceRepository"))
        .CreateChannel());
    }
  }
}