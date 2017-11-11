using System;
using System.Linq;
using System.ServiceModel;
using Microsoft.Practices.Unity;
using Registry.Common;
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

      ReregisterWcfServices(container);
    }

    public static void ReregisterWcfServices(IUnityContainer container)
    {
      var netTcpBinding = new NetTcpBinding();

      foreach (var registration in container.Registrations
     .Where(p => p.RegisteredType == typeof(IUserRepository) ||
                 p.RegisteredType == typeof(ICategoryRepository) ||
                 p.RegisteredType == typeof(IResourceGroupRepository) ||
                 p.RegisteredType == typeof(IResourceRepository)
                 && p.LifetimeManagerType == typeof(ContainerControlledLifetimeManager)))
      {
        registration.LifetimeManager.RemoveValue();
      }

      var userClient = (new ChannelFactory<IUserRepository>(
        netTcpBinding,
        new EndpointAddress("net.tcp://localhost:8090/IUserRepository"))).CreateChannel();
      ((ICommunicationObject)userClient).Faulted += OnFaulted;
      container.RegisterInstance(
        typeof(IUserRepository),
        userClient);

      var categoryClient = (new ChannelFactory<ICategoryRepository>(
        netTcpBinding,
        new EndpointAddress("net.tcp://localhost:8090/ICategoryRepository"))).CreateChannel();
      ((ICommunicationObject)categoryClient).Faulted += OnFaulted;
      container.RegisterInstance(
        typeof(ICategoryRepository),
        categoryClient);

      var resourceGroupClient = (new ChannelFactory<IResourceGroupRepository>(
        netTcpBinding,
        new EndpointAddress("net.tcp://localhost:8090/IResourceGroupRepository"))).CreateChannel();
      ((ICommunicationObject)resourceGroupClient).Faulted += OnFaulted;
      container.RegisterInstance(
        typeof(IResourceGroupRepository),
        resourceGroupClient);

      var resourceClient = (new ChannelFactory<IResourceRepository>(
       netTcpBinding,
       new EndpointAddress("net.tcp://localhost:8090/IResourceRepository"))).CreateChannel();
      ((ICommunicationObject)resourceClient).Faulted += OnFaulted;
      container.RegisterInstance(
        typeof(IResourceRepository), 
        resourceClient);
    }

    private static void OnFaulted(object sender, EventArgs eventArgs)
    {
      ((ICommunicationObject)sender).Abort();
      if (sender is IUserRepository)
      {
        sender = (new ChannelFactory<IUserRepository>(
        new NetTcpBinding(),
        new EndpointAddress("net.tcp://localhost:8090/IUserRepository"))).CreateChannel();
        ((ICommunicationObject)sender).Faulted += OnFaulted;
        RegistryCommon.Instance.Container.RegisterInstance(
          typeof(IUserRepository),
          sender);
        return;
      }

      if (sender is ICategoryRepository)
      {
        sender = (new ChannelFactory<ICategoryRepository>(
        new NetTcpBinding(),
        new EndpointAddress("net.tcp://localhost:8090/ICategoryRepository"))).CreateChannel();
        ((ICommunicationObject)sender).Faulted += OnFaulted;
        RegistryCommon.Instance.Container.RegisterInstance(
          typeof(ICategoryRepository),
          sender);
        return;
      }

      if (sender is IResourceGroupRepository)
      {
        sender = (new ChannelFactory<IResourceGroupRepository>(
        new NetTcpBinding(),
        new EndpointAddress("net.tcp://localhost:8090/IResourceGroupRepository"))).CreateChannel();
        ((ICommunicationObject)sender).Faulted += OnFaulted;
        RegistryCommon.Instance.Container.RegisterInstance(
          typeof(IResourceGroupRepository),
          sender);
        return;
      }

      if (sender is IResourceRepository)
      {
        sender = (new ChannelFactory<IResourceRepository>(
        new NetTcpBinding(),
        new EndpointAddress("net.tcp://localhost:8090/IResourceRepository"))).CreateChannel();
        ((ICommunicationObject)sender).Faulted += OnFaulted;
        RegistryCommon.Instance.Container.RegisterInstance(
          typeof(IResourceRepository),
          sender);
      }
    }
  }
}