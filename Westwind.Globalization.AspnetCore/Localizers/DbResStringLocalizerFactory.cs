﻿using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;

namespace Westwind.Globalization.AspnetCore
{
    public class DbResStringLocalizerFactory : IStringLocalizerFactory
    {
        private DbResourceConfiguration _config;


        private IWebHostEnvironment _host;

        public DbResStringLocalizerFactory(DbResourceConfiguration config, IWebHostEnvironment host)
        {
            _config = config;
            _host = host;
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            // strip off application base(location) if it's provided
            if (baseName != null && !string.IsNullOrEmpty(location) && baseName.StartsWith(location))
                baseName = baseName.Substring(location.Length + 1);

            return new DbResStringLocalizer(_config) { ResourceSet = baseName };
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            string baseName = resourceSource.FullName;
            return Create(baseName, _host.ApplicationName);
        }
    }
}
