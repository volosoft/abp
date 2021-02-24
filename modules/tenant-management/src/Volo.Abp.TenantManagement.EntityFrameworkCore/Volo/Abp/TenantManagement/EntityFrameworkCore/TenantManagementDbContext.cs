﻿using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.TenantManagement.EntityFrameworkCore
{
    [IgnoreMultiTenancy]
    [ConnectionStringName(AbpTenantManagementDbProperties.ConnectionStringName)]
    public class TenantManagementDbContext : AbpDbContext<TenantManagementDbContext>, ITenantManagementDbContext
    {
        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

        public TenantManagementDbContext(DbContextOptions<TenantManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.NamingConventionsRewriteName(AbpCommonDbProperties.DbNamingConvention);

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureTenantManagement();

            builder.NamingConventionsRewriteName(AbpCommonDbProperties.DbNamingConvention);

        }

    }
}
