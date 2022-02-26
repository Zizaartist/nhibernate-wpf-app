using Autofac;
using Autofac.Core;
using System;

namespace happy_water_carrier_test.Helpers
{
    // Взял отсюда https://github.com/oriches/Simple.Wpf.Exceptions/blob/master/Simple.Wpf.Exceptions/BootStrapper.cs
    // к сожалению более изящного подхода не нашел, придется обращаться к статике

    public class BootStrapper
    {
        private static ILifetimeScope _rootScope;

        public static void Start(ContainerBuilder builder)
        {
            if (_rootScope != null)
            {   
                return;
            }

            _rootScope = builder.Build();
        }

        public static void Stop()
        {
            _rootScope.Dispose();
        }

        public static T Resolve<T>()
        {
            if (_rootScope == null)
            {
                throw new Exception("Bootstrapper hasn't been started!");
            }

            return _rootScope.Resolve<T>(new Parameter[0]);
        }

        public static T Resolve<T>(Parameter[] parameters)
        {
            if (_rootScope == null)
            {
                throw new Exception("Bootstrapper hasn't been started!");
            }

            return _rootScope.Resolve<T>(parameters);
        }
    }
}
