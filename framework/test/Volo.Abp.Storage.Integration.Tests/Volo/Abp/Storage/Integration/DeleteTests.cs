﻿using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Volo.Abp.Storage.Integration
{
    [Collection(nameof(IntegrationCollection))]
    [Trait("Operation", "Delete"), Trait("Kind", "Integration")]
    public class DeleteTests
    {
        private StoresFixture _storeFixture;

        public DeleteTests(StoresFixture fixture)
        {
            _storeFixture = fixture;
        }

        [Theory(DisplayName = nameof(Delete)), InlineData("Store1"), InlineData("Store2"), InlineData("Store3"), InlineData("Store4"), InlineData("Store5"), InlineData("Store6")]
        public async Task Delete(string storeName)
        {
            var storageFactory = _storeFixture.Services.GetRequiredService<IAbpStorageFactory>();

            var store = storageFactory.GetStore(storeName);

            var file = await store.GetBlobAsync("Delete/ToDelete.txt");

            await file.DeleteBlobAsync();

            Assert.Null(await store.GetBlobAsync("Delete/ToDelete.txt"));
            Assert.NotNull(await store.GetBlobAsync("Delete/ToSurvive.txt"));
        }
    }
}
