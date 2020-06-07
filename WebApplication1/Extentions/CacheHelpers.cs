﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Extentions
{
    public class CacheHelpers
    {
        public static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromSeconds(30);
        private static readonly string _itemsKeyTemplate = "items-{0}-{1}-{2}-{3}";

        public static string GenerateCatalogItemCacheKey(int pageIndex, int itemsPage, int? brandId, int? typeId)
        {
            // return using string template
            return string.Format(_itemsKeyTemplate, pageIndex, itemsPage, brandId, typeId);
        }

        public static string GenerateCatalogBrandCacheKey()
        {
            return "brands";
        }

        public static string GenerateCatalogTypeCacheKey()
        {
            return "types";
        }
    }
}
