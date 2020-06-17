﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Settings;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.SettingManagement
{
    public class SettingManagementStore_Tests : SettingsTestBase
    {
        private readonly ISettingManagementStore _settingManagementStore;
        private readonly ISettingRepository _settingRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly SettingTestData _testData;

        public SettingManagementStore_Tests()
        {
            _settingManagementStore = GetRequiredService<ISettingManagementStore>();
            _settingRepository = GetRequiredService<ISettingRepository>();
            _unitOfWorkManager= GetRequiredService<IUnitOfWorkManager>();
            _testData = GetRequiredService<SettingTestData>();
        }

        [Fact]
        public async Task GetOrNull_NotExist_Should_Be_Null()
        {
            var value = await _settingManagementStore.GetOrNullAsync("notExistName", "notExistProviderName",
                "notExistProviderKey");

            value.ShouldBeNull();
        }

        [Fact]
        public async Task GetOrNullAsync()
        {
            var value = await _settingManagementStore.GetOrNullAsync("MySetting1", GlobalSettingValueProvider.ProviderName, null);

            value.ShouldNotBeNull();
            value.ShouldBe("42");
        }

        [Fact]
        public async Task SetAsync()
        {
            var setting = await _settingRepository.FindAsync(_testData.SettingId);
            setting.Value.ShouldBe("42");

            await _settingManagementStore.SetAsync("MySetting1", "43", GlobalSettingValueProvider.ProviderName, null);

            (await _settingRepository.FindAsync(_testData.SettingId)).Value.ShouldBe("43");
        }


        [Fact]
        public async Task Set_In_UnitOfWork_Should_Be_Consistent()
        {
            using (_unitOfWorkManager.Begin())
            {
                var value = await _settingManagementStore.GetOrNullAsync("MySetting1", GlobalSettingValueProvider.ProviderName, null);
                value.ShouldBe("42");

                await _settingManagementStore.SetAsync("MySetting1", "43", GlobalSettingValueProvider.ProviderName, null);

                var valueAfterSet = await _settingManagementStore.GetOrNullAsync("MySetting1", GlobalSettingValueProvider.ProviderName, null);
                valueAfterSet.ShouldBe("43");
            }
        }

        [Fact]
        public async Task DeleteAsync()
        {
            (await _settingRepository.FindAsync(_testData.SettingId)).ShouldNotBeNull();

            await _settingManagementStore.DeleteAsync("MySetting1", GlobalSettingValueProvider.ProviderName, null);

            (await _settingRepository.FindAsync(_testData.SettingId)).ShouldBeNull();
        }

        [Fact]
        public async Task Delete_In_UnitOfWork_Should_Be_Consistent()
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                (await _settingManagementStore.GetOrNullAsync("MySetting1", GlobalSettingValueProvider.ProviderName, null)).ShouldNotBeNull();

                await _settingManagementStore.DeleteAsync("MySetting1", GlobalSettingValueProvider.ProviderName, null);

                await uow.SaveChangesAsync();

                var value = await _settingManagementStore.GetOrNullAsync("MySetting1", GlobalSettingValueProvider.ProviderName, null);
                value.ShouldBeNull();
            }
        }
    }
}
