using AutoMapper;
using MyAnimeWorld.Common.Utilities.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAnimeWorld.Tests.Mocks
{
    public static class MockMapper
    {
        public static IMapper GetMapper() => Mapper.Instance;

        static MockMapper()
        {
            Mapper.Initialize(options => options.AddProfile<AutoMapperProfile>());
        }
    }
}
