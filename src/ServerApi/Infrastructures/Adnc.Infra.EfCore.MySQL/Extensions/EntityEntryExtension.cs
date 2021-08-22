﻿using Adnc.Infra.EfCore.Internal;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq;

namespace Adnc.Infra.EfCore.Extensions
{
    public static class EntityEntryExtension
    {
        public static KeyEntryModel[] GetKeyValues([NotNull] this EntityEntry entityEntry)
        {
            if (!entityEntry.IsKeySet)
                return default;

            var keyProps = entityEntry.Properties
                .Where(p => p.Metadata.IsPrimaryKey())
                .ToArray();
            if (keyProps.Length == 0)
                return default;

            var keyEntries = new KeyEntryModel[keyProps.Length];
            for (var i = 0; i < keyProps.Length; i++)
            {
                keyEntries[i] = new KeyEntryModel()
                {
                    PropertyName = keyProps[i].Metadata.Name,
                    ColumnName = (keyProps[i].Metadata as PropertyEntry).GetColumnName(),
                    Value = keyProps[i].CurrentValue,
                };
            }

            return keyEntries;
        }

        public static string GetColumnName(this PropertyEntry @this)
        {
            var storeObjectId =
                        StoreObjectIdentifier.Create(@this.Metadata.DeclaringEntityType, StoreObjectType.Table);
            return @this.Metadata.GetColumnName(storeObjectId.GetValueOrDefault());
        }
    }
}
