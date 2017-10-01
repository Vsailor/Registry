﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Unity;
using Registry;

namespace Registry.UI
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      RegistryCommon.Instance.Container = new UnityContainer();
      RegistryRegistration.Register(RegistryCommon.Instance.Container);
    }
  }
}