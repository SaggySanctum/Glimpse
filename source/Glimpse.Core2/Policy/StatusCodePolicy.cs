﻿using System;
using System.Collections.Generic;
using System.Configuration;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Resource;

namespace Glimpse.Core2.Policy
{
    [RuntimePolicy(RuntimeEvent.EndRequest)]
    public class StatusCodePolicy:ConfigurationSection, IRuntimePolicy
    {
        public IList<int> StatusCodeWhitelist { get; set; }

        public StatusCodePolicy()
        {
            StatusCodeWhitelist = new List<int>{200, 301, 302};
        }

        public StatusCodePolicy(IList<int> statusCodeWhitelist)
        {
            StatusCodeWhitelist = statusCodeWhitelist;
        }

        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            try
            {
                var statusCode = policyContext.RequestMetadata.ResponseStatusCode;
                return StatusCodeWhitelist.Contains(statusCode) ? RuntimePolicy.On : RuntimePolicy.Off;
            }
            catch (Exception exception)
            {
                policyContext.Logger.Warn(string.Format(Resources.ExecutePolicyWarning, GetType()), exception);
                return RuntimePolicy.Off;
            }
        }
    }
}