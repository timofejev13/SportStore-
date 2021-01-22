﻿using Moq;
using Ninject;
using SportsStore.Domain.Concrete;
using SportsStore.WebUI.Infrastructure.Concrete;
using SportStore.Domain.Abstract;
using SportStore.WebUI.Infrastructure.Abstract;
using SportStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace SportStore.WebUI.Infrastructure
{
	public class NinjectDependencyResolver : IDependencyResolver
	{
		private IKernel kernel;
		public NinjectDependencyResolver(IKernel kernelParam)
		{
			kernel = kernelParam;
			AddBindings();
		}
		public object GetService(Type serviceType)
		{
			return kernel.TryGet(serviceType);
		}
		public IEnumerable<object> GetServices(Type serviceType)
		{
			return kernel.GetAll(serviceType);
		}
		private void AddBindings()
		{
			kernel.Bind<IProductRepository>().To<EFProductRepository>();
			EmailSettings emailSettings = new EmailSettings
			{
				WriteAsFile = bool.Parse(ConfigurationManager
				.AppSettings["Email.WriteAsFile"] ?? "false")
			};
			kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>()
			.WithConstructorArgument("settings", emailSettings);
			kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
		}
	}
}