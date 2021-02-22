﻿using Shouldly;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.Authorization.TestServices
{
    public class AuthorizationTestPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var getGroup = context.GetGroupOrNull("TestGetGroup");
            if (getGroup == null)
            {
                getGroup = context.AddGroup("TestGetGroup");
            }

            var group = context.AddGroup("TestGroup");

            group.AddPermission("MyAuthorizedService1");

            group.AddPermission("MenuPermission1");
            group.AddPermission("MenuPermission2");
            group.AddPermission("MenuPermission3");
            group.AddPermission("MenuPermission4");

            group.GetPermissionOrNull("MyAuthorizedService1").ShouldNotBeNull();

            context.RemoveGroup("TestGetGroup");
        }
    }
}
