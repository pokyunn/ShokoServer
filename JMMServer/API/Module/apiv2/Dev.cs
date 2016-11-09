﻿using Nancy;
using Nancy.Security;
using System;
using Nancy.ModelBinding;
using JMMServer.Entities;
using JMMContracts;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;
using JMMServer.Commands;
using JMMServer.PlexAndKodi;
using JMMServer.Repositories;
using JMMServer.Tasks;
using Nancy.Responses;

namespace JMMServer.API.Module.apiv2
{
    public class Dev : Nancy.NancyModule
    {
        public static int version = 1;

        public Dev() : base("/api/dev")
        {
            #if DEBUG
            Get["/contracts/{entity?}"] = x => { return ExtractContracts((string)x.entity); };
            
            #endif
        }

        /// <summary>
        /// Dumps the contracts as JSON files embedded in a zip file.
        /// </summary>
        /// <param name="entityType">The type of the entity to dump (can be <see cref="string.Empty"/> or <c>null</c> to dump all).</param>
        private object ExtractContracts(string entityType)
        {
            var zipStream = new ContractExtractor().GetContractsAsZipStream(entityType);

            return new StreamResponse(() => zipStream, "application/zip").AsAttachment("contracts.zip");
        }
    }
}